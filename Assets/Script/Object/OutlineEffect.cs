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
        //기존 메쉬 오브젝트(원래 오브젝트)는 그대로 유지
        MeshFilter originalMeshFilter = GetComponent<MeshFilter>();

        outlineObj = new GameObject("Outline");

        //외곽선 오브젝트는 원래 오브젝트의 자식으로 설정
        outlineObj.transform.parent = transform;

        //외곽선 오브젝트의 위치, 회전, 크기를 원래 오브젝트와 동일하게 설정
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localRotation = Quaternion.identity;
        outlineObj.transform.localScale = Vector3.one;

        //외곽선 오브젝트에 MeshFilter 추가하고 원본 메쉬를 할당
        var meshFilter = outlineObj.AddComponent<MeshFilter>();
        meshFilter.mesh = GetComponent<MeshFilter>().mesh;

        var meshRenderer = outlineObj.AddComponent<MeshRenderer>();//외곽선 오브젝트에 MeshRenderer 추가

        Material outlineMat = new Material(Shader.Find("Custom/ObjectOutline"));//외곽선 전용 머테리얼 생성

        outlineMat.SetColor("_Color", outlineColor);//외곽선 색상 설정

        outlineMat.SetInt("_ZWrite", 1);//깊이 버퍼 설정

        outlineMat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);//깊이 비교 항상 설정

        outlineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Front);//앞면 제거(Backface만 렌더링)

        outlineObj.layer = gameObject.layer;//외곽선 레이어 설정
        meshRenderer.material = outlineMat;//외곽선 머테리얼 설정

        outlineObj.SetActive(false);//기본적으로 외곽선 비활성화
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
