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
    private float viewAngle = 0f; //시야각
    [SerializeField]
    private float moveSpeed = 3f; //이동속도
    [SerializeField]
    private float escapeDistance = 10f; //도망가는 거리

    [Header("Layer")]   
    [SerializeField]
    LayerMask targetMask; //타겟 레이어
    [SerializeField]
    LayerMask obstacleMask; //장애물 레이어

    NavMeshAgent agent;
    private bool isEscaping = false;
    private Vector3 escapeTarget; //도망가는 위치

    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if(isEscaping)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isEscaping = false; //도망 상태 해제
                agent.ResetPath(); //경로 초기화
            }
        }
        else
        {
            CheckFieldOfView();
        }
    }

    void CheckFieldOfView()
    {
        Vector3 myPos = transform.position + Vector3.up * 0.5f; //몬스터의 위치
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y); //현재 바라보는 방향

        // 감지 범위 내 타겟 확인
        Collider[] targets = Physics.OverlapSphere(myPos, detectionRange, targetMask);
        if (targets.Length == 0) return;

        foreach (Collider target in targets)
        {
            Vector3 targetPos = target.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

            // 타겟이 시야각 안에 있고 장애물이 없는 경우
            if (targetAngle <= viewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, detectionRange, obstacleMask))
            {
                Debug.DrawLine(myPos, targetPos, Color.red);

                StartEscape(targetPos);
                break;
            }
        }
    }

    void StartEscape(Vector3 targetPosition)
    {
        isEscaping = true;

        //타겟 반대 방향으로 도망갈 위치 계산
        Vector3 myPos = transform.position;
        Vector3 direction = (myPos - targetPosition).normalized;
        direction.y = 0f; // y 값 고정

        escapeTarget = myPos + direction * escapeDistance;

        //NavMesh 위에 유효한 위치인지 확인
        if (NavMesh.SamplePosition(escapeTarget, out NavMeshHit hit, escapeDistance, NavMesh.AllAreas))
        {
            escapeTarget = hit.position;
            agent.SetDestination(escapeTarget); //도망 위치로 이동
        }

        //몬스터가 도망 방향을 바라보도록 회전
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; //감지 범위
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, detectionRange);

        //시야각 방향 그리기
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

    protected override void Attack() //도망몬스터는 구현 X
    {

    }
    protected override void Damage()
    {

    }

    protected override void Die()
    {

    }
}
