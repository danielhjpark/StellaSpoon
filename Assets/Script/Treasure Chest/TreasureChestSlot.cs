using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureChestSlot : Slot
{
    private int chestItemWeightTotal = 0;
    public override void ClearSlot()
    {
        // �������� ���Կ��� �������� ������ �� ���� ����
        if (item != null)
        {
            if (this is TreasureChestSlot)
            {
                InventoryManager.instance.UpdateTotalWeight(-(item.itemWeight * itemCount));
            }
        }

        // ���� �ʱ�ȭ
        item = null;
        itemCount = 0;

        itemImage.sprite = null;
        text_count.gameObject.SetActive(false);

        SetColor(0);
    }
    public override void OnDrop(PointerEventData eventData)
    {
        chestItemWeightTotal = 0;//�ѹ��� �ʱ�ȭ
        if (DragSlot.instance.dragSlot != null) // �巡�� ������ �����ϴ� ���
        {
            if (DragSlot.instance.dragSlot is InventorySlot)
            {
                chestItemWeightTotal += DragSlot.instance.dragSlot.itemCount * DragSlot.instance.dragSlot.item.itemWeight;//������ ���� * ������ ����
                InventoryManager.instance.UpdateTotalWeight(-chestItemWeightTotal);

                ChangeSlot();

                DragSlot.instance.dragSlot.ClearSlot();
            }
            else
            {
                ChangeSlot();
            }
        }
    }
}