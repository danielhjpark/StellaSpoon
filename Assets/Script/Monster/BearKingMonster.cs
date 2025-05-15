using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

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

    private int JumpDamage = 50; //������� ������
    private int chargeDamage = 60; //���� ������

    public Transform playerTf;

    public Collider leftHandCollider;
    public Collider rightHandCollider;
    public GameObject jumpGroundEffectPrefab; //�ٴ� ����Ʈ ������
    public GameObject chargeGoundEffectPrefab; //���� ����Ʈ ������

    private bool isCharging = false;
    public static bool isChargeSetting = false; //���� �غ� �Ϸ� ����
    public static bool isJumping = false;


    [SerializeField]
    private GameObject currentGroundEffect;

    private void Start()
    {
        base.Start();
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
        attackRange = 3f;
    }

    protected override void HandleAttack()
    {
        animator.SetBool("Walk", false);
        if (!isAttack)
        {
            isAttack = true;
            StartCoroutine(Attack());
        }
    }


    private IEnumerator Attack()
    {
        if(!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        //Debug.Log("�⺻ ���� ����!");
        animator.SetTrigger("Attack8");
        attackRange = 15f;
        yield return new WaitForSeconds(5.0f);
        //Debug.Log("�⺻ ���� ����!");
        nextPattern = JUMP;
        nextPatternPlay();
    }

    private IEnumerator Jump()
    {
        if (!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        isJumping = true;
        //Debug.Log("������� ����!");
        yield return StartCoroutine(ShowJumpGroundEffect()); //�ٴ� ��� ȿ��

        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
        }

        //Debug.Log("����� �߻�!");
        animator.SetTrigger("Attack5");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shockwaveRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                //Debug.Log("�÷��̾ ����ĸ� �¾ҽ��ϴ�!");
                thirdPersonController.TakeDamage(JumpDamage, transform.position); //�÷��̾� ������
            }
        }

        nav.gameObject.GetComponent<NavMeshAgent>().enabled = true; // NavMeshAgent Ȱ��ȭ

        attackRange = 15f;
        yield return new WaitForSeconds(6.0f);
        isJumping = false;
        nextPattern = CHARGE;
        nextPatternPlay();
    }

    private IEnumerator Charge()
    {
        if (!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        //Debug.Log("���� �غ� ����!");
        isChargeSetting = true;
        animator.SetBool("Run Forward", true);
        // animator.SetTrigger("Charge");

        // ���� �غ� �� �÷��̾��� ���� ��ġ ����
        Vector3 targetPosition = player.transform.position;

        // ���� �غ� �� chargeGroundEffectPrefab ���� �� ũ�� ����
        yield return StartCoroutine(ShowChargeGroundEffect(targetPosition));

        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
        }
        //Debug.Log("���� ����!");
        isCharging = true;

        // ���� ���� �� NavMeshAgent ��Ȱ��ȭ (���� �̵��� ����)
        nav.isStopped = true;

        float startTime = Time.time;

        while (Time.time < startTime + chargeDuration)
        {
            // ��ǥ ��ġ������ �Ÿ� ���
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // ��ǥ�� �����ϸ� ���� ����
            if (distanceToTarget <= 2f)
            {
                break;
            }

            // ��ǥ �������� �̵�
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;

            // �浹 ����
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3.0f); // �浹 �ݰ� ����
            foreach (var hit in hitColliders)
            {
                if (hit.CompareTag("Player"))
                {
                    //Debug.Log("�÷��̾ ������ �¾ҽ��ϴ�!");
                    thirdPersonController.TakeDamage(chargeDamage, transform.position); // �÷��̾�� ������
                    isCharging = false;
                    isChargeSetting = false;
                    break; // �浹 �� ���� ����
                }
            }

            if (!isCharging) break; // �浹 �߻� �� ���� ����

            yield return null;
        }

        animator.SetBool("Run Forward", false);
        animator.SetTrigger("Attack3");
        
        yield return new WaitForSeconds(1f); // �ִϸ��̼� ��� �ð�
        isCharging = false;
        isChargeSetting = false;

        //Debug.Log("���� ����!");
        attackRange = 3f;
        yield return new WaitForSeconds(5.0f);

        // �÷��̾ �ٽ� ���� ������ �ִ��� Ȯ�� �� ����
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            nextPattern = ATTACK;
            nextPatternPlay();
        }
        else
        {
            // �÷��̾ �ʹ� �ָ� ���� �����ϰ� ���
            //Debug.Log("�÷��̾ �ʹ� �־ ������ �����մϴ�.");
            isAttack = false;
        }
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
        nav.gameObject.GetComponent<NavMeshAgent>().enabled = false; // NavMeshAgent ��Ȱ��ȭ
        if (jumpGroundEffectPrefab != null)
        {
            currentGroundEffect = Instantiate(jumpGroundEffectPrefab, new Vector3(transform.position.x, 0.01f, transform.position.z), Quaternion.identity);
            currentGroundEffect.transform.localScale = Vector3.zero;

            float duration = 2.0f;
            float elapsedTime = 0f;

            //Debug.Log("�ٴ� ��� ȿ���� ���� Ŀ���ϴ�.");
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

            currentGroundEffect.transform.localScale = new Vector3(direction.z / 2, 0.01f, 1.3f);
        }

        yield return new WaitForSeconds(2.0f); // 2�ʰ� ����
    }

    private void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shockwaveRadius);
    }

    public void ONLeftHand()
    {
        leftHandCollider.enabled = true;
    }
    public void OFFLeftHand()
    {
        leftHandCollider.enabled = false;
    }
    public void ONRightHand()
    {
        rightHandCollider.enabled = true;
    }
    public void OFFRightHand()
    {
        rightHandCollider.enabled = false;
    }
}
