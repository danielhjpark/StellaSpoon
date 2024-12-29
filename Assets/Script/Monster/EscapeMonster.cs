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
    private float viewAngle = 0f; //�þ߰�
    [SerializeField]
    private float moveSpeed = 3f; //�̵��ӵ�
    [SerializeField]
    private float escapeDistance = 10f; //�������� �Ÿ�

    [Header("Layer")]   
    [SerializeField]
    LayerMask targetMask; //Ÿ�� ���̾�
    [SerializeField]
    LayerMask obstacleMask; //��ֹ� ���̾�

    NavMeshAgent agent;
    private bool isEscaping = false;
    private Vector3 escapeTarget; //�������� ��ġ

    

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
                isEscaping = false; //���� ���� ����
                agent.ResetPath(); //��� �ʱ�ȭ
            }
        }
        else
        {
            CheckFieldOfView();
        }
    }

    void CheckFieldOfView()
    {
        Vector3 myPos = transform.position + Vector3.up * 0.5f; //������ ��ġ
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y); //���� �ٶ󺸴� ����

        // ���� ���� �� Ÿ�� Ȯ��
        Collider[] targets = Physics.OverlapSphere(myPos, detectionRange, targetMask);
        if (targets.Length == 0) return;

        foreach (Collider target in targets)
        {
            Vector3 targetPos = target.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

            // Ÿ���� �þ߰� �ȿ� �ְ� ��ֹ��� ���� ���
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

        //Ÿ�� �ݴ� �������� ������ ��ġ ���
        Vector3 myPos = transform.position;
        Vector3 direction = (myPos - targetPosition).normalized;
        direction.y = 0f; // y �� ����

        escapeTarget = myPos + direction * escapeDistance;

        //NavMesh ���� ��ȿ�� ��ġ���� Ȯ��
        if (NavMesh.SamplePosition(escapeTarget, out NavMeshHit hit, escapeDistance, NavMesh.AllAreas))
        {
            escapeTarget = hit.position;
            agent.SetDestination(escapeTarget); //���� ��ġ�� �̵�
        }

        //���Ͱ� ���� ������ �ٶ󺸵��� ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; //���� ����
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, detectionRange);

        //�þ߰� ���� �׸���
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

    protected override void Attack() //�������ʹ� ���� X
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
        agent.isStopped = true; //�̵� ���߱�
        GetComponent<Collider>().enabled = false; //�浹 ����
        animator.SetTrigger("Die");
        StartCoroutine(DieDelays());
    }
    IEnumerator DieDelays()
    {
        yield return new WaitForSeconds(dieDelay);

        Destroy(gameObject);
    }
}
