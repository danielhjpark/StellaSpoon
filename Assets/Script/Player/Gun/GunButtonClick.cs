using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunButtonClick : MonoBehaviour
{
    public enum ButtonType
    {
        AlwaysPlay,     // 1번 버튼
        TempestFang,    // 2번 버튼
        InfernoLance    // 3번 버튼
    }

    public ButtonType buttonType;

    private Button button;

    void Awake()
    {
        // Button 컴포넌트를 가져옴
        button = GetComponent<Button>();

        if (button != null)
        {
            // 클릭 이벤트에 메서드 등록
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button 컴포넌트를 찾을 수 없습니다.");
        }
    }

    void OnButtonClick()
    {
        switch (buttonType)
        {
            case ButtonType.AlwaysPlay:
                SoundManager.instance.Play_GunChangeSound(SoundManager.Gun.Button);
                break;

            case ButtonType.TempestFang:
                if (RifleManager.instance.tempestFang)
                {
                    SoundManager.instance.Play_GunChangeSound(SoundManager.Gun.Button);
                }
                break;

            case ButtonType.InfernoLance:
                if (RifleManager.instance.infernoLance)
                {
                    SoundManager.instance.Play_GunChangeSound(SoundManager.Gun.Button);
                }
                break;
        }
    }
}
