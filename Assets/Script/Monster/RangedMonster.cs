using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedMonster : MonsterBase
{
    [Header("Basic Information")]
    [SerializeField]
    protected float attackDamage = 10f;  //공격 데미지
    [SerializeField]
    private float attackRange = 7f;   //공격 범위

    [SerializeField]
    private float wanderRadius = 15f; //랜덤으로 움직이는 범위
    [SerializeField]
    private float wanderTime = 3f; //랜덤 이동 주기

    private float wanderTimer;

    private bool isPlayerDetected = false; //플레이어 감지 상태

    private Vector3 initialPosition; //초기 위치 저장

    [SerializeField]
    private GameObject projectilePrefab; //투척물 프리팹
    [SerializeField]
    private Transform firePoint; //투척물 생성위치

    private NavMeshAgent agent;      //몬스터의 NavMeshAgent

    [Header("Item")]
    public GameObject[] RangedItems;
    private int maxItem = 3;
    private float APercent = 99f;
    private float BPercent = 30f;
    private float CPercent = 10f;

    private new void Start()
    {
        base.Start(); //부모 클래스 초기화
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderTime;

        initialPosition = transform.position; //초기 위치 저장

        agent.avoidancePriority = Random.Range(30, 60); // 회피 우선순위를 랜덤으로 설정
    }

    private void Update()
    {
        if (player == null) return;

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
            Vector3 targetDis = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDis);

            // 부드럽게 회전하도록 Slerp 사용
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                //플레이어 방향을 바라보게 설정
                //플레이어를 바라봤을 때 진행
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
                    agent.ResetPath(); // 기존 경로 초기화
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
            agent.SetDestination(player.transform.position);
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!collDamage)
            {
                Debug.Log("데미지");
                Vector3 attackerPosition = transform.position; // 플레이어를 공격하는 방향
                                                               //여기에 플레이어에게 데미지를 입히는 로직 추가
                thirdPersonController.TakeDamage(attackDamage, attackerPosition);
                StartCoroutine(AttackDelay());
            }
        }
    }

    protected override void Attack()
    {
        animator.SetTrigger("Attack");
        Debug.Log("원거리 공격!");
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        //여기에 플레이어에게 데미지를 입히는 로직 추가

        /*animator.SetBool("Attack", false);*/
    }

    // 감지 및 공격 범위 시각화
    private void OnDrawGizmos() //항상 보이게 //선택시 보이게 OnDrawGizmosSelected
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
    }

    protected override void DropItem()
    {
        for (int i = 0; i < maxItem; i++)
        {
            float itemPercent = Random.Range(0f, 100f);
            GameObject itemToDrop = null;
            if (i == 0 && itemPercent <= APercent)
            {
                itemToDrop = RangedItems[0];
            }
            else if (i == 1 && itemPercent <= BPercent)
            {
                itemToDrop = RangedItems[1];
            }
            else if (i == 2 && itemPercent <= CPercent)
            {
                itemToDrop = RangedItems[2];
            }
            if (itemToDrop != null)
            {
                Vector3 dropPosition = transform.position + new Vector3(0f, 1f, 1f);
                Instantiate(itemToDrop, dropPosition, Quaternion.identity);
            }
        }
    }
}
