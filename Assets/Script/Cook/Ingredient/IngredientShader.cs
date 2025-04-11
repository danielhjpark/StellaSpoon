using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class IngredientShader : MonoBehaviour
{
    Renderer ingredientRenderer;
    Material ingredientMaterial;
    int totalMotionCount;
    float alphaValue;
    public void Start()
    {
        ingredientRenderer = GetComponent<Renderer>();
        ingredientMaterial = ingredientRenderer.material;
    }

    public void Initialize(int totalMotionCount) {
        this.totalMotionCount = totalMotionCount;
        alphaValue = GetShaderAlpha() / this.totalMotionCount;
    }

    float GetShaderAlpha() {
        float previousAlpha = 0f;
        if (ingredientMaterial.HasProperty("_Color2nd"))
        {
            Color currentColor = ingredientMaterial.GetColor("_Color2nd"); // ���� ���� ��������
            previousAlpha = currentColor.a;
        }
        return previousAlpha;
    }

    public void ApplyShaderAlpha() {
        if (ingredientMaterial.HasProperty("_Color2nd"))
        {
            Color currentColor = ingredientMaterial.GetColor("_Color2nd"); // ���� ���� ��������
            currentColor.a -= alphaValue; // ���� ���� ����
            ingredientMaterial.SetColor("_Color2nd", currentColor); // ���ο� ���� ����
        }
    }



}
