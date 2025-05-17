using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaWolfKingBullet : WolfKingMonster
{
    public GameObject player_; // �÷��̾�
    public Transform playerTr; // �÷��̾��� ��ġ
    public float moveSpeed = 5f; // �̵� �ӵ�

    [SerializeField]
    private int deleteTime = 5; // �Ѿ� ���� �ð�

    private Vector3 direction; //�Ѿ� ����

    private new void Start()
    {
        direction = transform.forward;
        Destroy(gameObject, deleteTime); //5�� �� �Ѿ� ����
    }
    private new void Update()
    {
        // �ش� �������� �̵�
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 attackerPosition = transform.position; // �÷��̾ �����ϴ� ���� Y�� ������ �ʿ�
            thirdPersonController.TakeDamage(damage, attackerPosition);

            Destroy(gameObject); //��ô�� ����
        }
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
