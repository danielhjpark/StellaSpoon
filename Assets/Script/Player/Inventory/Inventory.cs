using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // �κ��丮 Ȱ��ȭ ����. true�� �Ǹ� ī�޶� ������ �� �ʿ信 ���� �Է� ����

    [SerializeField]
    private GameObject go_InventoryBase; // �κ��丮 �̹���
    [SerializeField]
    private GameObject go_EquipmentBase; // ��� â �̹���
    [SerializeField]
    private GameObject go_SlotsParent; // Slot���� �θ��� Grid Setting

    private Slot[] slots; // ���Ե� �迭

    private ItemNameData itemNameData;

    public virtual Slot[] GetSlots()
    {
        if (slots == null || slots.Length == 0)
        {
            slots = go_SlotsParent.GetComponentsInChildren<Slot>(true);
        }
        return slots;
    }

    [SerializeField]
    private Item[] Inventoryitems;

    public virtual void LoadToInven(int _arrNum, string _itemName, int _itemCount)
    {
        for (int i = 0; i < Inventoryitems.Length; i++)
        {
            if (Inventoryitems[i].itemName == _itemName)
            {
                if (_arrNum >= 0 && _arrNum < slots.Length && slots[_arrNum] != null)
                {
                    slots[_arrNum].AddItem(Inventoryitems[i], _itemCount);
                    return;
                }
                else
                {
                    Debug.LogError($"Invalid slot index {_arrNum} or slot is null.");
                    return;
                }
            }
        }
        Debug.LogWarning($"������ '{_itemName}' �� Inventoryitems���� ã�� �� �����ϴ�.");
    }
    private void Awake()
    {
        if (go_SlotsParent == null)
        {
            Debug.LogError("[Inventory] go_SlotsParent�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        else
        {
            slots = go_SlotsParent.GetComponentsInChildren<Slot>(true);
            Debug.Log($"[Inventory] ���� {slots.Length}�� �ʱ�ȭ �Ϸ�");
        }
    }
    void Start()
    {
        itemNameData = FindObjectOfType<ItemNameData>();
    }

    public void OpenInventory()
    {
        if (inventoryActivated) return;

        go_InventoryBase.SetActive(true);
        go_EquipmentBase.SetActive(true);
        inventoryActivated = true;
    }

    public void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
        go_EquipmentBase.SetActive(false);
        inventoryActivated = false;
        itemNameData.HideToolTip();
    }

    // �������� ȹ���ϰ� �κ��丮 ���Կ� �߰��ϴ� �Լ�
    // _item: ȹ���� ������ ��ü
    // _count: ȹ���� ������ ���� (�⺻���� 1)
    public void AcquireItem(Item _item, int _count = 1)
    {
        // 1. �������� ��� Ÿ���� �ƴ� ���, ���� ���Կ��� ���� �������� ã�� �߰�
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                // ���Կ� �̹� �������� �ִ� ���
                if (slots[i].item != null)
                {
                    // ������ ������ �̸��� ȹ���� ������ �̸��� ������
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        // ���� ������ ���� ���� ���
                        int spaceLeft = 20 - slots[i].itemCount;

                        if (_count <= spaceLeft)
                        {
                            // ���Կ� ���� ������ ����ϸ� ������ ������ �߰��ϰ� �Լ� ����
                            slots[i].SetSlotCount(_count);
                            return;
                        }
                        else
                        {
                            // ���Կ� ���� ������ �����ϸ� ������ ������ �߰��ϰ� �ʰ����� ����
                            slots[i].SetSlotCount(spaceLeft);
                            _count -= spaceLeft; // �ʰ��� ����
                        }
                    }
                }
            }
        }

        // 2. ���� ������ �� ���Կ� �߰�
        for (int i = 0; i < slots.Length; i++)
        {
            // ������ ��� �ִ� ���
            if (slots[i].item == null)
            {
                if (_count <= 20)
                {
                    // ���� �������� 20�� �����̸� �� ���Կ� ��� �߰�
                    slots[i].AddItem(_item, _count);
                    return;
                }
                else
                {
                    // ���� �������� 20�� �ʰ��̸� ������ ������ �߰�
                    slots[i].AddItem(_item, 20);
                    _count -= 20; // �ʰ��� ����
                }
            }
        }

        // 3. �������� ��� �߰����� ���� ���
        if (_count > 0)
        {
            Debug.LogWarning("Not enough inventory space for all items.");
        }
    }
    public int GetItemCount(string itemName)
    {
        int totalCount = 0;

        foreach (Slot slot in slots)
        {
            if (slot.item != null && slot.item.itemName == itemName)
            {
                totalCount += slot.itemCount;
            }
        }

        return totalCount;
    }
    public bool DecreaseItemCount(string itemName, int count)
    {
        int remainingToRemove = count;

        // �κ��丮���� �ڿ������� (�ֱ� �߰��� ���Ժ���) ����
        for (int i = slots.Length - 1; i >= 0 && remainingToRemove > 0; i--)
        {
            if (slots[i].item != null && slots[i].item.itemName == itemName)
            {
                int removeCount = Mathf.Min(remainingToRemove, slots[i].itemCount);
                slots[i].SetSlotCount(-removeCount);
                remainingToRemove -= removeCount;
            }
        }

        if (remainingToRemove > 0)
        {
            Debug.LogWarning($"[Inventory] {itemName} �������� {count}�� �����Ϸ� ������ {remainingToRemove}�� �����մϴ�.");
            return false;
        }

        return true;
    }
}
