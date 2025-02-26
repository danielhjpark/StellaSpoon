using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RefrigeratorInventory : Inventory
{
    RefrigeratorSlot[] refrigeratorSlots;
    List<Item> items;
    void Start()
    {
        refrigeratorSlots = this.GetComponentsInChildren<RefrigeratorSlot>();
        items = new List<Item>();
        foreach (RefrigeratorSlot slot in refrigeratorSlots) {
            slot.OnSlotUpdate += SlotUpate;
        }
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        // 1. 아이템이 장비 타입이 아닌 경우, 기존 슬롯에서 같은 아이템을 찾아 추가
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < refrigeratorSlots.Length; i++)
            {
                // 슬롯에 이미 아이템이 있는 경우
                if (refrigeratorSlots[i].item != null)
                {
                    // 슬롯의 아이템 이름이 획득한 아이템 이름과 같으면
                    if (refrigeratorSlots[i].item.itemName == _item.itemName)
                    {
                        // 현재 슬롯의 남은 공간 계산
                        int spaceLeft = 20 - refrigeratorSlots[i].itemCount;

                        if (_count <= spaceLeft)
                        {
                            // 슬롯에 남은 공간이 충분하면 아이템 개수를 추가하고 함수 종료
                            refrigeratorSlots[i].SetSlotCount(_count);
                            return;
                        }
                        else
                        {
                            // 슬롯에 남은 공간이 부족하면 가능한 개수만 추가하고 초과분을 저장
                            refrigeratorSlots[i].SetSlotCount(spaceLeft);
                            _count -= spaceLeft; // 초과된 개수
                        }
                    }
                }
            }
        }
    }
    
    public void SlotUpate() {
        foreach(Ingredient ingredient in IngredientManager.IngredientAmount.Keys.ToList()) {
             IngredientManager.IngredientAmount[ingredient] = 0;
        }

        foreach (RefrigeratorSlot slot in refrigeratorSlots) {
            if(slot.item != null) {
                string ingredient = slot.item.itemName;
                int ingredientAmount = slot.itemCount;
                Ingredient currentIngredient = IngredientManager.instance.FindIngredient(ingredient);
                IngredientManager.IngredientAmount[currentIngredient] += ingredientAmount;
            }
        }
    }

    public void UseIngredient(Ingredient currentIngredient, int count) {
        int currentCount = count;
        foreach (RefrigeratorSlot slot in refrigeratorSlots) {
            if(slot.item != null) {
                if(slot.currentIngredient == currentIngredient) {
                    if(slot.itemCount < currentCount) {
                        currentCount -= slot.itemCount;
                        slot.UseItem(slot.itemCount);
                    }
                    else {
                        slot.UseItem(currentCount);
                        if(IngredientManager.IngredientAmount[currentIngredient] <= 0) items.Add(slot.previousItem);
                        return;
                    }
                }
            }
        }
    }

    public void RecallIngredient(Ingredient currentIngredient, int Count) {
        foreach (RefrigeratorSlot slot in refrigeratorSlots) {
            if(slot.currentIngredient == currentIngredient) {
                slot.RecallItem(Count);
                return;
            }
        }

        foreach(Item item in items) {
            if(item.itemName == currentIngredient.ingredientName) {
                foreach(RefrigeratorSlot slot in refrigeratorSlots) {
                    if(slot.item == null) {
                        slot.previousItem = item;
                        slot.RecallItem(Count);
                        items.Remove(item);
                        return;
                    }
                }
            }
        }


    }


}
