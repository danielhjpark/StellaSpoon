using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedMonster : MonsterBase1
{
    [SerializeField]
    private GameObject projectilePrefab; //공격 투사체
    [SerializeField]
    private Transform AttackTF; //투사체 생성 위치
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
        nav.avoidancePriority = Random.Range(30, 60); // 회피 우선순위를 랜덤으로 설정
    }

    protected override void HandleAttack()
    {
        if(!isAttack || Time.time - lastAttackTime >= damageDelayTime)
        {
            lastAttackTime = Time.time;
            isAttack = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true); //Attack 애니메이션 실행
            Instantiate(projectilePrefab, AttackTF.position, AttackTF.rotation);
        }
        //공격 구현
    }
}
