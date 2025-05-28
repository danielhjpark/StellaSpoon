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
        // 버튼에 클릭 이벤트 추가
        foreach (StoreUIButton uiButton in storeUIButtons)
        {
            uiButton.storeButton.onClick.AddListener(() => OnButtonClicked(uiButton));
        }
    }

    void OnButtonClicked(StoreUIButton clickedButton)
    {
        foreach (StoreUIButton uiButton in storeUIButtons)
        {
            // 선택한 버튼이면 UI 활성화 & 스프라이트 변경
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
