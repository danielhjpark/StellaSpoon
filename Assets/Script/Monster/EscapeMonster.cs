using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.AI;

public class EscapeMonster : MonsterBase
{
    //���� ���ʹ� ���� ���� ����
    //���� ������ ������ �� �ݴ��� ��� ����
    [Header("?���? 몬스?�� information")]
    public bool isEscaping = false; //�������� �� ����
    private Vector3 escapeTarget; //�������� ��ġ
    [SerializeField]
    private float moveSpeed = 3f; //�̵� �ӵ�
    [SerializeField]
    private float escapeSpeed = 5f; //���� �ӵ�
    [SerializeField]
    private float escapeDistance = 10f; //�������� �Ÿ�
    [SerializeField]
    private float viewAngle = 0f; //���� ���� ����

    [Header("Layer")]
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    LayerMask obstacleMask;
    private bool isPlayerDetected = false; //�÷��̾� ���� ����

    private bool isDie; //���� üũ ����


    //���� ���ʹ� �i�� ���� �ƴ� �������� ������ ����
    protected override void HandleChasing()
    {
        //���� ���� ������ �� ���� ����
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
                isEscaping = false; //���� ����
                isPlayerDetected = false;
                nav.speed = moveSpeed; //������ �ӵ� �ʱ�ȭ
                nav.ResetPath(); //��� �ʱ�ȭ
            }
        }
    }

    private void CheckFieldOfView()
    {
        Vector3 myPos = transform.position + Vector3.up * 0.5f; //������ ���� ��ġ
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y); //������ ���� ����

        //�þ� ���� �� Ÿ�� ����
        Collider[] targets = Physics.OverlapSphere(myPos, playerDetectionRange, targetMask);
        if (targets.Length == 0) return;

        foreach (Collider target in targets)
        {
            Vector3 targetPos = target.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

            //Ÿ���� �þ� ���� �ְ� ��ֹ��� ������ ���� ����
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

        nav.speed = escapeSpeed; //���� �ӵ��� ����

        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);

        //�������� ���� ����
        Vector3 myPos = transform.position;
        Vector3 direction = (myPos - targetPosition).normalized;
        direction.y = 0f; // y�� ����

        escapeTarget = myPos + direction * escapeDistance;

        //NavMesh �󿡼� ������ ��ġ�� ��ȿ���� Ȯ��
        if (NavMesh.SamplePosition(escapeTarget, out NavMeshHit hit, escapeDistance, NavMesh.AllAreas))
        {
            escapeTarget = hit.position;
            nav.SetDestination(escapeTarget); //���� ����
        }
        else
        {
            //NavMesh�� ��ȿ���� ���� ���, �����Ÿ��� �ٿ��� ��ȿ�� ��ġ�� ã��
            float reducedDistance = escapeDistance;
            while (reducedDistance > 1f) //�ּ� �Ÿ����� ����
            {
                reducedDistance -= 1f;
                Vector3 closerEscapeTarget = myPos + direction * reducedDistance;

                if (NavMesh.SamplePosition(closerEscapeTarget, out hit, reducedDistance, NavMesh.AllAreas))
                {
                    escapeTarget = hit.position; //��ȿ�� ��ġ�� ����
                    nav.SetDestination(escapeTarget); //���� ����
                    break;
                }
            }
        }

        //�������� �������� ȸ��
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
        Gizmos.color = Color.red; //���� ����
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, playerDetectionRange);

        //�þ� ���� ���� ǥ��
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);

        Debug.DrawRay(myPos, rightDir * playerDetectionRange, Color.blue);
        Debug.DrawRay(myPos, leftDir * playerDetectionRange, Color.blue);
        Debug.DrawRay(myPos, lookDir * playerDetectionRange, Color.cyan);

        Gizmos.color = Color.green; //������ ����
        Gizmos.DrawWireSphere(initialPosition, randomMoveRange);

        Gizmos.color = Color.magenta; //�÷��̾� ���� ���� ����
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
