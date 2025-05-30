using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.AI;

public class EscapeMonster : MonsterBase
{
    //?���? 몬스?��?�� 공격 범위 ?���?
    //?���? 범위?�� ?��?��?��?�� ?�� 반�??�? 경로 ?��?��
    [Header("?���? 몬스?�� information")]
    public bool isEscaping = false; //?��망�???���? �??�� 
    private Vector3 escapeTarget; //?��망�???�� ?���?
    [SerializeField]
    private float moveSpeed = 3f; //?��?�� ?��?��
    [SerializeField]
    private float escapeSpeed = 5f; //?��망속?��
    [SerializeField]
    private float escapeDistance = 10f; //?��망�???�� 거리
    [SerializeField]
    private float viewAngle = 0f; //감�?? 범위 각도

    [Header("Layer")]
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    LayerMask obstacleMask;
    private bool isPlayerDetected = false; //?��?��?��?�� 감�?? ?��?��

    private bool isDie; //죽음 체크 �??��


    //?���? 몬스?��?�� 쫒는 것이 ?��?�� ?��망�???�� 것으�? ?��?��
    protected override void HandleChasing()
    {
        //범위 ?��?�� ?��?��?��?�� ?�� ?���? ?��?��
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
                animator.SetBool("Run", false);
                isEscaping = false; //?���? 종료
                isPlayerDetected = false;
                nav.speed = moveSpeed; //???직임 ?��?�� 초기?��
                nav.ResetPath(); //경로 초기?��
            }
        }
    }

    private void CheckFieldOfView()
    {
        Vector3 myPos = transform.position + Vector3.up * 0.5f; //몬스?��?�� ?��?�� ?���?
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y); //몬스?��?�� ?��?�� 방향

        //?��?�� 범위 ?�� ???�? 감�??
        Collider[] targets = Physics.OverlapSphere(myPos, playerDetectionRange, targetMask);
        if (targets.Length == 0) return;

        foreach (Collider target in targets)
        {
            Vector3 targetPos = target.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

            //???겟이 ?��?�� ?��?�� ?���? ?��?��물이 ?��?���? ?���? ?��?��
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

        nav.speed = escapeSpeed; //?��망속?���? �?�?

        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);

        //?��망�???�� 방향 ?��?��
        Vector3 myPos = transform.position;
        Vector3 direction = (myPos - targetPosition).normalized;
        direction.y = 0f; // y �? 고정

        escapeTarget = myPos + direction * escapeDistance;

        //NavMesh ?��?��?�� ?��망갈 ?��치�?? ?��?��?���? ?��?��
        if (NavMesh.SamplePosition(escapeTarget, out NavMeshHit hit, escapeDistance, NavMesh.AllAreas))
        {
            escapeTarget = hit.position;
            nav.SetDestination(escapeTarget); //?���? ?��?��
        }
        else
        {
            //NavMesh?�� ?��?��?���? ?��??? 경우, ?���? 거리�? 줄여?�� ?��?��?�� ?��치�?? 찾음
            float reducedDistance = escapeDistance;
            while (reducedDistance > 1f) //최소 거리까�?? 감소
            {
                reducedDistance -= 1f;
                Vector3 closerEscapeTarget = myPos + direction * reducedDistance;

                if (NavMesh.SamplePosition(closerEscapeTarget, out hit, reducedDistance, NavMesh.AllAreas))
                {
                    escapeTarget = hit.position; //?��?��?�� ?��치로 ?��?��
                    nav.SetDestination(escapeTarget); //?���? ?��?��
                    break;
                }
            }
        }

        //?��망�???�� 방향?���? ?��?��
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; //감�?? 범위
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, playerDetectionRange);

        //?��?�� 각도 범위 ?��?��
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);

        Debug.DrawRay(myPos, rightDir * playerDetectionRange, Color.blue);
        Debug.DrawRay(myPos, leftDir * playerDetectionRange, Color.blue);
        Debug.DrawRay(myPos, lookDir * playerDetectionRange, Color.cyan);

        Gizmos.color = Color.green; //???직임 범위
        Gizmos.DrawWireSphere(initialPosition, randomMoveRange);

        Gizmos.color = Color.magenta; //?��?��?��?�� 공격 ?���? 범위 
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
