using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureChestSlot : Slot
{
    //private int chestItemWeightTotal = 0;

    public override void ClearSlot()
    {
        // �������� ���Կ��� �������� ������ �� ���� ����
        if (item != null)
        {
            InventoryManager.instance.UpdateTotalWeight(-(item.itemWeight * itemCount));
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

        if (DragSlot.instance.dragSlot != null)
        {
            Slot draggedSlot = DragSlot.instance.dragSlot;

            if (draggedSlot is InventorySlot || draggedSlot is TreasureChestSlot)
            {
                HandleMerge(draggedSlot); // ���� ���� ȣ��
            }
            else
            {
                ChangeSlot(); // �⺻ ���� ���� ó��
            }
        }
    }

    private void HandleMerge(Slot draggedSlot)
    {
        // ���� ���������� Ȯ��
        if (draggedSlot.item != null && this.item != null && draggedSlot.item.itemName == this.item.itemName)
        {
            // ���� �� ���� ���� ���
            int originalWeight = this.itemCount * this.item.itemWeight;

            // ����: ������ ���� ��ġ��
            this.SetSlotCount(draggedSlot.itemCount);

            // ���� �� ���� ���� ���
            int newWeight = this.itemCount * this.item.itemWeight;

            // �߰��� ���Ը�ŭ ������Ʈ
            int addedWeight = newWeight - originalWeight;
            InventoryManager.instance.UpdateTotalWeight(-addedWeight); // ���� ����

            // �巡�׵� ������ ������ �ʱ�ȭ
            draggedSlot.ClearSlot();
        }
        else
        {
            // �⺻������ ���� ���� ó��
            ChangeSlot();
        }
    }
}
