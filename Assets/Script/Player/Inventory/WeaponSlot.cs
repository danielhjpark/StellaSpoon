using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : Slot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) // �巡�� ������ �����ϴ� ���
        {
            // �巡�׵� �������� Ÿ���� Equipment���� Ȯ��
            if (DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
            {
                ChangeSlot(); // �ùٸ� Ÿ���̸� ���� ��ü
                //CheckWeaponRifle(); // ������ �������� Weapon_02���� Ȯ��
            }
            else
            {
                // �ùٸ��� ���� Ÿ���� ��� �巡�� �̹��� �ʱ�ȭ �� ���� ���� ����
                Debug.LogWarning("�ش� ���Կ��� ��� �����۸� ������ �� �ֽ��ϴ�.");

                // �巡�� �̹��� �ʱ�ȭ
                DragSlot.instance.SetColor(0);

                // �巡�� ������ �������� �״�� ���� (���� ���� ���� ����)
                DragSlot.instance.dragSlot.AddItemWithoutWeight(
                    DragSlot.instance.dragSlot.item,
                    DragSlot.instance.dragSlot.itemCount
                );
            }
        }
    }

    // Weapon_02 ���� ���θ� Ȯ���ϴ� �޼���
    private void CheckWeaponRifle()
    {
        if (item != null && item.itemName == "WeaponRifle")
        {
            InventoryManager.instance.isWeaponRifle = true;
            Debug.Log("WeaponRifle ������!");
        }
        else
        {
            InventoryManager.instance.isWeaponRifle = false;
            Debug.Log("WeaponRifle �����ȵ�!");
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        InventoryManager.instance.isWeaponRifle = false;
        Debug.Log("WeaponRifle ���� ����");
    }
}

