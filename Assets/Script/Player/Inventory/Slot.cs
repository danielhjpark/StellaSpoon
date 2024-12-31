using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지

    [SerializeField]
    private Text text_count;

    // 아이템 이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템을 슬롯에 추가하는 함수
    // _item: 추가할 아이템 객체
    // _count: 추가할 아이템 개수 (기본값은 1)
    public void AddItem(Item _item, int _count = 1)
    {
        // 슬롯에 추가할 아이템 정보 저장
        item = _item;
        itemCount = _count;

        // 아이템 이미지를 슬롯에 표시
        itemImage.sprite = item.itemImage;

        // 아이템이 장비 타입이 아닌 경우, 아이템 개수를 텍스트로 표시
        if (item.itemType != Item.ItemType.Equipment)
        {
            text_count.text = itemCount.ToString();
        }
        else
        {
            // 장비 타입인 경우, 갯수를 알려주는 텍스트를 비활성화
            text_count.gameObject.SetActive(false);
        }

        // 슬롯의 색상 불투명도로 활성화 표시
        SetColor(1);
    }

    // 슬롯에 표시된 아이템 개수를 업데이트하는 함수
    // _count: 증가(양수) 또는 감소(음수)할 아이템 개수
    public void SetSlotCount(int _count)
    {
        // 현재 아이템 개수를 업데이트
        itemCount += _count;
        text_count.text = itemCount.ToString();

        // 아이템 개수가 0 이하일 경우 슬롯을 초기화
        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // 슬롯을 초기화하는 함수 (아이템 제거)
    // 슬롯의 아이템 데이터를 모두 비우고 UI 요소 초기화
    private void ClearSlot()
    {
        // 아이템 데이터 초기화
        item = null;
        itemCount = 0;

        // 슬롯의 이미지와 텍스트를 초기화
        itemImage.sprite = null;
        text_count.gameObject.SetActive(false);

        // 슬롯을 비활성화 색상으로 표시
        SetColor(0);
    }
}
