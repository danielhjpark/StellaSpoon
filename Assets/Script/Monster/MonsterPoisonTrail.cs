using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoisonTrail : MonoBehaviour
{
    public GameObject poisonPrefab; //ƽ������ ������ ������
    public float spawnInterval = 0.5f; //������ ���� ����
    public float poisonDuration = 5f; //������ ���� �ð�


    private List<GameObject> poisonClouds = new List<GameObject>(); //������ ������ ���

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
