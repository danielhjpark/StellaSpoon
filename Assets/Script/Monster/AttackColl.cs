using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColl : MonoBehaviour
{
    private MonsterBase monsterBase;

    private void Start()
    {
        monsterBase = GetComponentInParent<MonsterBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹���� ��
        if (other.CompareTag("Player"))
        {
            monsterBase.attackColl = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // �÷��̾�� �浹���� �ʾ��� ��
        if (other.CompareTag("Player"))
        {
            monsterBase.attackColl = false;
        }
    }
}
