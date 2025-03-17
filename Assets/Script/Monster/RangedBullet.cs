using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBullet : RangedMonster
{
    public GameObject player_; // �÷��̾�
    protected ThirdPersonController thirdPersonController_;
    public Transform playerTr; // �÷��̾��� ��ġ
    public float moveSpeed = 5f; // �̵� �ӵ�

    private Vector3 direction; //�Ѿ� ����

    private new void Start()
    {
        player_ = GameObject.FindWithTag("Player");
        playerTr = player_.transform;
        thirdPersonController_ = player.GetComponent<ThirdPersonController>();

        if (playerTr != null )
        {
            // ���� ��ġ���� �÷��̾��� ������ ���
            direction = (playerTr.position - transform.position).normalized;

            direction.y = 0;
        }

        Destroy(gameObject, 1.5f);
    }
    private new void Update()
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

            thirdPersonController.TakeDamage(damage, attackerPosition);
        }
        if(other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
