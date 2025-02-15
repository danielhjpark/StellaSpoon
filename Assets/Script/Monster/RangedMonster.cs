using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedMonster : MonsterBase1
{
    [SerializeField]
    private GameObject projectilePrefab; //���� ����ü
    [SerializeField]
    private Transform AttackTF; //����ü ���� ��ġ
    private void Start()
    {
        base.Start();
        maxHealth = 100;
        currentHealth = maxHealth;
        damage = 10;
        idleMoveInterval = 2f;
        damageDelayTime = 5f;


        isDead = false;
        isMove = false;

        attackRange = 3f;
        playerDetectionRange = 5f;
        randomMoveRange = 7f;
        damageRange = 10f;
        nav.avoidancePriority = Random.Range(30, 60); // ȸ�� �켱������ �������� ����
    }

    protected override void HandleAttack()
    {
        if(!isAttack || Time.time - lastAttackTime >= damageDelayTime)
        {
            lastAttackTime = Time.time;
            isAttack = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true); //Attack �ִϸ��̼� ����
            Instantiate(projectilePrefab, AttackTF.position, AttackTF.rotation);
        }
        //���� ����
    }
}
