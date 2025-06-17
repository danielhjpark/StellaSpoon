using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineController : MonoBehaviour
{
    private Material originalMaterial;
    public Material outlineMaterial;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;  // 원래 머티리얼 저장
    }

    public void EnableOutline()
    {
        if (outlineMaterial != null)
            rend.material = outlineMaterial;
    }

    public void DisableOutline()
    {
        rend.material = originalMaterial;
    }
}