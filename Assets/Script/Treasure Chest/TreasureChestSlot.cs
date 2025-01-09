using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureChestSlot : Slot
{
    override public void AddItem(Item _item, int _count = 1)
    {
        // 슬롯에 추가할 아이템 정보 저장
        item = _item;
        itemCount = _count;

        // 아이템 이미지를 슬롯에 표시
        itemImage.sprite = item.itemImage;

        // 아이템이 장비 타입이 아닌 경우, 아이템 개수를 텍스트로 표시
        if (item.itemType != Item.ItemType.Equipment)
        {
            text_count.text = itemCount.ToString();
            text_count.gameObject.SetActive(true);
        }
        else
        {
            // 장비 타입인 경우, 갯수를 알려주는 텍스트를 비활성화
            text_count.gameObject.SetActive(false);
        }

        // 슬롯의 색상 불투명도로 활성화 표시
        SetColor(1);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 슬롯이 존재하고, 해당 슬롯이 인벤토리 내에 있을 때
        if (DragSlot.instance.dragSlot != null)
        {
            // 인벤토리 슬롯으로 이동 시에만 무게 업데이트
            if (DragSlot.instance.dragSlot.GetComponent<Slot>().GetType() == typeof(Slot)) // InventorySlot으로만 이동
            {
                // 인벤토리 슬롯으로 아이템을 이동시킬 때 무게 업데이트
                InventoryManager.instance.UpdateTotalWeight(item.itemWeight * itemCount);
            }

            // 아이템을 인벤토리로 이동하는 경우, 보물상자에서 아이템을 제거
            ClearSlot();
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
    }
    // 슬롯을 초기화하는 함수 (아이템 제거)
    public override void ClearSlot()
    {
        // 아이템 무게 업데이트를 하지 않음
        if (item != null)
        {
            // 보물상자 슬롯에서는 무게를 업데이트하지 않음
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
}
