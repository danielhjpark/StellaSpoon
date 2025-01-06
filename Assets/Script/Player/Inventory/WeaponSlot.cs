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
            }
            else
            {
                // �ùٸ��� ���� Ÿ���� ��� �巡�� �̹��� �ʱ�ȭ �� ���� ���� ����
                Debug.LogWarning("�ش� ���Կ��� ��� �����۸� ������ �� �ֽ��ϴ�.");

                // �巡�� �̹��� �ʱ�ȭ
                DragSlot.instance.SetColor(0);

                // �巡�� ������ �������� �״�� ���� (���� ���� ���� ����)
                DragSlot.instance.dragSlot.AddItem(
                    DragSlot.instance.dragSlot.item,
                    DragSlot.instance.dragSlot.itemCount
                );
            }
        }
    }
}

