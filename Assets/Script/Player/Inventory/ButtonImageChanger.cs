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

    public UIButton[] uiButtons; // 4개의 버튼 리스트


    private void Update()
    {
        if(DeviceManager.isDeactived)
        {
            SetDefalutSprite();
        }
    }


    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        foreach (UIButton uiButton in uiButtons)
        {
            uiButton.button.onClick.AddListener(() => OnButtonClicked(uiButton));
        }
    }

    void OnButtonClicked(UIButton clickedButton)
    {
        foreach (UIButton uiButton in uiButtons)
        {
            // 선택한 버튼이면 UI 활성화 & 스프라이트 변경
            bool isSelected = (uiButton == clickedButton);
            uiButton.buttonImage.sprite = isSelected ? uiButton.selectedSprite : uiButton.defaultSprite;
        }
    }

    private void SetDefalutSprite()
    {
        foreach(UIButton uiButton in uiButtons)
        {
            uiButton.buttonImage.sprite = uiButton.defaultSprite;
        }
    }
}

