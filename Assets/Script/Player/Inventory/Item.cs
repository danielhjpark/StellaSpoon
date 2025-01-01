using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType // ������ ����
    {
        Equipment, // ���
        Used, // �Һ� ������
        contaminatedIngredient, // ������ ��� ������
        Ingredient, // ��� ������
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

    //public string weaponType; // ���� ���� ����
}
