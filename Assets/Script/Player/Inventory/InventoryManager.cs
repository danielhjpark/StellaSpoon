using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private bool weightUpdatePaused = false;
    
    public int totalWeight = 0;

    public Text totalWeightText; // �� ���Ը� ǥ���� �ؽ�Ʈ

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

    // ���� ������Ʈ�� �Ͻ������� ��Ȱ��ȭ
    public void PauseWeightUpdate()
    {
        weightUpdatePaused = true;
    }

    // ���� ������Ʈ�� �ٽ� Ȱ��ȭ
    public void ResumeWeightUpdate()
    {
        weightUpdatePaused = false;
        RecalculateTotalWeight();
    }

    // ���� ������Ʈ
    public void UpdateTotalWeight(int weightChange)
    {
        if (!weightUpdatePaused)
        {
            totalWeight += weightChange;
            totalWeightText.text = totalWeight.ToString();
            Debug.Log($"Total Weight: {totalWeight}");
        }
    }

    // ��ü ���� ���� (�ʿ��� ���)
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


