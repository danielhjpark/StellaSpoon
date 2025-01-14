using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMonster : MonsterBase
{
    [Header("Basic Information")]
    [SerializeField]
    private float attackDamage = 10f; //공격 데미지
    [SerializeField]
    private float attackRange = 2f; //공격 범위

    [SerializeField]
    private float wanderRadius = 15f; //랜덤으로 움직이는 범위
    [SerializeField]
    private float wanderTime = 3; //랜덤 이동 주기

    private float wanderTimer;

    private bool isPlayerDetected = false; //플레이어 감지 상태

    private Vector3 initialPosition; //초기 위치 저장

    private NavMeshAgent agent;//몬스터의 NavMeshAgent
    [SerializeField]
    private Collider collider;

    private void Start()
    {
        base.Start(); //부모 클래스 초기화
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
        wanderTimer = wanderTime;

        initialPosition = transform.position; //초기 위치 저장
    }

    private void Update()
    {
        if (player.transform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //감지 범위 안에 있는 경우 플레이어를 따라감
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            FollowPlayer();
            isPlayerDetected = true;
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
        //감지 범위를 벗어난 경우 랜덤 위치로 이동
        else
        {
            if (isPlayerDetected)
            {
                isPlayerDetected = false;
                if (agent.hasPath)
                {
                    agent.ResetPath();// 기존 경로 초기화
                }
            }
            HandleRandomMovement();
        }
    }

    private void HandleRandomMovement()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderTime)
        {
            Vector3 newDestination = getRandomPoint(initialPosition, wanderRadius);
            agent.SetDestination(newDestination);
            wanderTimer = 0f;
            animator.SetBool("Walk", true);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetBool("Walk", false);
        }
    }

    private Vector3 getRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            return hit.position;
        }
        return center;
    }

    private void FollowPlayer()
    {
        if (agent != null && player.transform != null)
        {
            animator.SetBool("Walk", true);
            agent.isStopped = false;
            agent.SetDestination((player.transform.position) - (player.transform.position - transform.position).normalized * 0.5f);//목표보다 0.4 앞에 멈추게
            wanderTimer = 0f;
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
        Vector3 attackerPosition = transform.position; // 플레이어를 공격하는 방향
                                                       //여기에 플레이어에게 데미지를 입히는 로직 추가
        thirdPersonController.TakeDamage(attackDamage, attackerPosition);
        /*animator.SetBool("Attack", false);*/
    }

    // 감지 및 공격 범위 시각화
    private void OnDrawGizmos()//항상 보이게 //선택시 보이게 OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //감지 범위
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue; // 공격 범위
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green; //움직임 범위
        Gizmos.DrawWireSphere(initialPosition, wanderRadius);
    }

    public override void Damage(int bulletDamage)
    {
        base.Damage(bulletDamage);
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
    }

    protected override void Die()
    {
        base.Die();
        agent.isStopped = true;//이동 멈추기
        collider.enabled = false;//충돌 제거
        animator.SetTrigger("Die");
        StartCoroutine(DieDelays());
    }

    IEnumerator DieDelays()
    {
        yield return new WaitForSeconds(dieDelay);

        Destroy(gameObject);
    }
}
