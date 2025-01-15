using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지

    [SerializeField]
    protected Text text_count;

    private InputNumber theInputNumber;
    public RectTransform deleteImageRect;

    public static bool isFull = false;

    void Start()
    {
        // "deleteImage" 이름을 가진 GameObject를 찾아 RectTransform 가져오기
        GameObject deleteImage = GameObject.Find("DeleteImage");
        if (deleteImage != null)
        {
            deleteImageRect = deleteImage.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("DeleteImage를 찾을 수 없습니다. 이름을 확인하세요.");
        }
        theInputNumber = FindObjectOfType<InputNumber>();
    }

    // 아이템 이미지의 투명도 조절
    protected void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템을 슬롯에 추가하는 함수
    // _item: 추가할 아이템 객체
    // _count: 추가할 아이템 개수 (기본값은 1)
    virtual public void AddItem(Item _item, int _count = 1)
    {
        if (isFull)
        {
            Debug.Log("꽉 찼다.");
            return;
        }

        // 슬롯에 추가할 아이템 정보 저장
        item = _item;
        itemCount = _count;

        // 아이템 이미지를 슬롯에 표시
        itemImage.sprite = item.itemImage;

        // 아이템이 장비 타입이 아닌 경우, 아이템 개수를 텍스트로 표시
        if (item.itemType != Item.ItemType.Equipment)
        {
            text_count.text = itemCount.ToString();
            text_count.gameObject.SetActive(true);
        }
        else
        {
            // 장비 타입인 경우, 갯수를 알려주는 텍스트를 비활성화
            text_count.gameObject.SetActive(false);
        }

        // 슬롯의 색상 불투명도로 활성화 표시
        SetColor(1);


        // 아이템 무게 업데이트
        InventoryManager.instance.UpdateTotalWeight(item.itemWeight * _count);

    }

    // 슬롯에 표시된 아이템 개수를 업데이트하는 함수
    // _count: 증가(양수) 또는 감소(음수)할 아이템 개수
    public void SetSlotCount(int _count)
    {
        int previousCount = itemCount;
        // 현재 아이템 개수를 업데이트
        itemCount += _count;
        text_count.text = itemCount.ToString();
        text_count.gameObject.SetActive(true);

        int weightChange = item.itemWeight * (itemCount - previousCount);
        InventoryManager.instance.UpdateTotalWeight(weightChange);

        // 아이템 개수가 0 이하일 경우 슬롯을 초기화
        if (itemCount <= 0)
        {
            ClearSlot();
        }


    }

    // 슬롯을 초기화하는 함수 (아이템 제거)
    // 슬롯의 아이템 데이터를 모두 비우고 UI 요소 초기화
    virtual public void ClearSlot()
    {

        if (item != null)
        {
            // 무게 업데이트 (현재 아이템 무게 * 아이템 개수)
            InventoryManager.instance.UpdateTotalWeight(-item.itemWeight * itemCount);
        }

        // 아이템 데이터 초기화
        item = null;
        itemCount = 0;

        // 슬롯의 이미지와 텍스트를 초기화
        itemImage.sprite = null;
        text_count.gameObject.SetActive(false);

        // 슬롯을 비활성화 색상으로 표시
        SetColor(0);

  
    }

    // 마우스 드래그가 시작 됐을 때 발생하는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isFull)
        {
            Debug.Log("꽉 찼다.");
            return;
        }

        if (item != null) // 아이템이 있는 슬롯이라면 드래그 슬롯에 자기 자신을 할당한다.
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null) // 아이템이 있는 슬롯이라면 드래그 슬롯의 위치를 드래그가 발생한 위치로 따라 움직인다.
            DragSlot.instance.transform.position = eventData.position;
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    virtual public void OnEndDrag(PointerEventData eventData) // 드래그 슬롯 다시 초기화
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

    // 해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
    virtual public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }
    // A 슬롯을 드래그 하여 B 슬롯에 드롭하여, A 슬롯 B 슬롯 서로 자리를 바꾸기
    virtual public void ChangeSlot()
    {
        if (isFull)
        {
            Debug.Log("꽉 찼다.");
            return;
        }

        bool isWeaponSlot = this is WeaponSlot;
        bool isDraggedFromWeaponSlot = DragSlot.instance.dragSlot is WeaponSlot;

        // 빈 슬롯으로 이동할 경우 허용
        if (item == null || DragSlot.instance.dragSlot.item == null)
        {
            SwapItems();
            return;
        }

        // 슬롯끼리의 교환은 타입 제한 없이 허용
        if (!isWeaponSlot && !isDraggedFromWeaponSlot)
        {
            SwapItems();
            return;
        }

        // WeaponSlot과 InventorySlot 간의 Weapon 아이템 교환을 허용
        if ((isWeaponSlot || isDraggedFromWeaponSlot) &&
            item.itemType == Item.ItemType.Equipment &&
            DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
        {
            SwapItems();
            return;
        }

        // 제한 조건에 부합하지 않는 경우
        Debug.LogWarning("아이템 타입이 호환되지 않아 슬롯을 교환할 수 없습니다.");

        // 드래그 이미지 초기화
        DragSlot.instance.SetColor(0);

        // 드래그 슬롯 상태 복구
        DragSlot.instance.dragSlot.AddItemWithoutWeight(
            DragSlot.instance.dragSlot.item,
            DragSlot.instance.dragSlot.itemCount
        );
    }

    private void SwapItems()
    {
        // 무게 변경을 일시적으로 비활성화
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



    // 아이템 교환용 메서드: 무게 업데이트 없이 아이템 추가
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
