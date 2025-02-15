using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMonster : MonsterBase1
{
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

    // 감지 및 공격 범위 시각화
    private void OnDrawGizmos()//항상 보이게 //선택시 보이게 OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //감지 범위
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);

        Gizmos.color = Color.blue; // 공격 범위
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green; //움직임 범위
        Gizmos.DrawWireSphere(initialPosition, randomMoveRange);

        Gizmos.color = Color.magenta; //플레이어 공격 인지 범위 
        Gizmos.DrawWireSphere (transform.position, damageRange);
    }
}
