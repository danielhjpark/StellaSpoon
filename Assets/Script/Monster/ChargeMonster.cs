using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargeMonster : MonsterBase
{
    private float chargeDuration = 2f;
    private bool isCharging = false;
    [Header("���� ���� information")]
    [SerializeField]
    private float chargeSpeed = 10f; //���ݼӵ�

    protected override void HandleAttack()
    {
        if (!isAttack)
        {
            isAttack = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
            if (!isCharging)
            {
                Debug.Log("���� ����!");
                StartCoroutine(Charge());
            }
        }
    }
    private IEnumerator Charge()
    {
        isCharging = true;
        Vector3 targetPosition = player.transform.position - (player.transform.position - transform.position).normalized * 0.3f; //��ǥ���� 0.3 �տ� ���߰�

        // ���� ���� �� NavMeshAgent ��Ȱ��ȭ (���� �̵��� ����)
        nav.isStopped = true;

        float startTime = Time.time;

        while (Time.time < startTime + chargeDuration)
        {
            // ��ǥ ��ġ������ �Ÿ� ���
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // ��ǥ�� �����ϸ� ���� ����
            if (distanceToTarget <= 1f)
            {
                break;
            }

            // ��ǥ �������� �̵�
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;

            yield return null;
        }
        Debug.Log("���� ����!");
        StartCoroutine(WaitCharge());
        isCharging = false;

        // NavMeshAgent �ٽ� Ȱ��ȭ
        nav.isStopped = false;
        // ���� ��ġ�� ���ο� ��ġ�� ������Ʈ�Ͽ� ���� �ڸ��� ���ư��� �ʵ��� ����
        nav.SetDestination(transform.position);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && isCharging)
        {
            Debug.Log("�÷��̾�� �浹! ���� ����.");
            StopCharging();
        }
    }
    private void StopCharging()
    {
        isCharging = false;
        nav.isStopped = false; //NavMeshAgent ��Ȱ��ȭ
    }
    IEnumerator WaitCharge()
    {
        yield return new WaitForSeconds(0.3f);
    }
}
