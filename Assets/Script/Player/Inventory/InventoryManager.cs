using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private bool weightUpdatePaused = false;

    public int totalWeight = 0;
    public int extraWeaponWeight = 100; // ���Ⱑ �����Ǿ��� �� �߰��� ����

    public Text totalWeightText; // �� ���Ը� ǥ���� �ؽ�Ʈ

    public List<InventorySlot> inventorySlots; // ��� �κ��丮 ������ ��� ����Ʈ

    private bool _isWeaponRifle = false;
    public bool isWeaponRifle
    {
        get { return _isWeaponRifle; }
        set
        {
            _isWeaponRifle = value;
            RecalculateTotalWeight(); // ���� ���� ���ΰ� ����� �� ��ü ���� ����
        }
    }

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

            // ���Ⱑ ������ ��� �߰� ���� �ݿ�
            if (isWeaponRifle)
            {
                totalWeight += GetEquippedWeaponWeight();
            }

            totalWeightText.text = totalWeight.ToString();
            Debug.Log($"Total Weight: {totalWeight}");
        }
    }

    // ��ü ���� ���� (�ʿ��� ���)
    public void RecalculateTotalWeight()
    {
        totalWeight = 0;
        foreach (var slot in FindObjectsOfType<Slot>(true))
        {
            if (slot is TreasureChestSlot || slot is RefrigeratorSlot)
            {
                continue;
            }
            if (slot.item != null)
            {
                totalWeight += slot.item.itemWeight * slot.itemCount;
            }
        }

        // ���Ⱑ �����Ǿ��� ��� �߰� ���� ����
        if (isWeaponRifle)
        {
            totalWeight += GetEquippedWeaponWeight();
        }

        totalWeightText.text = totalWeight.ToString();
        //Debug.Log($"Recalculated Total Weight: {totalWeight}");
    }
    private int GetEquippedWeaponWeight()
    {
        if (RifleManager.instance == null) return 0;

        var weaponData = RifleManager.instance.GetCurrentWeaponData();
        return weaponData != null ? Mathf.RoundToInt(weaponData.weight) : 0;
    }

    public void ClearAllSlots()
    {
        foreach (var slot in inventorySlots)
        {
            slot.ClearSlot(); // �� ������ �������� �ʱ�ȭ
        }
    }
}
