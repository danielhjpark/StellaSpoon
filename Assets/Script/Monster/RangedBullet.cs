using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBullet : MonoBehaviour
{
    public Transform player; // 플레이어의 위치
    public float moveSpeed = 5f; // 이동 속도

    private Vector3 direction; //총알 방향

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        if(player != null )
        {
            // 현재 위치에서 플레이어의 방향을 계산
            direction = (player.position - transform.position).normalized;
        }

        Destroy(gameObject, 1.5f);
    }
    private void Update()
    {
        if (player != null)
        {
            // 해당 방향으로 이동
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //플레이어에게 데미지입히기
            Debug.Log("플레이어 데미지 입히기!");
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
