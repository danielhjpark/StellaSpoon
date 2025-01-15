using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; // ȹ���� ������
    public int itemCount; // ȹ���� �������� ����
    public Image itemImage; // �������� �̹���

    [SerializeField]
    protected Text text_count;

    private InputNumber theInputNumber;
    public RectTransform deleteImageRect;

    public static bool isFull = false;

    void Start()
    {
        // "deleteImage" �̸��� ���� GameObject�� ã�� RectTransform ��������
        GameObject deleteImage = GameObject.Find("DeleteImage");
        if (deleteImage != null)
        {
            deleteImageRect = deleteImage.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("DeleteImage�� ã�� �� �����ϴ�. �̸��� Ȯ���ϼ���.");
        }
        theInputNumber = FindObjectOfType<InputNumber>();
    }

    // ������ �̹����� ���� ����
    protected void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // �������� ���Կ� �߰��ϴ� �Լ�
    // _item: �߰��� ������ ��ü
    // _count: �߰��� ������ ���� (�⺻���� 1)
    virtual public void AddItem(Item _item, int _count = 1)
    {
        if (isFull)
        {
            Debug.Log("�� á��.");
            return;
        }

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


        // ������ ���� ������Ʈ
        InventoryManager.instance.UpdateTotalWeight(item.itemWeight * _count);

    }

    // ���Կ� ǥ�õ� ������ ������ ������Ʈ�ϴ� �Լ�
    // _count: ����(���) �Ǵ� ����(����)�� ������ ����
    public void SetSlotCount(int _count)
    {
        int previousCount = itemCount;
        // ���� ������ ������ ������Ʈ
        itemCount += _count;
        text_count.text = itemCount.ToString();
        text_count.gameObject.SetActive(true);

        int weightChange = item.itemWeight * (itemCount - previousCount);
        InventoryManager.instance.UpdateTotalWeight(weightChange);

        // ������ ������ 0 ������ ��� ������ �ʱ�ȭ
        if (itemCount <= 0)
        {
            ClearSlot();
        }


    }

    // ������ �ʱ�ȭ�ϴ� �Լ� (������ ����)
    // ������ ������ �����͸� ��� ���� UI ��� �ʱ�ȭ
    virtual public void ClearSlot()
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

    // ���콺 �巡�װ� ���� ���� �� �߻��ϴ� �̺�Ʈ
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isFull)
        {
            Debug.Log("�� á��.");
            return;
        }

        if (item != null) // �������� �ִ� �����̶�� �巡�� ���Կ� �ڱ� �ڽ��� �Ҵ��Ѵ�.
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // ���콺 �巡�� ���� �� ��� �߻��ϴ� �̺�Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null) // �������� �ִ� �����̶�� �巡�� ������ ��ġ�� �巡�װ� �߻��� ��ġ�� ���� �����δ�.
            DragSlot.instance.transform.position = eventData.position;
    }

    // ���콺 �巡�װ� ������ �� �߻��ϴ� �̺�Ʈ
    virtual public void OnEndDrag(PointerEventData eventData) // �巡�� ���� �ٽ� �ʱ�ȭ
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(deleteImageRect, Input.mousePosition, eventData.pressEventCamera))
        {
            if(DragSlot.instance.dragSlot != null)
            {
                theInputNumber.Call();
            }
        }
        else
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
    }

    // �ش� ���Կ� ���𰡰� ���콺 ��� ���� �� �߻��ϴ� �̺�Ʈ
    virtual public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }
    // A ������ �巡�� �Ͽ� B ���Կ� ����Ͽ�, A ���� B ���� ���� �ڸ��� �ٲٱ�
    virtual public void ChangeSlot()
    {
        if (isFull)
        {
            Debug.Log("�� á��.");
            return;
        }

        bool isWeaponSlot = this is WeaponSlot;
        bool isDraggedFromWeaponSlot = DragSlot.instance.dragSlot is WeaponSlot;

        // �� �������� �̵��� ��� ���
        if (item == null || DragSlot.instance.dragSlot.item == null)
        {
            SwapItems();
            return;
        }

        // ���Գ����� ��ȯ�� Ÿ�� ���� ���� ���
        if (!isWeaponSlot && !isDraggedFromWeaponSlot)
        {
            SwapItems();
            return;
        }

        // WeaponSlot�� InventorySlot ���� Weapon ������ ��ȯ�� ���
        if ((isWeaponSlot || isDraggedFromWeaponSlot) &&
            item.itemType == Item.ItemType.Equipment &&
            DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
        {
            SwapItems();
            return;
        }

        // ���� ���ǿ� �������� �ʴ� ���
        Debug.LogWarning("������ Ÿ���� ȣȯ���� �ʾ� ������ ��ȯ�� �� �����ϴ�.");

        // �巡�� �̹��� �ʱ�ȭ
        DragSlot.instance.SetColor(0);

        // �巡�� ���� ���� ����
        DragSlot.instance.dragSlot.AddItemWithoutWeight(
            DragSlot.instance.dragSlot.item,
            DragSlot.instance.dragSlot.itemCount
        );
    }

    private void SwapItems()
    {
        // ���� ������ �Ͻ������� ��Ȱ��ȭ
        InventoryManager.instance.PauseWeightUpdate();

        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItemWithoutWeight(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
            DragSlot.instance.dragSlot.AddItemWithoutWeight(_tempItem, _tempItemCount);
        else
            DragSlot.instance.dragSlot.ClearSlot();

        InventoryManager.instance.ResumeWeightUpdate();
    }



    // ������ ��ȯ�� �޼���: ���� ������Ʈ ���� ������ �߰�
    public void AddItemWithoutWeight(Item _item, int _count)
    {
        item = _item;
        itemCount = _count;

        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            text_count.text = itemCount.ToString();
            text_count.gameObject.SetActive(true);
        }
        else
        {
            text_count.gameObject.SetActive(false);
        }

        SetColor(1);
    }

}
