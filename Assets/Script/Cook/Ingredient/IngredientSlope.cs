using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class IngredientSlope : MonoBehaviour
{
    private const float slideSpeed = 1f; // 미끄러지는 속도
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
        // 중앙 방향 벡터
        if (bowlCenter == null) return;
        else
        {
            Vector3 bowlDistance = new Vector3(bowlCenter.position.x, 0, bowlCenter.position.z);
            Vector3 ingredientDistance = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            float distance = Vector3.Distance(bowlDistance, ingredientDistance);
            if (distance <= 0.7) return;
        }

        Vector3 toCenter = (bowlCenter.position - transform.position).normalized;

        // 빗면에 투영된 중앙 방향 계산
        Vector3 slideDirection = Vector3.ProjectOnPlane(toCenter, Vector3.down).normalized;

        // 힘 또는 속도 적용 (중력 기반으로 하려면 AddForce 사용)
        rb.MovePosition(transform.position + slideDirection * slideSpeed * Time.deltaTime);
        //rb.AddForce(transform.position + slideDirection * slideSpeed * Time.fixedDeltaTime / 2);

    }
}
