using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : Slot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            Slot draggedSlot = DragSlot.instance.dragSlot;

            // 자기 자신에게 드롭한 경우 아무 동작하지 않음
            if (draggedSlot == this)
            {
                DragSlot.instance.SetColor(0);
                DragSlot.instance.gameObject.SetActive(false);
                DragSlot.instance.dragSlot = null;
                return;
            }

            // 레시피 아이템인 경우
            if (draggedSlot.item != null && draggedSlot.item.itemType == Item.ItemType.Recipe)
            {
                string recipeName = draggedSlot.item.itemName;

                Recipe targetRecipe = RecipeManager.instance.FindRecipe(recipeName);
                if (targetRecipe != null)
                {
                    RecipeManager.instance.RecipeUnLock(targetRecipe);
                    Debug.Log($"[InventorySlot] '{recipeName}' 레시피 등록 완료");
                }
                else
                {
                    Debug.LogWarning($"[InventorySlot] 레시피 '{recipeName}' 찾을 수 없음");
                }

                // 아이템 제거
                draggedSlot.ClearSlot();
                return;
            }

            // 병합 또는 슬롯 변경
            if (draggedSlot is TreasureChestSlot || draggedSlot is InventorySlot)
            {
                HandleMerge(draggedSlot);
            }
            else
            {
                ChangeSlot();
            }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }

    public override void ClearSlot()
    {
        if (item != null)
        {
            InventoryManager.instance.UpdateTotalWeight(-item.itemWeight * itemCount);
        }

        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        text_count.gameObject.SetActive(false);
        SetColor(0);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    private void HandleMerge(Slot draggedSlot)
    {
        // 자기 자신에게 병합 시도한 경우 무시
        if (draggedSlot == this)
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.gameObject.SetActive(false);
            DragSlot.instance.dragSlot = null;
            return;
        }

        // 병합 가능: 같은 아이템 + 20 미만
        if (draggedSlot.item != null && this.item != null && draggedSlot.item.itemName == this.item.itemName)
        {
            if (this.itemCount < 20 && draggedSlot.itemCount < 20)
            {
                int originalWeight = this.itemCount * this.item.itemWeight;

                // 병합: 아이템 수량 합치기
                this.SetSlotCount(draggedSlot.itemCount);

                if (!(draggedSlot is InventorySlot)) // 외부 슬롯에서 왔을 경우에만 무게 추가
                {
                    int newWeight = this.itemCount * this.item.itemWeight;
                    int addedWeight = newWeight - originalWeight;
                    InventoryManager.instance.UpdateTotalWeight(addedWeight);
                }

                // 병합 대상 슬롯 초기화
                draggedSlot.ClearSlot();
                return;
            }
        }

        // 병합 조건 미충족 → 기본 슬롯 교환
        ChangeSlot();
    }
}

