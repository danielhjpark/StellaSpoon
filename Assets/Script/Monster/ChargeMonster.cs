using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargeMonster : MonsterBase
{
    [Header("Basic Information")]
    [SerializeField]
    private float attackDamage = 10f;//공격 데미지
    [SerializeField]
    private float attackRange = 5f;//공격 범위
    [SerializeField]
    private float chargeSpeed = 10f; //돌격속도
    [SerializeField]
    private float chargeDuration = 2f;


    private bool isCharging = false;

    private NavMeshAgent agent;      //몬스터의 NavMeshAgent
    private Animator animator;
    private Collider collider;

    private void Start()
    {
        base.Start(); //부모 클래스 초기화
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    private void Update()
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
        if (!isCharging)
        {
            Debug.Log("돌격 시작!");
            StartCoroutine(Charge());
        }
        //여기에 플레이어에게 데미지를 입히는 로직 추가

        /*animator.SetBool("Attack", false);*/
    }

    private IEnumerator Charge()
    {
        isCharging = true;
        Vector3 targetPosition = playerTf.position - (playerTf.position - transform.position).normalized * 0.4f; //목표보다 0.4 앞에 멈추게

        // 돌격 시작 시 NavMeshAgent 비활성화 (직접 이동을 위해)
        agent.isStopped = true;

        float startTime = Time.time;

        while (Time.time < startTime + chargeDuration)
        {
            // 목표 위치까지의 거리 계산
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // 목표에 도달하면 돌격 종료
            if (distanceToTarget <= 1f)
            {
                break;
            }

            // 목표 방향으로 이동
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;

            yield return null;
        }

        Debug.Log("돌격 종료!");
        isCharging = false;

        // NavMeshAgent 다시 활성화
        agent.isStopped = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && isCharging)
        {
            Debug.Log("플레이어와 충돌! 돌격 종료.");
            StopCharging();
        }
    }

    private void StopCharging()
    {
        isCharging = false;
        agent.isStopped = false; //NavMeshAgent 재활성화
    }

    // 감지 및 공격 범위 시각화
    private void OnDrawGizmos() //항상 보이게 //선택시 보이게 OnDrawGizmosSelected
    {
        Gizmos.color = Color.red; //감지 범위
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue; // 공격 범위
        Gizmos.DrawWireSphere(transform.position, attackRange);
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
        agent.isStopped = true; //이동 멈추기
        collider.enabled = false; //충돌 제거
        animator.SetTrigger("Die");
        StartCoroutine(DieDelays());
    }

    IEnumerator DieDelays()
    {
        yield return new WaitForSeconds(dieDelay);

        Destroy(gameObject);
    }
}
