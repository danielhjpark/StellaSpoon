using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; // 싱글톤으로 구현
    public Text totalWeightText; // 총 무게를 표시할 텍스트

    public float totalWeight = 0f; // 전체 인벤토리의 총 무게

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 총 무게 업데이트
    public void UpdateTotalWeight(float weightChange)
    {
        totalWeight += weightChange;
        totalWeightText.text = totalWeight.ToString();
    }
}

