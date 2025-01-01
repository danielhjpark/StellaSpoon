using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // �κ��丮 Ȱ��ȭ ����. true�� �Ǹ� ī�޶� ������ �� �ʿ信 ���� �Է� ����

    [SerializeField]
    private GameObject go_InventoryBase; // �κ��丮 �̹���
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
        inventoryActivated = true;
        deviceManager.uiPanel.SetActive(false);
    }

    public void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
        inventoryActivated = false;
        deviceManager.uiPanel.SetActive(true);
    }

    // �������� ȹ���ϰ� �κ��丮 ���Կ� �߰��ϴ� �Լ�
    // _item: ȹ���� ������ ��ü
    // _count: ȹ���� ������ ���� (�⺻���� 1)
    public void AcquireItem(Item _item, int _count = 1)
    {
        // 1. �������� ��� Ÿ���� �ƴ� ���, ���� ���Կ��� ���� �������� ã�� �߰�
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for(int i = 0; i < slots.Length; i++)
            {
                // ���Կ� �̹� �������� �ִ� ���
                if (slots[i].item != null)
                {
                    // ������ ������ �̸��� ȹ���� ������ �̸��� ������
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        // �ش� ������ ������ ������ ������Ʈ�ϰ� �Լ� ����
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }
        // 2. �� ������ ã�� �������� �߰�
        for (int i = 0; i < slots.Length; i++)
        {
            // ������ ��� �ִ� ���
            if (slots[i].item == null)
            {
                // �ش� ���Կ� �����۰� ������ �߰��ϰ� �Լ� ����
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
