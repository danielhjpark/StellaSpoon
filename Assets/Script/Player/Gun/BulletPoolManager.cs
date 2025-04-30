using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager instance;

    [SerializeField]
    private int poolSize = 10;

    // ���� ������ ������ ����Ʈ (���� -> ������ Ÿ�� -> ����Ʈ)
    private Dictionary<int, Dictionary<string, List<GameObject>>> poolDict = new();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
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

        // ��Ȱ��ȭ�� ������Ʈ ����
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // ���� ����
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


