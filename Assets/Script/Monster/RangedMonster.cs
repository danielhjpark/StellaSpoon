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
        if (!isAttack && Time.time - lastAttackTime >= damageDelayTime)
        {
            lastAttackTime = Time.time;
            isAttack = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true); //Attack 애니메이션 실행

            //함수로 애니메이션 이벤트를 이용해 Bullet 생성
        }
        //공격 구현
    }

    
    public void InstantBullet() //Bullet 생성 함수
    {
        GameObject bulletObj = Instantiate(projectilePrefab, AttackTF.position, AttackTF.rotation);
        RangedBullet bullet = bulletObj.GetComponent<RangedBullet>();
        bullet.damage = this.damage; //자식에게 데미지 전달
        bullet.thirdPersonController = this.thirdPersonController; //자식에게 ThirdPersonController 전달
    }
}
