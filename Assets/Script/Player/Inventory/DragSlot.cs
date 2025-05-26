using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    // 이 DragSlot UI 이미지 오브젝트를 담는다. 즉, 자기 자신을 담는다.
    static public DragSlot instance;

    // 드래그 대상이 되는 Slot을 참조 드래그 대상이 되는 Slot의 Sprite이미지가 복사되어 들어갈 것
    public Slot dragSlot;

    [SerializeField]
    private Image imageItem; // 자기 자신의 이미지 컴포넌트

    void Start()
    {
        instance = this;        
    }

    // 드래그 되는 슬롯의 이미지가 들어옴. 자기 자신의 이미지에 인수로 들어온 Sprite 이미지 할당
    public void DragSetImage(Image _itemImage)
    {
        SoundManager.instance.PlaySound(SoundManager.Display.Move_Item);
        gameObject.SetActive(true);
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    // 투명도 
    public void SetColor(int _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }

}
