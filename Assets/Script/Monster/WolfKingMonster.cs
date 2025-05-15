using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WolfKingMonster : MonsterBase
{
    private int nextPattern = 0;

    private static readonly int THROW = 0; //������
    private static readonly int PILLAR = 1; //��ռ����
    private static readonly int SPAWN = 2; //����

    [Header("Throrw")]
    [SerializeField]
    private Transform[] throwPosition; //��ô�� ��ġ
    [SerializeField]
    private GameObject throwObjectPrefab; //��ô�� ������Ʈ ������
    private int throwcount = 0; //��ô�� Ƚ��
    [SerializeField]
    private GameObject warningPrefab; // ���� ��� ������

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

        // �غ� �� �÷��̾��� ���� ��ġ ����
        Vector3 targetPosition = player.transform.position;
        foreach (Transform pos in throwPosition)
        {
            //�÷��̾�� �߰����� ���
            Vector3 middlePosition = transform.position + ((targetPosition - transform.position) / 2);
            // 1) ��� ������Ʈ ����
            GameObject warning = Instantiate(warningPrefab, new Vector3(middlePosition.x, 0.01f, middlePosition.z), pos.rotation);
            // 2) y�� ȸ������ 90���� ����
            Vector3 warnRot = warning.transform.rotation.eulerAngles;
            warnRot.x = 90;
            warning.transform.rotation = Quaternion.Euler(warnRot);
            // 3) ���� 10, �� 1�� ������ ���� (Quad ����)
            warning.transform.localScale = new Vector3(1, 5, 1);
            // 4) 3�� �� ����
            Destroy(warning, 3f);

            // ���� ��ô�� ����
            GameObject throwObject = Instantiate(throwObjectPrefab, pos.position, pos.rotation);
            throwObject.GetComponent<LunaWolfKingBullet>().thirdPersonController = thirdPersonController;
            throwObject.GetComponent<LunaWolfKingBullet>().damage = damage;
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
