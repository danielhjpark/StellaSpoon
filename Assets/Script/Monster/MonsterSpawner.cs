using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterprefab; //���� ������
    [SerializeField]
    private int maxMonsterCount; //�ִ� ���� ��
    [SerializeField]
    private float randomSpawnRange; //���� ���� ����
    [SerializeField]
    private float monsterCheckRange; //������ ���� �浹 üũ ����

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
        Vector3 spawnposition; //���� ��ġ
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
    protected virtual void OnDrawGizmos()//�׻� ���̰� //���ý� ���̰� OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //���� ����
        Gizmos.DrawWireSphere(transform.position, randomSpawnRange);
    }
}
