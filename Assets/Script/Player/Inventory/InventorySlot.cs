using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : Slot
{
    //private int inventoryItemWeightTotal = 0;

    public override void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            Slot draggedSlot = DragSlot.instance.dragSlot;

            if (draggedSlot is TreasureChestSlot || draggedSlot is InventorySlot)
            {
                HandleMerge(draggedSlot); // 병합 로직 호출
            }
            else
            {
                ChangeSlot(); // 기본 슬롯 변경 처리
            }
        }
    }

    private void HandleMerge(Slot draggedSlot)
    {
        // 같은 아이템인지 확인
        if (draggedSlot.item != null && this.item != null && draggedSlot.item.itemName == this.item.itemName)
        {
            // 병합 전 슬롯 무게 계산
            int originalWeight = this.itemCount * this.item.itemWeight;

            // 병합: 아이템 수량 합치기
            this.SetSlotCount(draggedSlot.itemCount);

            // 병합 후 슬롯 무게 계산
            int newWeight = this.itemCount * this.item.itemWeight;

            // 추가된 무게만큼 업데이트
            int addedWeight = newWeight - originalWeight;
            InventoryManager.instance.UpdateTotalWeight(addedWeight);

            // 드래그된 슬롯의 아이템 초기화
            draggedSlot.ClearSlot();
        }
        else
        {
            // 기본적으로 슬롯 변경 처리
            ChangeSlot();
        }
    }

}
