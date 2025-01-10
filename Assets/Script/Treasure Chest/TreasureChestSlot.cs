using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureChestSlot : Slot
{
    private int chestItemWeightTotal = 0;
    public override void ClearSlot()
    {
        // 보물상자 슬롯에서 아이템을 제거할 때 무게 감소
        if (item != null)
        {
            if (this is TreasureChestSlot)
            {
                InventoryManager.instance.UpdateTotalWeight(-(item.itemWeight * itemCount));
            }
        }

        // 슬롯 초기화
        item = null;
        itemCount = 0;

        itemImage.sprite = null;
        text_count.gameObject.SetActive(false);

        SetColor(0);
    }
    public override void OnDrop(PointerEventData eventData)
    {
        chestItemWeightTotal = 0;//총무게 초기화
        if (DragSlot.instance.dragSlot != null) // 드래그 슬롯이 존재하는 경우
        {
            if (DragSlot.instance.dragSlot is InventorySlot)
            {
                chestItemWeightTotal += DragSlot.instance.dragSlot.itemCount * DragSlot.instance.dragSlot.item.itemWeight;//아이템 수량 * 아이템 무게
                InventoryManager.instance.UpdateTotalWeight(-chestItemWeightTotal);

                ChangeSlot();

                DragSlot.instance.dragSlot.ClearSlot();
            }
            else
            {
                ChangeSlot();
            }
        }
    }
}