using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody bulletRigidbody;

    [SerializeField] private float moveSpeed = 10f;
    private float destroyTime = 3f;

    private int bulletDamage = 20;

    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private AudioClip[] hitSFXClips;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
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
        destroyTime = 3f;
    }

    public void SetDamageFromWeapon(WeaponData data)
    {
        bulletDamage = data.damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        MonsterBase monster = other.GetComponent<MonsterBase>();
        if (monster != null)
        {
            monster.Damage(bulletDamage);
        }

        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        if (hitSFXClips != null && hitSFXClips.Length > 0)
        {
            int index = Random.Range(0, hitSFXClips.Length);
            AudioSource.PlayClipAtPoint(hitSFXClips[index], transform.position, 1f);
        }

        DestroyBullet();
    }
}

