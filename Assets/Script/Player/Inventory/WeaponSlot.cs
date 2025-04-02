using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : Slot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) // 드래그 슬롯이 존재하는 경우
        {
            // 드래그된 아이템의 타입이 Equipment인지 확인
            if (DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
            {
                ChangeSlot(); // 올바른 타입이면 슬롯 교체
                //CheckWeaponRifle(); // 장착한 아이템이 Weapon_02인지 확인
            }
            else
            {
                // 올바르지 않은 타입일 경우 드래그 이미지 초기화 및 원래 슬롯 복구
                Debug.LogWarning("해당 슬롯에는 장비 아이템만 장착할 수 있습니다.");

                // 드래그 이미지 초기화
                DragSlot.instance.SetColor(0);

                // 드래그 슬롯의 아이템을 그대로 유지 (변경 없이 상태 복구)
                DragSlot.instance.dragSlot.AddItemWithoutWeight(
                    DragSlot.instance.dragSlot.item,
                    DragSlot.instance.dragSlot.itemCount
                );
            }
        }
    }

    // Weapon_02 장착 여부를 확인하는 메서드
    private void CheckWeaponRifle()
    {
        if (item != null && item.itemName == "WeaponRifle")
        {
            InventoryManager.instance.isWeaponRifle = true;
            Debug.Log("WeaponRifle 장착됨!");
        }
        else
        {
            InventoryManager.instance.isWeaponRifle = false;
            Debug.Log("WeaponRifle 장착안됨!");
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        InventoryManager.instance.isWeaponRifle = false;
        Debug.Log("WeaponRifle 장착 해제");
    }
}

