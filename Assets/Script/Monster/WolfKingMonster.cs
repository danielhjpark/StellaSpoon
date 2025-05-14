using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKingMonster : MonsterBase
{
    private int nextPattern = 0;

    private static readonly int THROW = 0; //������
    private static readonly int PILLAR = 1; //��ռ����
    private static readonly int SPAWN = 2; //����

    [Header("Throrw")]
    [SerializeField]
    private Transform[] throwPosition; //������ ��ġ
    [SerializeField]
    private GameObject throwObjectPrefab; //������ ������Ʈ ������
    private int throwcount = 0; //������ Ƚ��

    [Header("Pillar")]
    [SerializeField]
    private float pillarRange = 5.0f; //��� ����� ����
    [SerializeField]
    private GameObject pillarPrefab; //��� ������
    private int pillarCount = 0; //��� ����� Ƚ��
    [SerializeField]
    private Transform[] pillarPosition; //��� ����� ��ġ

    [Header("Spawn")]
    [SerializeField]
    private GameObject silverSpawnPrefab; //�ǹ����� ���� ������
    [SerializeField]
    private GameObject novaSpawnPrefab; //���ٿ��� ���� ������
    [SerializeField]
    private float spawnRange = 5f; //���� ����
    private int silverSpawnCount = 2; //�ǹ����� ���� Ƚ��
    private int novaSpawnCount = 1; //��ٿ��� ���� Ƚ��


    protected override void HandleAttack()
    {
        animator.SetBool("Walk", false);
        if (!isAttack)
        {
            isAttack = true;
            StartCoroutine(Throw());
        }
    }
    private IEnumerator Throw()
    {
        if (!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        //animator.SetTrigger("Attack8");
        foreach (Transform pos in throwPosition) //��ô�� ����
        {
            GameObject throwObject = Instantiate(throwObjectPrefab, pos.position, Quaternion.identity);
        }
        throwcount++;
        if (throwcount < 2)
        {
            yield return new WaitForSeconds(3.0f); //3�� ���
            nextPattern = THROW;
            nextPatternPlay();
        }
        else
        {
            throwcount = 0;
            yield return new WaitForSeconds(4.0f); //4�� ���
            nextPattern = PILLAR;
            nextPatternPlay();
        }
    }

    private IEnumerator Pillar()
    {
        if (!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        pillarCount = Random.Range(4, 6); //��� ����� Ƚ��

        yield return new WaitForSeconds(2.0f); //2�� ���
        //�ִϸ��̼� ���

        for (int i = 0; i < pillarCount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-pillarRange, pillarRange), 0, Random.Range(-pillarRange, pillarRange));
            GameObject pillar = Instantiate(pillarPrefab, transform.position + randomPos, Quaternion.identity);
            Destroy(pillar, 5.0f); //5�� �� ��� ����
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(5.0f); //5�� ���
        nextPattern = SPAWN;
        nextPatternPlay();
    }

    private IEnumerator Spawn()
    {
        if (!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        yield return new WaitForSeconds(2.0f); //2�� ���
        //�ִϸ��̼� ���
        for (int i = 0; i < silverSpawnCount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            GameObject silverWolf = Instantiate(silverSpawnPrefab, transform.position + randomPos, Quaternion.identity);
        }
        for (int i = 0; i < novaSpawnCount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            GameObject novaWolf = Instantiate(novaSpawnPrefab, transform.position + randomPos, Quaternion.identity);
        }
        yield return new WaitForSeconds(3.0f); //3�� ���
    }
    protected void nextPatternPlay()
    {
        switch (nextPattern)
        {
            case 0:
                StartCoroutine(Throw());
                break;
            case 1:
                StartCoroutine(Pillar());
                break;
            case 2:
                StartCoroutine(Spawn());
                break;
        }
    }
}
