using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class IngredientSlope : MonoBehaviour
{
    private const float slideSpeed = 1f; // �̲������� �ӵ�
    private Transform bowlCenter;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Transform bowlCenter)
    {
        this.bowlCenter = bowlCenter;
    }

    void Update()
    {
        // �߾� ���� ����
        if (bowlCenter == null) return;
        else
        {
            Vector3 bowlDistance = new Vector3(bowlCenter.position.x, 0, bowlCenter.position.z);
            Vector3 ingredientDistance = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            float distance = Vector3.Distance(bowlDistance, ingredientDistance);
            if (distance <= 0.7) return;
        }

        Vector3 toCenter = (bowlCenter.position - transform.position).normalized;

        // ���鿡 ������ �߾� ���� ���
        Vector3 slideDirection = Vector3.ProjectOnPlane(toCenter, Vector3.down).normalized;

        // �� �Ǵ� �ӵ� ���� (�߷� ������� �Ϸ��� AddForce ���)
        rb.MovePosition(transform.position + slideDirection * slideSpeed * Time.deltaTime);
        //rb.AddForce(transform.position + slideDirection * slideSpeed * Time.fixedDeltaTime / 2);

    }
}
