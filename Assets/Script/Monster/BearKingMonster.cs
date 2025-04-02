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
    public float shockwaveRadius = 5.0f; // 충격파 범위
    public float chargeSpeed = 10.0f;
    private float chargeDuration = 2f;

    public Transform playerTf;

    public Collider leftHandCollider;
    public Collider rightHandCollider;
    public GameObject jumpGroundEffectPrefab; // 바닥 이펙트 프리팹
    public GameObject chargeGoundEffectPrefab; // 돌진 이펙트 프리팹

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
            animator.SetBool("Attack", true); //Attack 애니메이션 실행*/
            //플레이어에게 데미지 주기
            StartCoroutine(Attack());
        }
    }


    private IEnumerator Attack()
    {
        if(!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        Debug.Log("기본 공격 시작!");
        // animator.SetTrigger("Attack");
        yield return new WaitForSeconds(5.0f);
        Debug.Log("기본 공격 종료!");
        nextPattern = JUMP;
        nextPatternPlay();
    }

    private IEnumerator Jump()
    {
        if (!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        Debug.Log("내려찍기 시작!");
        // animator.SetTrigger("Jump");
        yield return StartCoroutine(ShowJumpGroundEffect()); //바닥 경고 효과

        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
        }

        Debug.Log("충격파 발생!");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shockwaveRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("플레이어가 충격파를 맞았습니다!");
            }
        }

        yield return new WaitForSeconds(6.0f);
        nextPattern = CHARGE;
        nextPatternPlay();
    }

    private IEnumerator Charge()
    {
        if (!inAttackRange) yield break; //공격 범위 안에 플레이어가 없으면 공격하지 않음
        Debug.Log("돌진 준비 시작!");
        // animator.SetTrigger("Charge");

        // 돌진 준비 시 플레이어의 현재 위치 저장
        Vector3 targetPosition = player.transform.position;

        // 돌진 준비 시 chargeGroundEffectPrefab 생성 및 크기 조정
        yield return StartCoroutine(ShowChargeGroundEffect(targetPosition));

        if (currentGroundEffect != null)
        {
            Destroy(currentGroundEffect);
        }
        Debug.Log("돌진 시작!");
        isCharging = true;

        // 돌격 시작 시 NavMeshAgent 비활성화 (직접 이동을 위해)
        nav.isStopped = true;

        float startTime = Time.time;

        while (Time.time < startTime + chargeDuration)
        {
            // 목표 위치까지의 거리 계산
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // 목표에 도달하면 돌격 종료
            if (distanceToTarget <= 4f)
            {
                break;
            }

            // 목표 방향으로 이동
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;

            yield return null;
        }

        isCharging = false;
        Debug.Log("돌진 종료!");

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

            Debug.Log("바닥 경고 효과가 점차 커집니다.");
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

            currentGroundEffect.transform.localScale = new Vector3(direction.z / 2, 0.01f, 1);
        }

        yield return new WaitForSeconds(2.0f); // 2초간 멈춤
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shockwaveRadius);
    }
}
