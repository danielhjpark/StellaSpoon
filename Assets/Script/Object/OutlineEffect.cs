using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OutlineEffect : MonoBehaviour
{
    public Color outlineColor = Color.yellow;
    public float outlineWidth = 0.03f;
    private GameObject outlineObj;

    void Start()
    {
        // 기존 메쉬 오브젝트(원래 오브젝트)는 그대로 유지
        MeshFilter originalMeshFilter = GetComponent<MeshFilter>();

        // 외곽선용 오브젝트 생성
        outlineObj = new GameObject("Outline");
        outlineObj.transform.parent = transform;
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localRotation = Quaternion.identity;
        outlineObj.transform.localScale = Vector3.one;

        var meshFilter = outlineObj.AddComponent<MeshFilter>();
        meshFilter.mesh = GetComponent<MeshFilter>().mesh;

        var meshRenderer = outlineObj.AddComponent<MeshRenderer>();

        // 외곽선 전용 머테리얼 생성
        Material outlineMat = new Material(Shader.Find("Custom/ObjectOutline"));
        outlineMat.SetColor("_Color", outlineColor);
        outlineMat.SetInt("_ZWrite", 1); // 깊이 버퍼 쓰기 활성화
        outlineMat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always); // 깊이 비교 항상 그리기
        outlineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Front); // 앞면 제거
        outlineObj.layer = gameObject.layer;

        meshRenderer.material = outlineMat;

        outlineObj.layer = gameObject.layer;

        outlineObj.SetActive(false);
    }

    public void EnableOutline()
    {
        if (outlineObj != null)
            outlineObj.SetActive(true);
    }

    public void DisableOutline()
    {
        if (outlineObj != null)
            outlineObj.SetActive(false);
    }
}
