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
        if (!isAttack && Time.time - lastAttackTime >= damageDelayTime)
        {
            lastAttackTime = Time.time;
            isAttack = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true); //Attack �ִϸ��̼� ����

            //�Լ��� �ִϸ��̼� �̺�Ʈ�� �̿��� Bullet ����
        }
        //���� ����
    }

    
    public void InstantBullet() //Bullet ���� �Լ�
    {
        GameObject bulletObj = Instantiate(projectilePrefab, AttackTF.position, AttackTF.rotation);
        RangedBullet bullet = bulletObj.GetComponent<RangedBullet>();
        bullet.damage = this.damage; //�ڽĿ��� ������ ����
        bullet.thirdPersonController = this.thirdPersonController; //�ڽĿ��� ThirdPersonController ����
    }
}
