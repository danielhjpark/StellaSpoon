using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody bulletRigidbody; // 리지드 바디로 관리

    [SerializeField]
    private float moveSpeed = 10f; // 총알의 이동 속도
    private float destoryTime = 3f;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        destoryTime -= Time.deltaTime;
    
        if(destoryTime <= 0)
        {
            DestroyBullet();
        }

        BulletMove();
    }

    // 총알의 이동 구현
    private void BulletMove()
    {
        bulletRigidbody.velocity = transform.forward * moveSpeed; 
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
        destoryTime = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyBullet();
    }
}
