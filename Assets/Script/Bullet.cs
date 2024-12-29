using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 20;

    private void OnTriggerEnter(Collider other)
    {
        MonsterBase monster = other.GetComponent<MonsterBase>();
        if (monster != null)
        {
            Debug.Log("충돌");
            monster.Damage(bulletDamage); // 몬스터에게 피해 전달
            Destroy(gameObject); // 총알 제거
        }
    }
}
