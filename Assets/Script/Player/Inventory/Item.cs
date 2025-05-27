using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType // 아이템 유형
    {
        Equipment, // 장비
        Used, // 소비 아이템
        contaminatedIngredient, // 오염된 재료 아이템
        Ingredient, // 재료 아이템
        Recipe,// 레시피
        ETC, // 기타 아이템
    }

    [Header("아이템 이름")]
    public string itemName; // 아이템의 이름
    [Header("아이템 유형")]
    public ItemType itemType; // 아이템의 유형
    [Header("아이템의 이미지(인벤토리 안에서의 이미지)")]
    public Sprite itemImage; // 아이템의 이미지(인벤토리 안에서의 이미지)
    [Header("아이템의 프리팹")]
    public GameObject itemPrefab; // 아이템의 프리팹 (아이템 생성 시 프리팹으로 찍어낸다.)
    [Header("아이템의 무게")]
    public int itemWeight; // 아이템 무게
    [Header("아이템의 가격")]
    public int itemBuyPrice; // 아이템 가격
    public int itemSellPrice; // 아이템 판매 가격
    [Header("설명할 아이템의 한글 명")]
    public string descItemName;
}
