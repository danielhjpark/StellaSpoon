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
        // ���� �޽� ������Ʈ(���� ������Ʈ)�� �״�� ����
        MeshFilter originalMeshFilter = GetComponent<MeshFilter>();

        // �ܰ����� ������Ʈ ����
        outlineObj = new GameObject("Outline");
        outlineObj.transform.parent = transform;
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localRotation = Quaternion.identity;
        outlineObj.transform.localScale = Vector3.one;

        var meshFilter = outlineObj.AddComponent<MeshFilter>();
        meshFilter.mesh = GetComponent<MeshFilter>().mesh;

        var meshRenderer = outlineObj.AddComponent<MeshRenderer>();

        // �ܰ��� ���� ���׸��� ����
        Material outlineMat = new Material(Shader.Find("Custom/ObjectOutline"));
        outlineMat.SetColor("_Color", outlineColor);
        outlineMat.SetInt("_ZWrite", 1); // ���� ���� ���� Ȱ��ȭ
        outlineMat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always); // ���� �� �׻� �׸���
        outlineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Front); // �ո� ����
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
