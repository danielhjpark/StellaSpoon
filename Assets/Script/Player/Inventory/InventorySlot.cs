using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : Slot
{
    private int inventoryItemWeightTotal = 0;

    public override void OnDrop(PointerEventData eventData)
    {
        inventoryItemWeightTotal = 0;
        if (DragSlot.instance.dragSlot != null) // �巡�� ������ �����ϴ� ���
        {
            if (DragSlot.instance.dragSlot is TreasureChestSlot)
            {
                inventoryItemWeightTotal += DragSlot.instance.dragSlot.itemCount * DragSlot.instance.dragSlot.item.itemWeight;//������ ���� * ������ ����
                InventoryManager.instance.UpdateTotalWeight(inventoryItemWeightTotal);

                ChangeSlot();
            }
            //if�� �߰��� ���� ���� Ž�� ����
            else
            {
                ChangeSlot();
            }
        }
    }
}