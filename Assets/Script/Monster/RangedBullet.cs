using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBullet : RangedMonster
{
    public Transform playerTr; // 플레이어의 위치
    public float moveSpeed = 5f; // 이동 속도

    private Vector3 direction; //총알 방향

    private Rigidbody rb;

    private new void Start()
    {
        base.Start();

        playerTr = GameObject.FindWithTag("Player").transform;

        if (playerTr != null )
        {
            // 현재 위치에서 플레이어의 방향을 계산
            direction = (playerTr.position - transform.position).normalized;

            direction.y = 0;

        }

        Destroy(gameObject, 1.5f);
    }
    private void Update()
    {
        if (playerTr != null)
        {
            // 해당 방향으로 이동
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //플레이어에게 데미지입히기
            Debug.Log("플레이어 데미지 입히기!");

            Destroy(gameObject); //투척물 제거

            Vector3 attackerPosition = transform.position; // 플레이어를 공격하는 방향 Y값 보정이 필요

            thirdPersonController.TakeDamage(attackDamage, attackerPosition);
        }
        if(other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
