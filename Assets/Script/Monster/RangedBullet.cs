using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBullet : MonoBehaviour
{
    public Transform player; // �÷��̾��� ��ġ
    public float moveSpeed = 5f; // �̵� �ӵ�

    private Vector3 direction; //�Ѿ� ����

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        if(player != null )
        {
            // ���� ��ġ���� �÷��̾��� ������ ���
            direction = (player.position - transform.position).normalized;
        }

        Destroy(gameObject, 1.5f);
    }
    private void Update()
    {
        if (player != null)
        {
            // �ش� �������� �̵�
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //�÷��̾�� ������������
            Debug.Log("�÷��̾� ������ ������!");
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
