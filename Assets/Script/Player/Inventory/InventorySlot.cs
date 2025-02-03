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

    public override void ClearSlot()
    {
        if (item != null)
        {
            // 무게 업데이트 (현재 아이템 무게 * 아이템 개수)
            InventoryManager.instance.UpdateTotalWeight(-item.itemWeight * itemCount);
        }

        // 아이템 데이터 초기화
        item = null;
        itemCount = 0;

        // 슬롯의 이미지와 텍스트를 초기화
        itemImage.sprite = null;
        text_count.gameObject.SetActive(false);

        // 슬롯을 비활성화 색상으로 표시
        SetColor(0);
    }

    private void HandleMerge(Slot draggedSlot)
    {
        // 같은 아이템인지 확인
        if (draggedSlot.item != null && this.item != null && draggedSlot.item.itemName == this.item.itemName)
        {
            // 병합 조건: 현재 슬롯이 20개 미만일 때만 병합
            if (this.itemCount < 20 && draggedSlot.itemCount < 20)
            {
                // 병합 전 슬롯 무게 계산
                int originalWeight = this.itemCount * this.item.itemWeight;

                // 병합: 아이템 수량 합치기
                this.SetSlotCount(draggedSlot.itemCount);

                if (draggedSlot is InventorySlot == false)
                {
                    // 병합 후 슬롯 무게 계산
                    int newWeight = this.itemCount * this.item.itemWeight;

                    // 추가된 무게만큼 업데이트
                    int addedWeight = newWeight - originalWeight;
                    InventoryManager.instance.UpdateTotalWeight(addedWeight);
                }

                // 드래그된 슬롯의 아이템 초기화
                draggedSlot.ClearSlot();
            }
            else
            {
                // 병합 조건에 해당하지 않으면 단순 교환
                ChangeSlot();
            }
        }
        else
        {
            // 기본적으로 슬롯 변경 처리
            ChangeSlot();
        }
    }


}
