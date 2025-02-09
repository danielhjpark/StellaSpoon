using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody bulletRigidbody; // ������ �ٵ�� ����

    [SerializeField]
    private float moveSpeed = 10f; // �Ѿ��� �̵� �ӵ�
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

    // �Ѿ��� �̵� ����
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
