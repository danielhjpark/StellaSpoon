using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.AI;

public class EscapeMonster : MonsterBase
{
    [Header("Basic Information")]
    [SerializeField]
    protected float attackDamage = 10f;  //공격 데미지
    [Range(0f, 360f)]
    [SerializeField]
    private float viewAngle = 0f; //감지 범위 각도
    [SerializeField]
    private float moveSpeed = 3f; //이동 속도
    [SerializeField]
    private float escapeSpeed = 5f; //도망속도
    [SerializeField]
    private float escapeDistance = 10f; //도망가는 거리

    [SerializeField]
    private float wanderRadius = 15f; //랜덤으로 움직이는 범위
    [SerializeField]
    private float wanderTime = 3f; //랜덤 이동 주기

    private float wanderTimer;

    private bool isPlayerDetected = false; //플레이어 감지 상태

    private Vector3 initialPosition; //초기 위치 저장

    private Collider escapeMonsterCollider;

    [Header("Layer")]   
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    LayerMask obstacleMask;

    NavMeshAgent agent;
    public bool isEscaping = false;
    private Vector3 escapeTarget; //도망가는 위치

    [Header("Item")]
    public GameObject[] EscapeItems;
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

        escapeMonsterCollider = GetComponent<Collider>();

        agent.avoidancePriority = Random.Range(30, 60); // 회피 우선순위를 랜덤으로 설정

        agent.speed = moveSpeed; //이동속도 설정
    }
    private void Update()
    {
        if(!isEscaping)
        {
            CheckFieldOfView();
            if(!isPlayerDetected)
            {
                HandleRandomMovement();
            }
            
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isEscaping = false; //도망 종료
                isPlayerDetected = false;
                agent.speed = moveSpeed; //움직임 속도 초기화
                agent.ResetPath(); //경로 초기화
            }
        }
    }

    private void HandleRandomMovement()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderTime)
        {
            Vector3 newDestination = GetRandomPoint(initialPosition, wanderRadius); // 초기 위치 기준
            agent.SetDestination(newDestination);
            wanderTimer = 0f;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath(); // 이동 종료 후 경로 초기화
        }
    }

    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        randomDirection.y = center.y; // 높이 고정

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return center;
    }

    private void CheckFieldOfView()
    {
        Vector3 myPos = transform.position + Vector3.up * 0.5f; //몬스터의 현재 위치
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y); //몬스터의 현재 방향

        //시야 범위 내 타겟 감지
        Collider[] targets = Physics.OverlapSphere(myPos, detectionRange, targetMask);
        if (targets.Length == 0) return;

        foreach (Collider target in targets)
        {
            Vector3 targetPos = target.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

            //타겟이 시야 내에 있고 장애물이 없으면 도망 시작
            if (targetAngle <= viewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, detectionRange, obstacleMask))
            {
                Debug.DrawLine(myPos, targetPos, Color.red);

                StartEscape(targetPos);
                break;
            }
        }
    }

    private void StartEscape(Vector3 targetPosition)
    {
        isEscaping = true;
        isPlayerDetected = true;

        agent.speed = escapeSpeed; //도망속도로 변경

        //도망가는 방향 설정
        Vector3 myPos = transform.position;
        Vector3 direction = (myPos - targetPosition).normalized;
        direction.y = 0f; // y 값 고정

        escapeTarget = myPos + direction * escapeDistance;

        //NavMesh 상에서 도망갈 위치가 유효한지 확인
        if (NavMesh.SamplePosition(escapeTarget, out NavMeshHit hit, escapeDistance, NavMesh.AllAreas))
        {
            escapeTarget = hit.position;
            agent.SetDestination(escapeTarget); //도망 실행
        }
        else
        {
            //NavMesh에 유효하지 않은 경우, 도망 거리를 줄여서 유효한 위치를 찾음
            float reducedDistance = escapeDistance;
            while (reducedDistance > 1f) //최소 거리까지 감소
            {
                reducedDistance -= 1f;
                Vector3 closerEscapeTarget = myPos + direction * reducedDistance;

                if (NavMesh.SamplePosition(closerEscapeTarget, out hit, reducedDistance, NavMesh.AllAreas))
                {
                    escapeTarget = hit.position; //유효한 위치로 설정
                    agent.SetDestination(escapeTarget); //도망 실행
                    break;
                }
            }
        }

        //도망가는 방향으로 회전
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, detectionRange);

        //시야 각도 범위 표시
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);

        Debug.DrawRay(myPos, rightDir * detectionRange, Color.blue);
        Debug.DrawRay(myPos, leftDir * detectionRange, Color.blue);
        Debug.DrawRay(myPos, lookDir * detectionRange, Color.cyan);

        Gizmos.color = Color.green; //움직임 범위
        Gizmos.DrawWireSphere(initialPosition, wanderRadius);
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collDamage)
            {
                Debug.Log("데미지");
                Vector3 attackerPosition = transform.position; // 플레이어를 공격하는 방향
                                                               //여기에 플레이어에게 데미지를 입히는 로직 추가
                thirdPersonController.TakeDamage(attackDamage, attackerPosition);
                StartCoroutine(AttackDelay());
            }
        }
    }

    protected override void Attack() //도망 몬스터는 공격 X
    {

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
                itemToDrop = EscapeItems[0];
            }
            else if (i == 1 && itemPercent <= BPercent)
            {
                itemToDrop = EscapeItems[1];
            }
            else if (i == 2 && itemPercent <= CPercent)
            {
                itemToDrop = EscapeItems[2];
            }
            if (itemToDrop != null)
            {
                Vector3 dropPosition = transform.position + new Vector3(0f, 1f, 1f);
                Instantiate(itemToDrop, dropPosition, Quaternion.identity);
            }
        }
    }
}
