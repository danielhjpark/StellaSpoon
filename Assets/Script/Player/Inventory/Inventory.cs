using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // 인벤토리 활성화 여부. true가 되면 카메라 움직임 등 필요에 따라 입력 막기

    [SerializeField]
    private GameObject go_InventoryBase; // 인벤토리 이미지
    [SerializeField]
    private GameObject go_SlotsParent; // Slot들의 부모인 Grid Setting

    private Slot[] slots; // 슬롯들 배열

    [SerializeField]
    private DeviceManager deviceManager;

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    public void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
        inventoryActivated = true;
        deviceManager.uiPanel.SetActive(false);
    }

    public void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
        inventoryActivated = false;
        deviceManager.uiPanel.SetActive(true);
    }

    // 아이템을 획득하고 인벤토리 슬롯에 추가하는 함수
    // _item: 획득한 아이템 객체
    // _count: 획득한 아이템 개수 (기본값은 1)
    public void AcquireItem(Item _item, int _count = 1)
    {
        // 1. 아이템이 장비 타입이 아닌 경우, 기존 슬롯에서 같은 아이템을 찾아 추가
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for(int i = 0; i < slots.Length; i++)
            {
                // 슬롯에 이미 아이템이 있는 경우
                if (slots[i].item != null)
                {
                    // 슬롯의 아이템 이름이 획득한 아이템 이름과 같으면
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        // 해당 슬롯의 아이템 개수를 업데이트하고 함수 종료
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }
        // 2. 빈 슬롯을 찾아 아이템을 추가
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯이 비어 있는 경우
            if (slots[i].item == null)
            {
                // 해당 슬롯에 아이템과 개수를 추가하고 함수 종료
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
