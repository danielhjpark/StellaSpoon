using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private bool weightUpdatePaused = false;
    
    public int totalWeight = 0;

    public Text totalWeightText; // 총 무게를 표시할 텍스트

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

    // 무게 업데이트를 일시적으로 비활성화
    public void PauseWeightUpdate()
    {
        weightUpdatePaused = true;
    }

    // 무게 업데이트를 다시 활성화
    public void ResumeWeightUpdate()
    {
        weightUpdatePaused = false;
        RecalculateTotalWeight();
    }

    // 무게 업데이트
    public void UpdateTotalWeight(int weightChange)
    {
        if (!weightUpdatePaused)
        {
            totalWeight += weightChange;
            totalWeightText.text = totalWeight.ToString();
            Debug.Log($"Total Weight: {totalWeight}");
        }
    }

    // 전체 무게 재계산 (필요할 경우)
    public void RecalculateTotalWeight()
    {
        totalWeight = 0;
        foreach (var slot in FindObjectsOfType<Slot>())
        {
            if (slot.item != null)
            {
                totalWeight += slot.item.itemWeight * slot.itemCount;
            }
        }
        Debug.Log($"Recalculated Total Weight: {totalWeight}");
    }
}


