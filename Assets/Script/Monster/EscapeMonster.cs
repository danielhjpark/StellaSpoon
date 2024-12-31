using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.AI;

public class EscapeMonster : MonsterBase
{
    [Header("Basic Information")]
    [Range(0f, 360f)]
    [SerializeField]
    private float viewAngle = 0f; //감지 범위 각도
    [SerializeField]
    private float moveSpeed = 3f; //이동 속도
    [SerializeField]
    private float escapeDistance = 10f; //도망가는 거리

    [SerializeField]
    private float wanderRadius = 15f; //랜덤으로 움직이는 범위
    [SerializeField]
    private float wanderTime = 3f; //랜덤 이동 주기

    private float wanderTimer;

    private bool isPlayerDetected = false; //플레이어 감지 상태

    private Vector3 initialPosition; //초기 위치 저장

    [Header("Layer")]   
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    LayerMask obstacleMask;

    NavMeshAgent agent;
    private bool isEscaping = false;
    private Vector3 escapeTarget; //도망가는 위치

    

    private void Start()
    {
        base.Start(); //부모 클래스 초기화
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderTime;

        initialPosition = transform.position; //초기 위치 저장

        agent.avoidancePriority = Random.Range(30, 60); // 회피 우선순위를 랜덤으로 설정
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
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
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
        agent.isStopped = true; //이동 멈춤
        GetComponent<Collider>().enabled = false; //충돌 제거
        animator.SetTrigger("Die");
        StartCoroutine(DieDelays());
    }
    IEnumerator DieDelays()
    {
        yield return new WaitForSeconds(dieDelay);

        Destroy(gameObject);
    }
}
