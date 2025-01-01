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
            Debug.Log("�浹");
            monster.Damage(bulletDamage); // ���Ϳ��� ���� ����
            Destroy(gameObject); // �Ѿ� ����
        }
    }
}
