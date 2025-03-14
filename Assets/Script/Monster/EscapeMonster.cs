using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class EscapeMonster : MonsterBase
{
    //도망 몬스터는 공격 범위 없고
    //인지 범위에 들어왔을 때 반대쪽 경로 설정
    [Header("도망 몬스터 information")]
    public bool isEscaping = false;
    private Vector3 escapeTarget; //도망가는 위치
    [SerializeField]
    private float moveSpeed = 3f; //이동 속도
    [SerializeField]
    private float escapeSpeed = 5f; //도망속도
    [SerializeField]
    private float escapeDistance = 10f; //도망가는 거리
    [SerializeField]
    private float viewAngle = 0f; //감지 범위 각도

    [Header("Layer")]
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    LayerMask obstacleMask;
    private bool isPlayerDetected = false; //플레이어 감지 상태

    private bool isDie; //죽음 체크 변수


    //도망 몬스터는 쫒는 것이 아닌 도망가는 것으로 수정
    protected override void HandleChasing()
    {
        //범위 내에 들어왔을 때 도망 실행
        if (!isEscaping)
        {
            CheckFieldOfView();
            if (!isPlayerDetected)
            {
                base.HandleRandomMove();
            }

        }
        else
        {
            if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance)
            {
                isEscaping = false; //도망 종료
                isPlayerDetected = false;
                nav.speed = moveSpeed; //움직임 속도 초기화
                nav.ResetPath(); //경로 초기화
            }
        }
    }

    private void CheckFieldOfView()
    {
        Vector3 myPos = transform.position + Vector3.up * 0.5f; //몬스터의 현재 위치
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y); //몬스터의 현재 방향

        //시야 범위 내 타겟 감지
        Collider[] targets = Physics.OverlapSphere(myPos, playerDetectionRange, targetMask);
        if (targets.Length == 0) return;

        foreach (Collider target in targets)
        {
            Vector3 targetPos = target.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

            //타겟이 시야 내에 있고 장애물이 없으면 도망 시작
            if (targetAngle <= viewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, playerDetectionRange, obstacleMask))
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

        nav.speed = escapeSpeed; //도망속도로 변경

        //도망가는 방향 설정
        Vector3 myPos = transform.position;
        Vector3 direction = (myPos - targetPosition).normalized;
        direction.y = 0f; // y 값 고정

        escapeTarget = myPos + direction * escapeDistance;

        //NavMesh 상에서 도망갈 위치가 유효한지 확인
        if (NavMesh.SamplePosition(escapeTarget, out NavMeshHit hit, escapeDistance, NavMesh.AllAreas))
        {
            escapeTarget = hit.position;
            nav.SetDestination(escapeTarget); //도망 실행
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
                    nav.SetDestination(escapeTarget); //도망 실행
                    break;
                }
            }
        }

        //도망가는 방향으로 회전
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red; //감지 범위
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, playerDetectionRange);

        //시야 각도 범위 표시
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);

        Debug.DrawRay(myPos, rightDir * playerDetectionRange, Color.blue);
        Debug.DrawRay(myPos, leftDir * playerDetectionRange, Color.blue);
        Debug.DrawRay(myPos, lookDir * playerDetectionRange, Color.cyan);

        Gizmos.color = Color.green; //움직임 범위
        Gizmos.DrawWireSphere(initialPosition, randomMoveRange);

        Gizmos.color = Color.magenta; //플레이어 공격 인지 범위 
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
