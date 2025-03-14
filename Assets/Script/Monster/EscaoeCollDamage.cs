using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscaoeCollDamage : MonoBehaviour
{
    private MonsterBase monsterBase;
    private bool canDealDamage = true; // �������� �� �� �ִ��� ���θ� ��Ÿ���� �÷���

    private void Start()
    {
        monsterBase = GetComponentInParent<MonsterBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹���� ��
        if (other.CompareTag("Player") && canDealDamage)
        {
            monsterBase.PlayerDamage();
            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDealDamage = false; // �������� �� �� ������ ����
        yield return new WaitForSeconds(5f); // 5�� ���
        canDealDamage = true; // �ٽ� �������� �� �� �ֵ��� ����
    }
}
