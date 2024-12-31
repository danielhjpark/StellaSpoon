using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item; // ȹ���� ������
    public int itemCount; // ȹ���� �������� ����
    public Image itemImage; // �������� �̹���

    [SerializeField]
    private Text text_count;

    // ������ �̹����� ���� ����
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // �������� ���Կ� �߰��ϴ� �Լ�
    // _item: �߰��� ������ ��ü
    // _count: �߰��� ������ ���� (�⺻���� 1)
    public void AddItem(Item _item, int _count = 1)
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
        }
        else
        {
            // ��� Ÿ���� ���, ������ �˷��ִ� �ؽ�Ʈ�� ��Ȱ��ȭ
            text_count.gameObject.SetActive(false);
        }

        // ������ ���� �������� Ȱ��ȭ ǥ��
        SetColor(1);
    }

    // ���Կ� ǥ�õ� ������ ������ ������Ʈ�ϴ� �Լ�
    // _count: ����(���) �Ǵ� ����(����)�� ������ ����
    public void SetSlotCount(int _count)
    {
        // ���� ������ ������ ������Ʈ
        itemCount += _count;
        text_count.text = itemCount.ToString();

        // ������ ������ 0 ������ ��� ������ �ʱ�ȭ
        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // ������ �ʱ�ȭ�ϴ� �Լ� (������ ����)
    // ������ ������ �����͸� ��� ���� UI ��� �ʱ�ȭ
    private void ClearSlot()
    {
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
