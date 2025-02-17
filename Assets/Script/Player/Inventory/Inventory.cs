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

    [SerializeField]
    private DeviceManager deviceManager;

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    public void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
        go_EquipmentBase.SetActive(true);
        inventoryActivated = true;
    }

    public void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
        go_EquipmentBase.SetActive(false);
        inventoryActivated = false;
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

}
