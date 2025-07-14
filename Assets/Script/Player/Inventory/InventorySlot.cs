using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : Slot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            Slot draggedSlot = DragSlot.instance.dragSlot;

            // �ڱ� �ڽſ��� ����� ��� �ƹ� �������� ����
            if (draggedSlot == this)
            {
                DragSlot.instance.SetColor(0);
                DragSlot.instance.gameObject.SetActive(false);
                DragSlot.instance.dragSlot = null;
                return;
            }

            // ������ �������� ���
            if (draggedSlot.item != null && draggedSlot.item.itemType == Item.ItemType.Recipe)
            {
                string recipeName = draggedSlot.item.itemName;

                Recipe targetRecipe = RecipeManager.instance.FindRecipe(recipeName);
                if (targetRecipe != null)
                {
                    RecipeManager.instance.RecipeUnLock(targetRecipe);
                    Debug.Log($"[InventorySlot] '{recipeName}' ������ ��� �Ϸ�");
                }
                else
                {
                    Debug.LogWarning($"[InventorySlot] ������ '{recipeName}' ã�� �� ����");
                }

                // ������ ����
                draggedSlot.ClearSlot();
                return;
            }

            // ���� �Ǵ� ���� ����
            if (draggedSlot is TreasureChestSlot || draggedSlot is InventorySlot)
            {
                HandleMerge(draggedSlot);
            }
            else
            {
                ChangeSlot();
            }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }

    public override void ClearSlot()
    {
        if (item != null)
        {
            InventoryManager.instance.UpdateTotalWeight(-item.itemWeight * itemCount);
        }

        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        text_count.gameObject.SetActive(false);
        SetColor(0);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    private void HandleMerge(Slot draggedSlot)
    {
        // �ڱ� �ڽſ��� ���� �õ��� ��� ����
        if (draggedSlot == this)
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.gameObject.SetActive(false);
            DragSlot.instance.dragSlot = null;
            return;
        }

        // ���� ����: ���� ������ + 20 �̸�
        if (draggedSlot.item != null && this.item != null && draggedSlot.item.itemName == this.item.itemName)
        {
            if (this.itemCount < 20 && draggedSlot.itemCount < 20)
            {
                int originalWeight = this.itemCount * this.item.itemWeight;

                // ����: ������ ���� ��ġ��
                this.SetSlotCount(draggedSlot.itemCount);

                if (!(draggedSlot is InventorySlot)) // �ܺ� ���Կ��� ���� ��쿡�� ���� �߰�
                {
                    int newWeight = this.itemCount * this.item.itemWeight;
                    int addedWeight = newWeight - originalWeight;
                    InventoryManager.instance.UpdateTotalWeight(addedWeight);
                }

                // ���� ��� ���� �ʱ�ȭ
                draggedSlot.ClearSlot();
                return;
            }
        }

        // ���� ���� ������ �� �⺻ ���� ��ȯ
        ChangeSlot();
    }
}

