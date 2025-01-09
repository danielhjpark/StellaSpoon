using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureChestSlot : Slot
{
    override public void AddItem(Item _item, int _count = 1)
    {
        // ���Կ� �߰��� ������ ���� ����
        item = _item;
        itemCount = _count;

        // ������ �̹����� ���Կ� ǥ��
        itemImage.sprite = item.itemImage;

        // �������� ��� Ÿ���� �ƴ� ���, ������ ������ �ؽ�Ʈ�� ǥ��
        if (item.itemType != Item.ItemType.Equipment)
        {
            text_count.text = itemCount.ToString();
            text_count.gameObject.SetActive(true);
        }
        else
        {
            // ��� Ÿ���� ���, ������ �˷��ִ� �ؽ�Ʈ�� ��Ȱ��ȭ
            text_count.gameObject.SetActive(false);
        }

        // ������ ���� �������� Ȱ��ȭ ǥ��
        SetColor(1);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ������ �����ϰ�, �ش� ������ �κ��丮 ���� ���� ��
        if (DragSlot.instance.dragSlot != null)
        {
            // �κ��丮 �������� �̵� �ÿ��� ���� ������Ʈ
            if (DragSlot.instance.dragSlot.GetComponent<Slot>().GetType() == typeof(Slot)) // InventorySlot���θ� �̵�
            {
                // �κ��丮 �������� �������� �̵���ų �� ���� ������Ʈ
                InventoryManager.instance.UpdateTotalWeight(item.itemWeight * itemCount);
            }

            // �������� �κ��丮�� �̵��ϴ� ���, �������ڿ��� �������� ����
            ClearSlot();
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
    }
    // ������ �ʱ�ȭ�ϴ� �Լ� (������ ����)
    public override void ClearSlot()
    {
        // ������ ���� ������Ʈ�� ���� ����
        if (item != null)
        {
            // �������� ���Կ����� ���Ը� ������Ʈ���� ����
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
}
