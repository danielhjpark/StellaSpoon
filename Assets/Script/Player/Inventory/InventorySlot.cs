using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : Slot
{
    //private int inventoryItemWeightTotal = 0;

    public override void OnDrop(PointerEventData eventData)
    {

        if (DragSlot.instance.dragSlot != null)
        {
            Slot draggedSlot = DragSlot.instance.dragSlot;

            if (draggedSlot is TreasureChestSlot || draggedSlot is InventorySlot)
            {
                HandleMerge(draggedSlot); // ���� ���� ȣ��
            }
            else
            {
                ChangeSlot(); // �⺻ ���� ���� ó��
            }
        }
    }

    public override void ClearSlot()
    {
        if (item != null)
        {
            // ���� ������Ʈ (���� ������ ���� * ������ ����)
            InventoryManager.instance.UpdateTotalWeight(-item.itemWeight * itemCount);
        }

        // ������ ������ �ʱ�ȭ
        item = null;
        itemCount = 0;

        // ������ �̹����� �ؽ�Ʈ�� �ʱ�ȭ
        itemImage.sprite = null;
        text_count.gameObject.SetActive(false);

        // ������ ��Ȱ��ȭ �������� ǥ��
        SetColor(0);
    }

    private void HandleMerge(Slot draggedSlot)
    {
        // ���� ���������� Ȯ��
        if (draggedSlot.item != null && this.item != null && draggedSlot.item.itemName == this.item.itemName)
        {
            // ���� ����: ���� ������ 20�� �̸��� ���� ����
            if (this.itemCount < 20 && draggedSlot.itemCount < 20)
            {
                // ���� �� ���� ���� ���
                int originalWeight = this.itemCount * this.item.itemWeight;

                // ����: ������ ���� ��ġ��
                this.SetSlotCount(draggedSlot.itemCount);

                if (draggedSlot is InventorySlot == false)
                {
                    // ���� �� ���� ���� ���
                    int newWeight = this.itemCount * this.item.itemWeight;

                    // �߰��� ���Ը�ŭ ������Ʈ
                    int addedWeight = newWeight - originalWeight;
                    InventoryManager.instance.UpdateTotalWeight(addedWeight);
                }

                // �巡�׵� ������ ������ �ʱ�ȭ
                draggedSlot.ClearSlot();
            }
            else
            {
                // ���� ���ǿ� �ش����� ������ �ܼ� ��ȯ
                ChangeSlot();
            }
        }
        else
        {
            // �⺻������ ���� ���� ó��
            ChangeSlot();
        }
    }


}
