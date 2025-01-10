using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : Slot
{
    private int inventoryItemWeightTotal = 0;

    public override void OnDrop(PointerEventData eventData)
    {
        inventoryItemWeightTotal = 0;
        if (DragSlot.instance.dragSlot != null) // 드래그 슬롯이 존재하는 경우
        {
            if (DragSlot.instance.dragSlot is TreasureChestSlot)
            {
                inventoryItemWeightTotal += DragSlot.instance.dragSlot.itemCount * DragSlot.instance.dragSlot.item.itemWeight;//아이템 수량 * 아이템 무게
                InventoryManager.instance.UpdateTotalWeight(inventoryItemWeightTotal);

                ChangeSlot();
            }
            //if문 추가로 슬롯 조건 탐색 가능
            else
            {
                ChangeSlot();
            }
        }
    }
}