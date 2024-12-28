using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMonster : MonsterBase
{
    public float attackDamage = 10f;  //공격 데미지
    public float attackRange = 2f;   //공격 범위
    private NavMeshAgent agent;      //몬스터의 NavMeshAgent
    private Animator animator;
    [SerializeField]
    private Collider collider;

    void Start()
    {
        base.Start(); //부모 클래스 초기화
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (playerTf == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTf.position);

        //감지 범위 안에 있는 경우 플레이어를 따라감
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            FollowPlayer();
        }
        //공격 범위 안에 있는 경우 공격
        else if (distanceToPlayer <= attackRange)
        {
            StopMoving();
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        //감지 범위를 벗어난 경우 멈춤
        else
        {
            StopMoving();
        }
    }

    private void FollowPlayer()
    {
        if (agent != null && playerTf != null)
        {
            animator.SetBool("Walk", true);
            agent.isStopped = false;
            agent.SetDestination(playerTf.position);
        }
    }

    private void StopMoving()
    {
        if (agent != null)
        {
            animator.SetBool("Walk", false);
            agent.isStopped = true;
        }
    }

    protected override void Attack()
    {
        animator.SetTrigger("Attack");
        Debug.Log("근접 공격: " + attackDamage + "의 피해를 입힘!");
        //여기에 플레이어에게 데미지를 입히는 로직 추가
        
        /*animator.SetBool("Attack", false);*/
    }

    // 감지 및 공격 범위 시각화
    void OnDrawGizmos() //항상 보이게 //선택시 보이게 OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //감지 범위
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue; // 공격 범위
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected override void Damage()
    {
        
    }

    protected override void Die()
    {
        agent.isStopped = true; //이동 멈추기
        collider.enabled = false; //충돌 제거
        animator.SetTrigger("Die");
        StartCoroutine(DieDelays());
        Destroy(gameObject);
    }

    IEnumerator DieDelays()
    {
        yield return new WaitForSeconds(DieDelay);
    }
}
