using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.AI;

public class EscapeMonster : MonsterBase
{
    //?„ë§? ëª¬ìŠ¤?„°?Š” ê³µê²© ë²”ìœ„ ?—†ê³?
    //?¸ì§? ë²”ìœ„?— ?“¤?–´?™”?„ ?•Œ ë°˜ë??ìª? ê²½ë¡œ ?„¤? •
    [Header("?„ë§? ëª¬ìŠ¤?„° information")]
    public bool isEscaping = false; //?„ë§ê???Š”ì¤? ë³??ˆ˜ 
    private Vector3 escapeTarget; //?„ë§ê???Š” ?œ„ì¹?
    [SerializeField]
    private float moveSpeed = 3f; //?´?™ ?†?„
    [SerializeField]
    private float escapeSpeed = 5f; //?„ë§ì†?„
    [SerializeField]
    private float escapeDistance = 10f; //?„ë§ê???Š” ê±°ë¦¬
    [SerializeField]
    private float viewAngle = 0f; //ê°ì?? ë²”ìœ„ ê°ë„

    [Header("Layer")]
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    LayerMask obstacleMask;
    private bool isPlayerDetected = false; //?”Œ? ˆ?´?–´ ê°ì?? ?ƒ?ƒœ

    private bool isDie; //ì£½ìŒ ì²´í¬ ë³??ˆ˜


    //?„ë§? ëª¬ìŠ¤?„°?Š” ì«’ëŠ” ê²ƒì´ ?•„?‹Œ ?„ë§ê???Š” ê²ƒìœ¼ë¡? ?ˆ˜? •
    protected override void HandleChasing()
    {
        //ë²”ìœ„ ?‚´?— ?“¤?–´?™”?„ ?•Œ ?„ë§? ?‹¤?–‰
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
                isEscaping = false; //?„ë§? ì¢…ë£Œ
                isPlayerDetected = false;
                nav.speed = moveSpeed; //???ì§ì„ ?†?„ ì´ˆê¸°?™”
                nav.ResetPath(); //ê²½ë¡œ ì´ˆê¸°?™”
            }
        }
    }

    private void CheckFieldOfView()
    {
        Vector3 myPos = transform.position + Vector3.up * 0.5f; //ëª¬ìŠ¤?„°?˜ ?˜„?¬ ?œ„ì¹?
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y); //ëª¬ìŠ¤?„°?˜ ?˜„?¬ ë°©í–¥

        //?‹œ?•¼ ë²”ìœ„ ?‚´ ???ê²? ê°ì??
        Collider[] targets = Physics.OverlapSphere(myPos, playerDetectionRange, targetMask);
        if (targets.Length == 0) return;

        foreach (Collider target in targets)
        {
            Vector3 targetPos = target.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

            //???ê²Ÿì´ ?‹œ?•¼ ?‚´?— ?ˆê³? ?¥?• ë¬¼ì´ ?—†?œ¼ë©? ?„ë§? ?‹œ?‘
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

        nav.speed = escapeSpeed; //?„ë§ì†?„ë¡? ë³?ê²?

        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);

        //?„ë§ê???Š” ë°©í–¥ ?„¤? •
        Vector3 myPos = transform.position;
        Vector3 direction = (myPos - targetPosition).normalized;
        direction.y = 0f; // y ê°? ê³ ì •

        escapeTarget = myPos + direction * escapeDistance;

        //NavMesh ?ƒ?—?„œ ?„ë§ê°ˆ ?œ„ì¹˜ê?? ?œ ?š¨?•œì§? ?™•?¸
        if (NavMesh.SamplePosition(escapeTarget, out NavMeshHit hit, escapeDistance, NavMesh.AllAreas))
        {
            escapeTarget = hit.position;
            nav.SetDestination(escapeTarget); //?„ë§? ?‹¤?–‰
        }
        else
        {
            //NavMesh?— ?œ ?š¨?•˜ì§? ?•Š??? ê²½ìš°, ?„ë§? ê±°ë¦¬ë¥? ì¤„ì—¬?„œ ?œ ?š¨?•œ ?œ„ì¹˜ë?? ì°¾ìŒ
            float reducedDistance = escapeDistance;
            while (reducedDistance > 1f) //ìµœì†Œ ê±°ë¦¬ê¹Œì?? ê°ì†Œ
            {
                reducedDistance -= 1f;
                Vector3 closerEscapeTarget = myPos + direction * reducedDistance;

                if (NavMesh.SamplePosition(closerEscapeTarget, out hit, reducedDistance, NavMesh.AllAreas))
                {
                    escapeTarget = hit.position; //?œ ?š¨?•œ ?œ„ì¹˜ë¡œ ?„¤? •
                    nav.SetDestination(escapeTarget); //?„ë§? ?‹¤?–‰
                    break;
                }
            }
        }

        //?„ë§ê???Š” ë°©í–¥?œ¼ë¡? ?šŒ? „
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
        Gizmos.color = Color.red; //ê°ì?? ë²”ìœ„
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, playerDetectionRange);

        //?‹œ?•¼ ê°ë„ ë²”ìœ„ ?‘œ?‹œ
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);

        Debug.DrawRay(myPos, rightDir * playerDetectionRange, Color.blue);
        Debug.DrawRay(myPos, leftDir * playerDetectionRange, Color.blue);
        Debug.DrawRay(myPos, lookDir * playerDetectionRange, Color.cyan);

        Gizmos.color = Color.green; //???ì§ì„ ë²”ìœ„
        Gizmos.DrawWireSphere(initialPosition, randomMoveRange);

        Gizmos.color = Color.magenta; //?”Œ? ˆ?´?–´ ê³µê²© ?¸ì§? ë²”ìœ„ 
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
