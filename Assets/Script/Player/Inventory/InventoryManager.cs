using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; // �̱������� ����
    public Text totalWeightText; // �� ���Ը� ǥ���� �ؽ�Ʈ

    public float totalWeight = 0f; // ��ü �κ��丮�� �� ����

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

    // �� ���� ������Ʈ
    public void UpdateTotalWeight(float weightChange)
    {
        totalWeight += weightChange;
        totalWeightText.text = totalWeight.ToString();
    }
}

