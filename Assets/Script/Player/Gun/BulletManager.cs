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
    private GameObject hitEffectPrefab; // �浹 �� ������ ��ƼŬ ������

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
            Debug.Log("�浹");
            monster.Damage(bulletDamage);
        }

        // ��ƼŬ ����Ʈ�� �ش� ��ġ�� ����
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        DestroyBullet();
    }
}
