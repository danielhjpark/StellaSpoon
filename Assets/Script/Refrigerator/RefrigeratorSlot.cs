using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class RefrigeratorSlot : Slot
{
    public Ingredient currentIngredient;
    public Item previousItem;
    public event Action OnSlotUpdate;

    override public void AddItem(Item _item, int _count = 1) {
        if(!IsTypeIngredient(_item)) {
            return;
        }
        base.AddItem(_item, _count);
        previousItem = _item;
        currentIngredient = IngredientManager.instance.FindIngredient(_item.itemName);
    }

    override public void ChangeSlot()
    {
        if(!IsTypeIngredient(DragSlot.instance.dragSlot.item)){return;}

        Item _tempItem = item;
        int _tempItemCount = itemCount;
        
        if (_tempItem != null) {
            int addItemCount;
            if(_tempItem.itemName == DragSlot.instance.dragSlot.item.itemName){
                addItemCount = itemCount + DragSlot.instance.dragSlot.itemCount;
                AddItem(DragSlot.instance.dragSlot.item, addItemCount);
                DragSlot.instance.dragSlot.ClearSlot();
            }
            else {
               addItemCount = DragSlot.instance.dragSlot.itemCount;
                AddItem(DragSlot.instance.dragSlot.item, addItemCount);
                DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
            }
        }
        else {
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            DragSlot.instance.dragSlot.ClearSlot();
        } 

    }

    private bool IsTypeIngredient(Item _item) {
        if(_item.itemType.ToString() == "Ingredient" ||_item.itemType.ToString() == "contaminatedIngredient") {
            return true;
           
        }
         return false;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null && DragSlot.instance.dragSlot != this) ChangeSlot();
        OnSlotUpdate?.Invoke();
    }

    override public void ClearSlot()
    {
        // 아이템 데이터 초기화
        previousItem = item;
        currentIngredient = null;
        item = null;
        itemCount = 0;

        // 슬롯의 이미지와 텍스트를 초기화
        itemImage.sprite = null;
        text_count.gameObject.SetActive(false);

        // 슬롯을 비활성화 색상으로 표시
        SetColor(0);
        OnSlotUpdate?.Invoke();
    }

    public void UseItem(int count) {
        SetSlotCount(-count);
    }

    public void RecallItem(int count) {
        if(item == null) {
            AddItem(previousItem, count);
            return;
        }
        else SetSlotCount(count);
    }
}
