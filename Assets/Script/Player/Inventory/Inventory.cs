using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // 인벤토리 활성화 여부. true가 되면 카메라 움직임 등 필요에 따라 입력 막기

    [SerializeField]
    private GameObject go_InventoryBase; // 인벤토리 이미지
    [SerializeField]
    private GameObject go_EquipmentBase; // 장비 창 이미지
    [SerializeField]
    private GameObject go_SlotsParent; // Slot들의 부모인 Grid Setting

    private Slot[] slots; // 슬롯들 배열

    private ItemNameData itemNameData;

    public virtual Slot[] GetSlots()
    {
        if (slots == null || slots.Length == 0)
        {
            slots = go_SlotsParent.GetComponentsInChildren<Slot>(true);
        }
        return slots;
    }

    [SerializeField]
    private Item[] Inventoryitems;

    public virtual void LoadToInven(int _arrNum, string _itemName, int _itemCount)
    {
        for (int i = 0; i < Inventoryitems.Length; i++)
        {
            if (Inventoryitems[i].itemName == _itemName)
            {
                if (_arrNum >= 0 && _arrNum < slots.Length && slots[_arrNum] != null)
                {
                    slots[_arrNum].AddItem(Inventoryitems[i], _itemCount);
                    return;
                }
                else
                {
                    Debug.LogError($"Invalid slot index {_arrNum} or slot is null.");
                    return;
                }
            }
        }
        Debug.LogWarning($"아이템 '{_itemName}' 을 Inventoryitems에서 찾을 수 없습니다.");
    }
    private void Awake()
    {
        if (go_SlotsParent == null)
        {
            Debug.LogError("[Inventory] go_SlotsParent가 할당되지 않았습니다.");
        }
        else
        {
            slots = go_SlotsParent.GetComponentsInChildren<Slot>(true);
            Debug.Log($"[Inventory] 슬롯 {slots.Length}개 초기화 완료");
        }
    }
    void Start()
    {
        itemNameData = FindObjectOfType<ItemNameData>();
    }

    public void OpenInventory()
    {
        if (inventoryActivated) return;

        go_InventoryBase.SetActive(true);
        go_EquipmentBase.SetActive(true);
        inventoryActivated = true;
    }

    public void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
        go_EquipmentBase.SetActive(false);
        inventoryActivated = false;
        itemNameData.HideToolTip();
    }

    // 아이템을 획득하고 인벤토리 슬롯에 추가하는 함수
    // _item: 획득한 아이템 객체
    // _count: 획득한 아이템 개수 (기본값은 1)
    public void AcquireItem(Item _item, int _count = 1)
    {
        // 1. 아이템이 장비 타입이 아닌 경우, 기존 슬롯에서 같은 아이템을 찾아 추가
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                // 슬롯에 이미 아이템이 있는 경우
                if (slots[i].item != null)
                {
                    // 슬롯의 아이템 이름이 획득한 아이템 이름과 같으면
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        // 현재 슬롯의 남은 공간 계산
                        int spaceLeft = 20 - slots[i].itemCount;

                        if (_count <= spaceLeft)
                        {
                            // 슬롯에 남은 공간이 충분하면 아이템 개수를 추가하고 함수 종료
                            slots[i].SetSlotCount(_count);
                            return;
                        }
                        else
                        {
                            // 슬롯에 남은 공간이 부족하면 가능한 개수만 추가하고 초과분을 저장
                            slots[i].SetSlotCount(spaceLeft);
                            _count -= spaceLeft; // 초과된 개수
                        }
                    }
                }
            }
        }

        // 2. 남은 개수를 빈 슬롯에 추가
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯이 비어 있는 경우
            if (slots[i].item == null)
            {
                if (_count <= 20)
                {
                    // 남은 아이템이 20개 이하이면 한 슬롯에 모두 추가
                    slots[i].AddItem(_item, _count);
                    return;
                }
                else
                {
                    // 남은 아이템이 20개 초과이면 가능한 개수만 추가
                    slots[i].AddItem(_item, 20);
                    _count -= 20; // 초과분 저장
                }
            }
        }

        // 3. 아이템을 모두 추가하지 못한 경우
        if (_count > 0)
        {
            Debug.LogWarning("Not enough inventory space for all items.");
        }
    }
    public int GetItemCount(string itemName)
    {
        int totalCount = 0;

        foreach (Slot slot in slots)
        {
            if (slot.item != null && slot.item.itemName == itemName)
            {
                totalCount += slot.itemCount;
            }
        }

        return totalCount;
    }
    public bool DecreaseItemCount(string itemName, int count)
    {
        int remainingToRemove = count;

        // 인벤토리에서 뒤에서부터 (최근 추가된 슬롯부터) 제거
        for (int i = slots.Length - 1; i >= 0 && remainingToRemove > 0; i--)
        {
            if (slots[i].item != null && slots[i].item.itemName == itemName)
            {
                int removeCount = Mathf.Min(remainingToRemove, slots[i].itemCount);
                slots[i].SetSlotCount(-removeCount);
                remainingToRemove -= removeCount;
            }
        }

        if (remainingToRemove > 0)
        {
            Debug.LogWarning($"[Inventory] {itemName} 아이템을 {count}개 제거하려 했지만 {remainingToRemove}개 부족합니다.");
            return false;
        }

        return true;
    }
}
