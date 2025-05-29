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
    private Item[] refriInventoryitems;  // ������ �����۵�

    public override Slot[] GetSlots()
    {
        // RefrigeratorInventory�� �⺻ slots�� �ƴ� refrigeratorSlots�� �������� �����ؾ� ��
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
                    Debug.Log($"[�����] ���� {_arrNum}�� {_itemName} {_itemCount}�� �ε��");
                    return;
                }
                else
                {
                    Debug.LogError($"[�����] �߸��� ���� �ε��� {_arrNum} �Ǵ� null ����");
                    return;
                }
            }
        }
        Debug.LogWarning($"[�����] Inventoryitems�� '{_itemName}' �������� �����ϴ�.");
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if(_item.itemType != Item.ItemType.contaminatedIngredient) return;
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < refrigeratorSlots.Length; i++)
            {
                // ���Կ� �̹� �������� �ִ� ���
                if (refrigeratorSlots[i].item != null)
                {
                    // ������ ������ �̸��� ȹ���� ������ �̸��� ������
                    if (refrigeratorSlots[i].item.itemName == _item.itemName)
                    {
                        // ���� ������ ���� ���� ���
                        int spaceLeft = 100 - refrigeratorSlots[i].itemCount;

                        if (_count <= spaceLeft)
                        {
                            // ���Կ� ���� ������ ����ϸ� ������ ������ �߰��ϰ� �Լ� ����
                            refrigeratorSlots[i].SetSlotCount(_count);
                            return;
                        }
                        else
                        {
                            // ���Կ� ���� ������ �����ϸ� ������ ������ �߰��ϰ� �ʰ����� ����
                            refrigeratorSlots[i].SetSlotCount(spaceLeft);
                            _count -= spaceLeft; // �ʰ��� ����
                        }
                    }
                }
            }
            // 2. ���� ������ �� ���Կ� �߰�
            for (int i = 0; i < refrigeratorSlots.Length; i++)
            {
                // ������ ��� �ִ� ���
                if (refrigeratorSlots[i].item == null)
                {
                    if (_count <= 100)
                    {
                        // ���� �������� 20�� �����̸� �� ���Կ� ��� �߰�
                        refrigeratorSlots[i].AddItem(_item, _count);
                        return;
                    }
                    else
                    {
                        // ���� �������� 20�� �ʰ��̸� ������ ������ �߰�
                        refrigeratorSlots[i].AddItem(_item, 100);
                        _count -= 100; // �ʰ��� ����
                    }
                }
            }

            // 3. �������� ��� �߰����� ���� ���
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
