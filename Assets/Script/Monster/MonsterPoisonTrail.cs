using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoisonTrail : MonoBehaviour
{
    public GameObject poisonPrefab; //틱데미지 독구름 프리팹
    public float spawnInterval = 0.5f; //독구름 생성 간격
    public float poisonDuration = 5f; //독구름 지속 시간


    private List<GameObject> poisonClouds = new List<GameObject>(); //생성된 독구름 목록

    private void Start()
    {
        StartCoroutine(SpawnPoisonTrail());
    }
    IEnumerator SpawnPoisonTrail()
    {
        while (true)
        {
            GameObject poison = Instantiate(poisonPrefab, transform.position, Quaternion.identity);
            poisonClouds.Add(poison);

            StartCoroutine(DestroyPoisonAfterDelay(poison, poisonDuration));
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator DestroyPoisonAfterDelay(GameObject poison, float delay)
    {
        yield return new WaitForSeconds(delay);
        poisonClouds.Remove(poison);
        Destroy(poison);
    }
}
