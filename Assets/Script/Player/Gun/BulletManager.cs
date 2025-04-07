using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody bulletRigidbody;

    [SerializeField]
    private float moveSpeed = 10f;

    private float destoryTime = 3f;

    [SerializeField]
    private int bulletDamage = 20;

    [SerializeField]
    private GameObject hitEffectPrefab; // 충돌 시 생성할 파티클 프리팹

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        destoryTime -= Time.deltaTime;

        if (destoryTime <= 0)
        {
            DestroyBullet();
        }

        BulletMove();
    }

    private void BulletMove()
    {
        bulletRigidbody.velocity = transform.forward * moveSpeed;
    }

    private void DestroyBullet()
    {
        gameObject.SetActive(false);
        destoryTime = 3f;
    }

    public void SetDamage(int damage)
    {
        bulletDamage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        MonsterBase monster = other.GetComponent<MonsterBase>();
        if (monster != null)
        {
            Debug.Log("충돌");
            monster.Damage(bulletDamage);
        }

        // 파티클 이펙트를 해당 위치에 생성
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        DestroyBullet();
    }
}
