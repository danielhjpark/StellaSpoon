using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager instance;

    [SerializeField]
    private int poolSize = 10;

    // 무기 레벨별 프리팹 리스트 (레벨 -> 프리팹 타입 -> 리스트)
    private Dictionary<int, Dictionary<string, List<GameObject>>> poolDict = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동 시 파괴되지 않게 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetObject(int weaponLevel, string type, GameObject prefab)
    {
        if (!poolDict.ContainsKey(weaponLevel))
        {
            poolDict[weaponLevel] = new Dictionary<string, List<GameObject>>();
        }

        if (!poolDict[weaponLevel].ContainsKey(type))
        {
            poolDict[weaponLevel][type] = new List<GameObject>();
        }

        var pool = poolDict[weaponLevel][type];

        // 비활성화된 오브젝트 재사용
        for (int i = pool.Count - 1; i >= 0; i--)
        {
            GameObject obj = pool[i];
            if (obj == null)
            {
                pool.RemoveAt(i);
                continue;
            }

            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // 새로 생성
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);

        if (newObj.GetComponent<ParticleSystem>() != null &&
            newObj.GetComponent<AutoDisableOnParticleEnd>() == null)
        {
            newObj.AddComponent<AutoDisableOnParticleEnd>();
        }

        pool.Add(newObj);
        return newObj;
    }
}


