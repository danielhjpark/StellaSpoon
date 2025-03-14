using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscaoeCollDamage : MonoBehaviour
{
    private MonsterBase monsterBase;
    private bool canDealDamage = true; // 데미지를 줄 수 있는지 여부를 나타내는 플래그

    private void Start()
    {
        monsterBase = GetComponentInParent<MonsterBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌했을 때
        if (other.CompareTag("Player") && canDealDamage)
        {
            monsterBase.PlayerDamage();
            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDealDamage = false; // 데미지를 줄 수 없도록 설정
        yield return new WaitForSeconds(5f); // 5초 대기
        canDealDamage = true; // 다시 데미지를 줄 수 있도록 설정
    }
}
