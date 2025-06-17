using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunButtonClick : MonoBehaviour
{
    public enum ButtonType
    {
        AlwaysPlay,     // 1�� ��ư
        TempestFang,    // 2�� ��ư
        InfernoLance    // 3�� ��ư
    }

    public ButtonType buttonType;

    private Button button;

    void Awake()
    {
        // Button ������Ʈ�� ������
        button = GetComponent<Button>();

        if (button != null)
        {
            // Ŭ�� �̺�Ʈ�� �޼��� ���
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button ������Ʈ�� ã�� �� �����ϴ�.");
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
