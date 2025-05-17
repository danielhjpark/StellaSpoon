using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaWolfKingBullet : WolfKingMonster
{
    public GameObject player_; // 플레이어
    public Transform playerTr; // 플레이어의 위치
    public float moveSpeed = 5f; // 이동 속도

    [SerializeField]
    private int deleteTime = 5; // 총알 삭제 시간

    private Vector3 direction; //총알 방향

    private new void Start()
    {
        direction = transform.forward;
        Destroy(gameObject, deleteTime); //5초 후 총알 삭제
    }
    private new void Update()
    {
        // 해당 방향으로 이동
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 attackerPosition = transform.position; // 플레이어를 공격하는 방향 Y값 보정이 필요
            thirdPersonController.TakeDamage(damage, attackerPosition);

            Destroy(gameObject); //투척물 제거
        }
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
