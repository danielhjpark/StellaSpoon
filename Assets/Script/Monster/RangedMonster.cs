using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedMonster : MonsterBase
{
    [Header("원거리 몬스터 information")]
    [SerializeField]
    private GameObject projectilePrefab; //공격 투사체
    [SerializeField]
    private Transform AttackTF; //투사체 생성 위치

    protected override void HandleAttack()
    {
        base.HandleAttack();
        // 현재 시간이 마지막 공격 시간 + 쿨타임 이상인지 확인
        if (Time.time >= lastAttackTime + damageDelayTime)
        {
            lastAttackTime = Time.time; // 마지막 공격 시간 업데이트
            Instantiate(projectilePrefab, AttackTF.position, AttackTF.rotation);
        }
    }
}
