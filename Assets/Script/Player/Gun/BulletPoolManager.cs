using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager instance;

    [SerializeField]
    private GameObject[] prefabs;
    private int poolSize = 10;
    private List<GameObject>[] objPools;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }
    void Start()
    {
        instance = this;
        InitObjPool();
    }

    private void InitObjPool()
    {
        objPools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < prefabs.Length; i++)
        {
            objPools[i] = new List<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject obj = Instantiate(prefabs[i]);
                obj.SetActive(false);
                DontDestroyOnLoad(obj);
                objPools[i].Add(obj);
            }
        }
    }

    public GameObject ActivateObj(int index)
    {
        GameObject obj = null;

        for (int i = 0; i < objPools[index].Count; i++)
        {
            if (!objPools[index][i].activeInHierarchy)
            {
                obj = objPools[index][i];
                obj.SetActive(true);
                return obj;
            }
        }

        // 새로 생성
        obj = Instantiate(prefabs[index]);

        // 파티클이면 자동으로 AutoDisable 붙이기
        if (obj.GetComponent<ParticleSystem>() != null &&
            obj.GetComponent<AutoDisableOnParticleEnd>() == null)
        {
            obj.AddComponent<AutoDisableOnParticleEnd>();
        }

        objPools[index].Add(obj);
        obj.SetActive(true);

        return obj;
    }
}
