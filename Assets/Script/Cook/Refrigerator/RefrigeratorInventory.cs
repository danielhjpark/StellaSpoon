using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RefrigeratorInventory : Inventory
{
    public RefrigeratorSlot[] refrigeratorSlots;
    [SerializeField] GameObject slotParent;
    List<Item> Refriitems;
    void Awake()
    {
        refrigeratorSlots = slotParent.GetComponentsInChildren<RefrigeratorSlot>();
        Refriitems = new List<Item>();
        foreach (RefrigeratorSlot slot in refrigeratorSlots)
        {
            slot.OnSlotUpdate += SlotUpate;
        }
    }

    [SerializeField]
    private Item[] refriInventoryitems;  // 냉장고용 아이템들

    public override Slot[] GetSlots()
    {
        // RefrigeratorInventory는 기본 slots이 아닌 refrigeratorSlots를 기준으로 동작해야 함
        return refrigeratorSlots;
    }

    public override void LoadToInven(int _arrNum, string _itemName, int _itemCount)
    {
        for (int i = 0; i < refriInventoryitems.Length; i++)
        {
            if (refriInventoryitems[i].itemName == _itemName)
            {
                if (_arrNum >= 0 && _arrNum < refrigeratorSlots.Length && refrigeratorSlots[_arrNum] != null)
                {
                    refrigeratorSlots[_arrNum].AddItem(refriInventoryitems[i], _itemCount);
                    Debug.Log($"[냉장고] 슬롯 {_arrNum}에 {_itemName} {_itemCount}개 로드됨");
                    return;
                }
                else
                {
                    Debug.LogError($"[냉장고] 잘못된 슬롯 인덱스 {_arrNum} 또는 null 슬롯");
                    return;
                }
            }
        }
        Debug.LogWarning($"[냉장고] Inventoryitems에 '{_itemName}' 아이템이 없습니다.");
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if(_item.itemType != Item.ItemType.contaminatedIngredient) return;
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
                        int spaceLeft = 100 - refrigeratorSlots[i].itemCount;

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
            // 2. 남은 개수를 빈 슬롯에 추가
            for (int i = 0; i < refrigeratorSlots.Length; i++)
            {
                // 슬롯이 비어 있는 경우
                if (refrigeratorSlots[i].item == null)
                {
                    if (_count <= 100)
                    {
                        // 남은 아이템이 20개 이하이면 한 슬롯에 모두 추가
                        refrigeratorSlots[i].AddItem(_item, _count);
                        return;
                    }
                    else
                    {
                        // 남은 아이템이 20개 초과이면 가능한 개수만 추가
                        refrigeratorSlots[i].AddItem(_item, 100);
                        _count -= 100; // 초과분 저장
                    }
                }
            }

            // 3. 아이템을 모두 추가하지 못한 경우
            if (_count > 0)
            {
                Debug.LogWarning("Not enough inventory space for all items.");
            }
        }
    }

    public void SlotUpate()
    {
        foreach (Ingredient ingredient in IngredientManager.IngredientAmount.Keys.ToList())
        {
            IngredientManager.IngredientAmount[ingredient] = 0;
        }

        foreach (RefrigeratorSlot slot in refrigeratorSlots)
        {
            if (slot.item != null)
            {
                string ingredient = slot.item.itemName;
                int ingredientAmount = slot.itemCount;
                Ingredient currentIngredient = IngredientManager.instance.FindIngredient(ingredient);
                IngredientManager.IngredientAmount[currentIngredient] += ingredientAmount;
            }
        }
    }

    public void UseIngredient(Ingredient currentIngredient, int count)
    {
        int currentCount = count;
        foreach (RefrigeratorSlot slot in refrigeratorSlots)
        {
            if (slot.item != null)
            {
                if (slot.currentIngredient == currentIngredient)
                {
                    if (slot.itemCount < currentCount)
                    {
                        currentCount -= slot.itemCount;
                        slot.UseItem(slot.itemCount);
                    }
                    else
                    {
                        slot.UseItem(currentCount);
                        if (IngredientManager.IngredientAmount[currentIngredient] <= 0) Refriitems.Add(slot.previousItem);
                        return;
                    }
                }
            }
        }
    }

    public void AddIngredient(Ingredient currentIngredient, int Count)
    {
        foreach (RefrigeratorSlot slot in refrigeratorSlots)
        {
            if (slot.currentIngredient == null)
            {
                slot.RecallItem(Count);
                return;
            }
        }

    }

    //Recall Ingredient From DailyMenuSystem
    public void RecallIngredient(Ingredient currentIngredient, int Count)
    {
        foreach (RefrigeratorSlot slot in refrigeratorSlots)
        {
            if (slot.currentIngredient == currentIngredient)
            {
                slot.RecallItem(Count);
                return;
            }
        }

        foreach (Item item in Refriitems)
        {
            if (item.itemName == currentIngredient.ingredientName)
            {
                foreach (RefrigeratorSlot slot in refrigeratorSlots)
                {
                    if (slot.item == null)
                    {
                        slot.previousItem = item;
                        slot.RecallItem(Count);
                        Refriitems.Remove(item);
                        return;
                    }
                }
            }
        }
    }
}
