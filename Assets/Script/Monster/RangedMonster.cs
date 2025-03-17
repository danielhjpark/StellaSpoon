using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedMonster : MonsterBase
{
    [Header("���Ÿ� ���� information")]
    [SerializeField]
    private GameObject projectilePrefab; //���� ����ü
    [SerializeField]
    private Transform AttackTF; //����ü ���� ��ġ

    protected override void HandleAttack()
    {
        base.HandleAttack();
        // ���� �ð��� ������ ���� �ð� + ��Ÿ�� �̻����� Ȯ��
        if (Time.time >= lastAttackTime + damageDelayTime)
        {
            lastAttackTime = Time.time; // ������ ���� �ð� ������Ʈ
            Instantiate(projectilePrefab, AttackTF.position, AttackTF.rotation);
        }
    }
}
