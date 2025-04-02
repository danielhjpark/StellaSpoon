using System.Collections;
using TMPro;
using UnityEngine;

public class BearKingMonster : MonsterBase
{
    private int nextPattern = 0;

    private static readonly int ATTACK = 0;
    private static readonly int JUMP = 1;
    private static readonly int CHARGE = 2;

    public float attackRadius = 1.5f;
    public float shockwaveRadius = 5.0f; // ����� ����
    public float chargeSpeed = 10.0f;
    private float chargeDuration = 2f;

    public Transform playerTf;

    public Collider leftHandCollider;
    public Collider rightHandCollider;
    public GameObject jumpGroundEffectPrefab; // �ٴ� ����Ʈ ������
    public GameObject chargeGoundEffectPrefab; // ���� ����Ʈ ������

    private bool isCharging = false;
    [SerializeField]
    private GameObject currentGroundEffect;

    private void Start()
    {
        base.Start();
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
    }

    protected override void HandleAttack()
    {
        //animator.SetBool("Walk", false);
        if (!isAttack)
        {
            isAttack = true;
            /*animator.SetBool("Walk", false);
            animator.SetBool("Attack", true); //Attack �ִϸ��̼� ����*/
            //�÷��̾�� ������ �ֱ�
            StartCoroutine(Attack());
        }
    }


    private IEnumerator Attack()
    {
        if(!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        Debug.Log("�⺻ ���� ����!");
        // animator.SetTrigger("Attack");
        yield return new WaitForSeconds(5.0f);
        Debug.Log("�⺻ ���� ����!");
        nextPattern = JUMP;
        nextPatternPlay();
    }

    private IEnumerator Jump()
    {
        if (!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        Debug.Log("������� ����!");
        // animator.SetTrigger("Jump");
        yield return StartCoroutine(ShowJumpGroundEffect()); //�ٴ� ��� ȿ��

        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
        }

        Debug.Log("����� �߻�!");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shockwaveRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("�÷��̾ ����ĸ� �¾ҽ��ϴ�!");
            }
        }

        yield return new WaitForSeconds(6.0f);
        nextPattern = CHARGE;
        nextPatternPlay();
    }

    private IEnumerator Charge()
    {
        if (!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        Debug.Log("���� �غ� ����!");
        // animator.SetTrigger("Charge");

        // ���� �غ� �� �÷��̾��� ���� ��ġ ����
        Vector3 targetPosition = player.transform.position;

        // ���� �غ� �� chargeGroundEffectPrefab ���� �� ũ�� ����
        yield return StartCoroutine(ShowChargeGroundEffect(targetPosition));

        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
        }
        Debug.Log("���� ����!");
        isCharging = true;

        // ���� ���� �� NavMeshAgent ��Ȱ��ȭ (���� �̵��� ����)
        nav.isStopped = true;

        float startTime = Time.time;

        while (Time.time < startTime + chargeDuration)
        {
            // ��ǥ ��ġ������ �Ÿ� ���
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // ��ǥ�� �����ϸ� ���� ����
            if (distanceToTarget <= 4f)
            {
                break;
            }

            // ��ǥ �������� �̵�
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;

            yield return null;
        }

        isCharging = false;
        Debug.Log("���� ����!");

        yield return new WaitForSeconds(5.0f);
        nextPattern = ATTACK;
        nextPatternPlay();
    }

    private void nextPatternPlay()
    {
        switch (nextPattern)
        {
            case 0:
                StartCoroutine(Attack());
                break;
            case 1:
                StartCoroutine(Jump());
                break;
            case 2:
                StartCoroutine(Charge());
                break;
        }
    }

    private IEnumerator ShowJumpGroundEffect()
    {
        if (jumpGroundEffectPrefab != null)
        {
            currentGroundEffect = Instantiate(jumpGroundEffectPrefab, new Vector3(transform.position.x, 0.01f, transform.position.z), Quaternion.identity);
            currentGroundEffect.transform.localScale = Vector3.zero;

            float duration = 2.0f;
            float elapsedTime = 0f;

            Debug.Log("�ٴ� ��� ȿ���� ���� Ŀ���ϴ�.");
            while (elapsedTime < duration)
            {
                float progress = elapsedTime / duration;
                float scale = Mathf.Lerp(0, shockwaveRadius * 2, progress * progress);
                currentGroundEffect.transform.localScale = new Vector3(scale, 0.01f, scale);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            currentGroundEffect.transform.localScale = new Vector3(shockwaveRadius * 2, 0.01f, shockwaveRadius * 2);
        }
        yield return new WaitForSeconds(2.0f); // 2�ʰ� ����
    }

    private IEnumerator ShowChargeGroundEffect(Vector3 targetPosition)
    {
        if (chargeGoundEffectPrefab != null)
        {
            Vector3 middlePosition =  transform.position + ((targetPosition - transform.position)/2);
            currentGroundEffect = Instantiate(chargeGoundEffectPrefab, new Vector3(middlePosition.x, 0.01f, middlePosition.z), Quaternion.identity);

            // ���� ������ y���� 0���� �����Ͽ� y�� ȸ���� ����
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;
            currentGroundEffect.transform.rotation = Quaternion.LookRotation(direction);

            currentGroundEffect.transform.localScale = new Vector3(direction.z / 2, 0.01f, 1);
        }

        yield return new WaitForSeconds(2.0f); // 2�ʰ� ����
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shockwaveRadius);
    }
}
