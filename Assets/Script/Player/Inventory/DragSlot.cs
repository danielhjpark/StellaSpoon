using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    // �� DragSlot UI �̹��� ������Ʈ�� ��´�. ��, �ڱ� �ڽ��� ��´�.
    static public DragSlot instance;

    // �巡�� ����� �Ǵ� Slot�� ���� �巡�� ����� �Ǵ� Slot�� Sprite�̹����� ����Ǿ� �� ��
    public Slot dragSlot;

    [SerializeField]
    private Image imageItem; // �ڱ� �ڽ��� �̹��� ������Ʈ

    void Start()
    {
        instance = this;        
    }

    // �巡�� �Ǵ� ������ �̹����� ����. �ڱ� �ڽ��� �̹����� �μ��� ���� Sprite �̹��� �Ҵ�
    public void DragSetImage(Image _itemImage)
    {
        SoundManager.instance.PlaySound(SoundManager.Display.Move_Item);
        gameObject.SetActive(true);
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    // ���� 
    public void SetColor(int _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }

}
