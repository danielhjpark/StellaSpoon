using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BearKingMonster : MonsterBase
{
    private Coroutine currentPatternCoroutine;

    private int nextPattern = 0;

    private static readonly int ATTACK = 0;
    private static readonly int JUMP = 1;
    private static readonly int CHARGE = 2;

    public float attackRadius = 1.5f;
    public float shockwaveRadius = 5.0f; // ����� ����
    public float chargeSpeed = 10.0f;
    private float chargeDuration = 2f;

    [SerializeField]
    private int JumpDamage = 40; //������� ������
    [SerializeField]
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
    [SerializeField]
    private GameObject jumpEffectprefab; //���� ����Ʈ ������
    private GameObject currentEffect;
    private bool jumpEffectOn; //���� ����Ʈ ���� ����

    [SerializeField]
    protected GameObject bossHealthUI;


    private bool isPlayerInRange = false;

    protected override void Start()
    {
        base.Start();
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
        attackRange = 3f;

        if (bossHealthUI != null)
        {
            bossHealthUI.SetActive(false);
        }
    }

    private void Update()
    {
        base.Update();

        if (distanceToPlayer <= playerDetectionRange && !isPlayerInRange)
        {
            isPlayerInRange = true;
            if (bossHealthUI != null)
            {
                bossHealthUI.SetActive(true);
            }
        }
        else if (distanceToPlayer > playerDetectionRange && isPlayerInRange)
        {
            isPlayerInRange = false;
            if (bossHealthUI != null)
            {
                bossHealthUI.SetActive(false);
            }
        }
        // ���� ���� ����
        if (inAttackRange && !isAttack)
        {
            HandleAttack(); // �÷��̾ ���� ������ �ٽ� ������ ����
        }
        else if(!inAttackRange && isAttack)
        {
            // ���� ����� ���� ���� ���� �� ���� �ʱ�ȭ
            if (currentPatternCoroutine != null)
            {
                StopCoroutine(currentPatternCoroutine); currentPatternCoroutine = null;
            }
            attackRadius = 3f;
        }
    }

    protected override void HandleAttack()
    {
        if (isAttack || currentPatternCoroutine != null) return; // �̹� ���� ���̸� ����
        animator.SetBool("Walk", false);
        isAttack = true;
        StartCoroutine(Attack());
    }


    private IEnumerator Attack()
    {
        if (!inAttackRange || isDead) yield break;

        animator.SetTrigger("Attack8");
        attackRange = 31f; //���� ���� ���� ����

        float elapsed = 0f;
        while (elapsed < 5.0f)
        {
            if (isDead)
            {
                CleanupEffects();
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        nextPattern = JUMP;
        currentPatternCoroutine = null;
        nextPatternPlay();
    }


    private IEnumerator Jump()
    {
        if (!inAttackRange || isDead) yield break;

        attackRange = 31f;
        isJumping = true;
        yield return StartCoroutine(ShowJumpGroundEffect());

        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
        }

        animator.SetTrigger("Attack5");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shockwaveRadius);
        foreach (var hit in hitColliders)
        {
            if (isDead) break;
            if (hit.CompareTag("Player"))
            {
                thirdPersonController.TakeDamage(JumpDamage, transform.position);
            }
        }

        yield return new WaitForSeconds(6.0f);

        // ���� ����
        isJumping = false;
        nav.enabled = true;
        animator.SetBool("Walk", false); // �ȱ� �ִϸ��̼� ���� ����

        nextPattern = CHARGE;
        currentPatternCoroutine = null;
        nextPatternPlay();
    }


    private void JumpEffectOn()
    {
        if (isDead) return;
        currentEffect = Instantiate(jumpEffectprefab, transform.position, Quaternion.identity);
        Destroy(currentEffect, 1f);  // ����Ʈ�� 2�� �� ���������
    }

    private IEnumerator Charge()
    {
        if (!inAttackRange || isDead) yield break;

        nav.isStopped = true;
        isCharging = true;

        isChargeSetting = true;
        attackRange = 3f;
        animator.SetBool("Run Forward", true);

        Vector3 targetPosition = player.transform.position;
        yield return StartCoroutine(ShowChargeGroundEffect(targetPosition));

        if (isDead)
        {
            CleanupEffects();
            yield break;
        }

        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
        }

        float startTime = Time.time;
        while (Time.time < startTime + chargeDuration)
        {
            if (isDead)
            {
                CleanupEffects();
                yield break;
            }

            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3.0f);
            foreach (var hit in hitColliders)
            {
                if (hit.CompareTag("Player"))
                {
                    thirdPersonController.TakeDamage(chargeDamage, transform.position);
                    isCharging = false;
                    isChargeSetting = false;
                    break;
                }
            }

            if (!isCharging) break;
            yield return null;
        }

        animator.SetBool("Run Forward", false);
        animator.SetTrigger("Attack3");
        yield return new WaitForSeconds(1f);

        isCharging = false;
        isChargeSetting = false;

        yield return new WaitForSeconds(5.0f);

        if (isDead)
        {
            CleanupEffects();
            yield break;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            nextPattern = ATTACK;
            currentPatternCoroutine = null;
            nextPatternPlay();
        }
        else
        {
            isAttack = false;
        }
    }


    private void CleanupEffects()
    {
        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
            currentGroundEffect = null;
        }

        // �߰� ����Ʈ�� ������ ���⿡ �� ����
    }

    private void nextPatternPlay()
    {
        if (isDead) return;

        if (currentPatternCoroutine != null)
        {
            StopCoroutine(currentPatternCoroutine);
            currentPatternCoroutine = null;
        }

        switch (nextPattern)
        {
            case 0:
                currentPatternCoroutine = StartCoroutine(Attack());
                break;
            case 1:
                currentPatternCoroutine = StartCoroutine(Jump());
                break;
            case 2:
                currentPatternCoroutine = StartCoroutine(Charge());
                break;
        }
    }

    private IEnumerator ShowJumpGroundEffect()
    {
        nav.gameObject.GetComponent<NavMeshAgent>().enabled = false; // NavMeshAgent ��Ȱ��ȭ
        if (jumpGroundEffectPrefab != null)
        {
            currentGroundEffect = Instantiate(jumpGroundEffectPrefab, new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z), Quaternion.identity);
            currentGroundEffect.transform.localScale = Vector3.zero;

            float duration = 2.0f;
            float elapsedTime = 0f; 

            //Debug.Log("�ٴ� ��� ȿ���� ���� Ŀ���ϴ�.");
            while (elapsedTime < duration)
            {
                if (isDead)
                {
                    Destroy(currentGroundEffect);
                    currentGroundEffect = null;
                    yield break;
                }
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
            Vector3 middlePosition = transform.position + ((targetPosition - transform.position) / 2);
            currentGroundEffect = Instantiate(chargeGoundEffectPrefab, new Vector3(middlePosition.x, transform.position.y + 0.01f, middlePosition.z), Quaternion.identity);

            // ���� ������ y���� 0���� �����Ͽ� y�� ȸ���� ����
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;
            currentGroundEffect.transform.rotation = Quaternion.LookRotation(direction);

            currentGroundEffect.transform.localScale = new Vector3(direction.z / 2, 0.01f, 1.3f);
        }

        yield return new WaitForSeconds(2.0f); // 2�ʰ� ����
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();
        if (!Manager.stage_01_clear)
        {
            Manager.stage_01_clear = true;
            Debug.Log("�������� 1 Ŭ����");
        }
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
