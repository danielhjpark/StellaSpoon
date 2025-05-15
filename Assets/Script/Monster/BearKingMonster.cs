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
    public float shockwaveRadius = 5.0f; // 충격파 범위
    public float chargeSpeed = 10.0f;
    private float chargeDuration = 2f;

    private int JumpDamage = 50; //내려찍기 데미지
    private int chargeDamage = 60; //돌진 데미지

    public Transform playerTf;

    public Collider leftHandCollider;
    public Collider rightHandCollider;
    public GameObject jumpGroundEffectPrefab; //바닥 이펙트 프리팹
    public GameObject chargeGoundEffectPrefab; //돌진 이펙트 프리팹

    private bool isCharging = false;
    public static bool isChargeSetting = false; //돌진 준비 완료 여부
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
        if(!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        //Debug.Log("기본 공격 시작!");
        animator.SetTrigger("Attack8");
        attackRange = 15f;
        yield return new WaitForSeconds(5.0f);
        //Debug.Log("기본 공격 종료!");
        nextPattern = JUMP;
        nextPatternPlay();
    }

    private IEnumerator Jump()
    {
        if (!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        isJumping = true;
        //Debug.Log("내려찍기 시작!");
        yield return StartCoroutine(ShowJumpGroundEffect()); //바닥 경고 효과

        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
        }

        //Debug.Log("충격파 발생!");
        animator.SetTrigger("Attack5");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shockwaveRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                //Debug.Log("플레이어가 충격파를 맞았습니다!");
                thirdPersonController.TakeDamage(JumpDamage, transform.position); //플레이어 데미지
            }
        }

        nav.gameObject.GetComponent<NavMeshAgent>().enabled = true; // NavMeshAgent 활성화

        attackRange = 15f;
        yield return new WaitForSeconds(6.0f);
        isJumping = false;
        nextPattern = CHARGE;
        nextPatternPlay();
    }

    private IEnumerator Charge()
    {
        if (!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        //Debug.Log("돌진 준비 시작!");
        isChargeSetting = true;
        animator.SetBool("Run Forward", true);
        // animator.SetTrigger("Charge");

        // 돌진 준비 시 플레이어의 현재 위치 저장
        Vector3 targetPosition = player.transform.position;

        // 돌진 준비 시 chargeGroundEffectPrefab 생성 및 크기 조정
        yield return StartCoroutine(ShowChargeGroundEffect(targetPosition));

        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
        }
        //Debug.Log("돌진 시작!");
        isCharging = true;

        // 돌격 시작 시 NavMeshAgent 비활성화 (직접 이동을 위해)
        nav.isStopped = true;

        float startTime = Time.time;

        while (Time.time < startTime + chargeDuration)
        {
            // 목표 위치까지의 거리 계산
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // 목표에 도달하면 돌격 종료
            if (distanceToTarget <= 2f)
            {
                break;
            }

            // 목표 방향으로 이동
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;

            // 충돌 감지
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3.0f); // 충돌 반경 설정
            foreach (var hit in hitColliders)
            {
                if (hit.CompareTag("Player"))
                {
                    //Debug.Log("플레이어가 돌진에 맞았습니다!");
                    thirdPersonController.TakeDamage(chargeDamage, transform.position); // 플레이어에게 데미지
                    isCharging = false;
                    isChargeSetting = false;
                    break; // 충돌 후 루프 종료
                }
            }

            if (!isCharging) break; // 충돌 발생 시 돌진 종료

            yield return null;
        }

        animator.SetBool("Run Forward", false);
        animator.SetTrigger("Attack3");
        
        yield return new WaitForSeconds(1f); // 애니메이션 대기 시간
        isCharging = false;
        isChargeSetting = false;

        //Debug.Log("돌진 종료!");
        attackRange = 3f;
        yield return new WaitForSeconds(5.0f);

        // 플레이어가 다시 공격 범위에 있는지 확인 후 공격
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            nextPattern = ATTACK;
            nextPatternPlay();
        }
        else
        {
            // 플레이어가 너무 멀면 공격 생략하고 대기
            //Debug.Log("플레이어가 너무 멀어서 공격을 생략합니다.");
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
        nav.gameObject.GetComponent<NavMeshAgent>().enabled = false; // NavMeshAgent 비활성화
        if (jumpGroundEffectPrefab != null)
        {
            currentGroundEffect = Instantiate(jumpGroundEffectPrefab, new Vector3(transform.position.x, 0.01f, transform.position.z), Quaternion.identity);
            currentGroundEffect.transform.localScale = Vector3.zero;

            float duration = 2.0f;
            float elapsedTime = 0f;

            //Debug.Log("바닥 경고 효과가 점차 커집니다.");
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
        yield return new WaitForSeconds(2.0f); // 2초간 멈춤
    }

    private IEnumerator ShowChargeGroundEffect(Vector3 targetPosition)
    {
        if (chargeGoundEffectPrefab != null)
        {
            Vector3 middlePosition =  transform.position + ((targetPosition - transform.position)/2);
            currentGroundEffect = Instantiate(chargeGoundEffectPrefab, new Vector3(middlePosition.x, 0.01f, middlePosition.z), Quaternion.identity);

            // 방향 벡터의 y값을 0으로 설정하여 y축 회전만 적용
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;
            currentGroundEffect.transform.rotation = Quaternion.LookRotation(direction);

            currentGroundEffect.transform.localScale = new Vector3(direction.z / 2, 0.01f, 1.3f);
        }

        yield return new WaitForSeconds(2.0f); // 2초간 멈춤
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
