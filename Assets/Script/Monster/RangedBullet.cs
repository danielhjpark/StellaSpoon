using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBullet : RangedMonster
{
    public Transform playerTr; // �÷��̾��� ��ġ
    public float moveSpeed = 5f; // �̵� �ӵ�

    private Vector3 direction; //�Ѿ� ����

    private Rigidbody rb;

    private new void Start()
    {
        base.Start();

        playerTr = GameObject.FindWithTag("Player").transform;

        if (playerTr != null )
        {
            // ���� ��ġ���� �÷��̾��� ������ ���
            direction = (playerTr.position - transform.position).normalized;

            direction.y = 0;

        }

        Destroy(gameObject, 1.5f);
    }
    private void Update()
    {
        if (playerTr != null)
        {
            // �ش� �������� �̵�
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�÷��̾�� ������������
            Debug.Log("�÷��̾� ������ ������!");

            Destroy(gameObject); //��ô�� ����

            Vector3 attackerPosition = transform.position; // �÷��̾ �����ϴ� ���� Y�� ������ �ʿ�

            thirdPersonController.TakeDamage(attackDamage, attackerPosition);
        }
        if(other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
