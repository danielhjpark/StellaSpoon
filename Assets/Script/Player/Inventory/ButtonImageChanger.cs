using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageChanger : MonoBehaviour
{
    [System.Serializable]
    public class UIButton
    {
        public Button button;
        public Image buttonImage;
        public Sprite defaultSprite;
        public Sprite selectedSprite;
    }

    public UIButton[] uiButtons; // 4���� ��ư ����Ʈ

    void Start()
    {
        // ��ư�� Ŭ�� �̺�Ʈ �߰�
        foreach (UIButton uiButton in uiButtons)
        {
            uiButton.button.onClick.AddListener(() => OnButtonClicked(uiButton));
        }
    }

    void OnButtonClicked(UIButton clickedButton)
    {
        foreach (UIButton uiButton in uiButtons)
        {
            // ������ ��ư�̸� UI Ȱ��ȭ & ��������Ʈ ����
            bool isSelected = (uiButton == clickedButton);
            uiButton.buttonImage.sprite = isSelected ? uiButton.selectedSprite : uiButton.defaultSprite;
        }
    }
}

