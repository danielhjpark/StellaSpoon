using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargeMonster : MonsterBase
{
    private float chargeDuration = 2f;
    private bool isCharging = false;
    [Header("돌격 몬스터 information")]
    [SerializeField]
    private float chargeSpeed = 10f; //돌격속도
    private new void Start()
    {
        base.Start();
        maxHealth = 100;
        currentHealth = maxHealth;
        damage = 10;
        idleMoveInterval = 2f;
        damageDelayTime = 5f;


        isDead = false;
        isMove = false;

        attackRange = 5f;
        playerDetectionRange = 6f;
        randomMoveRange = 7f;
        damageRange = 10f;
        nav.avoidancePriority = Random.Range(30, 60); // 회피 우선순위를 랜덤으로 설정
    }

    protected override void HandleAttack()
    {
        if (!isAttack || Time.time - lastAttackTime >= damageDelayTime)
        {
            //공격 구현
            lastAttackTime = Time.time;
            isAttack = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
            if (!isCharging)
            {
                Debug.Log("돌격 시작!");
                StartCoroutine(Charge());
            }
        }
    }
    private IEnumerator Charge()
    {
        isCharging = true;
        Vector3 targetPosition = player.transform.position - (player.transform.position - transform.position).normalized * 0.3f; //목표보다 0.3 앞에 멈추게

        // 돌격 시작 시 NavMeshAgent 비활성화 (직접 이동을 위해)
        nav.isStopped = true;

        float startTime = Time.time;

        while (Time.time < startTime + chargeDuration)
        {
            // 목표 위치까지의 거리 계산
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // 목표에 도달하면 돌격 종료
            if (distanceToTarget <= 1f)
            {
                break;
            }

            // 목표 방향으로 이동
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;

            yield return null;
        }

        Debug.Log("돌격 종료!");
        StartCoroutine(WaitCharge());
        isCharging = false;

        // NavMeshAgent 다시 활성화
        nav.isStopped = false;
        // 현재 위치를 새로운 위치로 업데이트하여 원래 자리로 돌아가지 않도록 설정
        nav.SetDestination(transform.position);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어와 충돌됨");
        }
        if (other.gameObject.CompareTag("Player") && isCharging)
        {
            Debug.Log("플레이어와 충돌! 돌격 종료.");
            StopCharging();
            Vector3 attackerPosition = transform.position; // 플레이어를 공격하는 방향
            thirdPersonController.TakeDamage(damage, attackerPosition);
        }
    }
    private void StopCharging()
    {
        isCharging = false;
        nav.isStopped = false; //NavMeshAgent 재활성화
    }
    IEnumerator WaitCharge()
    {
        yield return new WaitForSeconds(0.3f);
    }
}
