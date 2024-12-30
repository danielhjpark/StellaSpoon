using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 인벤토리의 베이스로 인벤토리 슬롯들을 등록시키고 사용할 준비를 하는 스크립트
// 추상 클래스로 작성하여 인벤토리 베이스 자체적으로 인스턴스 불가하게 함.
abstract public class InventoryBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject mInventoryBase; // Inventory 최상위 부모(활성 / 비활성 목적)
    [SerializeField]
    protected GameObject mInventorySlotsParent; // Slot들을 담을 부모 게임오브젝트

    protected InventorySlot[] mSlots; // 인벤토리 슬롯 배열
    // 인벤토리 베이스를 초기화
    protected void Awake()
    {
        if(mInventoryBase.activeSelf)
        {
            mInventoryBase.SetActive(false);
        }

        mSlots = mInventorySlotsParent.GetComponentsInChildren<InventorySlot>();
    }
}
