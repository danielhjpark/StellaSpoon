using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreButtonImageChanger : MonoBehaviour
{
    [System.Serializable]
    public class StoreUIButton
    {
        public Button storeButton;
        public Image storeButtonImage;
        public Sprite storeDefaultSprite;
        public Sprite storeSelectedSprite;
    }

    public StoreUIButton[] storeUIButtons;
    void Start()
    {
        // ��ư�� Ŭ�� �̺�Ʈ �߰�
        foreach (StoreUIButton uiButton in storeUIButtons)
        {
            uiButton.storeButton.onClick.AddListener(() => OnButtonClicked(uiButton));
        }
    }

    void OnButtonClicked(StoreUIButton clickedButton)
    {
        foreach (StoreUIButton uiButton in storeUIButtons)
        {
            // ������ ��ư�̸� UI Ȱ��ȭ & ��������Ʈ ����
            bool isSelected = (uiButton == clickedButton);
            uiButton.storeButtonImage.sprite = isSelected ? uiButton.storeSelectedSprite : uiButton.storeDefaultSprite;
        }
    }
    private void SetDefalutSprite()
    {
        foreach (StoreUIButton uiButton in storeUIButtons)
        {
            uiButton.storeButtonImage.sprite = uiButton.storeDefaultSprite;
        }
    }
}
