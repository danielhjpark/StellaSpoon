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
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ���� ����
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

        // ���� ����
        obj = Instantiate(prefabs[index]);

        // ��ƼŬ�̸� �ڵ����� AutoDisable ���̱�
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
