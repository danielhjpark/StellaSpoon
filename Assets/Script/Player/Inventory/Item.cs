using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType // ������ ����
    {
        Equipment, // ���
        Used, // �Һ� ������
        contaminatedIngredient, // ������ ��� ������
        Ingredient, // ��� ������
        Recipe,// ������
        ETC, // ��Ÿ ������
    }

    [Header("������ �̸�")]
    public string itemName; // �������� �̸�
    [Header("������ ����")]
    public ItemType itemType; // �������� ����
    [Header("�������� �̹���(�κ��丮 �ȿ����� �̹���)")]
    public Sprite itemImage; // �������� �̹���(�κ��丮 �ȿ����� �̹���)
    [Header("�������� ������")]
    public GameObject itemPrefab; // �������� ������ (������ ���� �� ���������� ����.)
    [Header("�������� ����")]
    public int itemWeight; // ������ ����
    [Header("�������� ����")]
    public int itemBuyPrice; // ������ ����
    public int itemSellPrice; // ������ �Ǹ� ����
    [Header("������ �������� �ѱ� ��")]
    public string descItemName;
}
