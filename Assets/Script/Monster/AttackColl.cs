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
        // 플레이어와 충돌했을 때
        if (other.CompareTag("Player"))
        {
            monsterBase.attackColl = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // 플레이어와 충돌하지 않았을 때
        if (other.CompareTag("Player"))
        {
            monsterBase.attackColl = false;
        }
    }
}
