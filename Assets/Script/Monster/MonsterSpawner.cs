using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterprefab; //몬스터 프리팹
    [SerializeField]
    private int maxMonsterCount; //최대 몬스터 수
    [SerializeField]
    private float randomSpawnRange; //랜덤 스폰 범위
    [SerializeField]
    private float monsterCheckRange; //스폰시 몬스터 충돌 체크 범위

    private List<GameObject> monsters = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < maxMonsterCount; i++)
        {
            SpawnMonster();
        }
    }
    private void SpawnMonster()
    {
        Vector3 spawnposition; //스폰 위치
        bool validPosition = false;

        while (!validPosition)
        {
            spawnposition = transform.position + new Vector3(Random.Range(-randomSpawnRange, randomSpawnRange), 0, Random.Range(-randomSpawnRange, randomSpawnRange));

            Collider[] colliders = Physics.OverlapSphere(spawnposition, monsterCheckRange);
            validPosition = true;

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Monster"))
                {
                    validPosition = false;
                    break;
                }
            }

            if (validPosition)
            {
                GameObject monster = Instantiate(monsterprefab, spawnposition, Quaternion.identity, transform);

                monsters.Add(monster);
                StartCoroutine(RespawnMonster(monster));
            }
        }
    }
    IEnumerator RespawnMonster(GameObject monster)
    {
        while (true)
        {
            yield return new WaitUntil(() => !monster.activeSelf);
            yield return new WaitForSeconds(5);
            Vector3 spawnPosition;
            bool validPosition = false;

            while (!validPosition)
            {
                spawnPosition = transform.position + new Vector3(Random.Range(-randomSpawnRange, randomSpawnRange), 0, Random.Range(-randomSpawnRange, randomSpawnRange));

                Collider[] colliders = Physics.OverlapSphere(spawnPosition, monsterCheckRange);
                validPosition = true;

                foreach (Collider col in colliders)
                {
                    if (col.CompareTag("Monster"))
                    {
                        validPosition = false;
                        break;
                    }
                }

                if (validPosition)
                {
                    monster.transform.position = spawnPosition;
                    monster.SetActive(true);
                }
            }
        }
    }
    protected virtual void OnDrawGizmos()//항상 보이게 //선택시 보이게 OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //감지 범위
        Gizmos.DrawWireSphere(transform.position, randomSpawnRange);
    }
}
