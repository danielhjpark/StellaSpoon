using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPreviewCamera : MonoBehaviour
{
    public Transform target;
    public float radius = 10f;
    public float angleFromTop = 45f;
    public float rotationSpeed = 30f;
    private float currentAngle = 0f;
    public Vector3 orbitOffset = new Vector3(0, 0, 1);
    void Update()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        float radians = currentAngle * Mathf.Deg2Rad;

        // 기준점을 타겟 위치에서 z축으로 1만큼 이동한 지점으로 설정
        Vector3 pivotPoint = target.position + orbitOffset;

        float y = Mathf.Sin(angleFromTop * Mathf.Deg2Rad) * radius;
        float horizontalRadius = Mathf.Cos(angleFromTop * Mathf.Deg2Rad) * radius;
        float x = Mathf.Cos(radians) * horizontalRadius;
        float z = Mathf.Sin(radians) * horizontalRadius;

        Vector3 offset = new Vector3(x, y, z);
        transform.position = pivotPoint + offset;

        // 여전히 타겟을 바라봄
        transform.LookAt(target.position);
    }
}
