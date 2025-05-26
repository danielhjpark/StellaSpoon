using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private bool weightUpdatePaused = false;

    public int totalWeight = 0;
    public int extraWeaponWeight = 100; // 무기가 장착되었을 때 추가할 무게

    public Text totalWeightText; // 총 무게를 표시할 텍스트

    public List<InventorySlot> inventorySlots; // 모든 인벤토리 슬롯을 담는 리스트

    private bool _isWeaponRifle = false;
    public bool isWeaponRifle
    {
        get { return _isWeaponRifle; }
        set
        {
            _isWeaponRifle = value;
            RecalculateTotalWeight(); // 무기 장착 여부가 변경될 때 전체 무게 재계산
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

            // 무기가 장착된 경우 추가 무게 반영
            if (isWeaponRifle)
            {
                totalWeight += GetEquippedWeaponWeight();
            }

            totalWeightText.text = totalWeight.ToString();
            Debug.Log($"Total Weight: {totalWeight}");
        }
    }

    // 전체 무게 재계산 (필요할 경우)
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

        // 무기가 장착되었을 경우 추가 무게 적용
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
            slot.ClearSlot(); // 각 슬롯의 아이템을 초기화
        }
    }
}
