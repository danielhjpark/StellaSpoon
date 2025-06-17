using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public static bool OptionActivated = false; // �ɼ� ǥ�� ����

    public GameObject go_Option; //�ɼ� ������Ʈ

    [SerializeField]
    private GameObject soundBase; // ���� ���̽�
    [SerializeField]
    private GameObject keyBase; // ����Ű ���̽�

    private bool SoundActivated = false;
    private bool KeyActivated = false;

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
            uiButton.button.onClick.AddListener(() => SoundManager.instance.PlaySound(SoundManager.Display.Button));
        }
    }
    private void Update()
    {
        if (DeviceManager.isDeactived)
        {
            SetDefalutSprite();
        }
    }

    public void OpenOption()
    {
        if (OptionActivated) return;
        SoundManager.instance.PlaySound(SoundManager.Display.Display_Menu_Button);
        go_Option.SetActive(true);
        soundBase.SetActive(true);
        keyBase.SetActive(false);
        OptionActivated = true;
    }

    public void CloseOption()
    {
        go_Option.SetActive(false);
        OptionActivated = false;
    }

    public void OpenSoundBase()
    {
        if (SoundActivated) return;

        soundBase.SetActive(true);
        keyBase.SetActive(false);
        SoundActivated = true;
    }

    public void CloseSoundBase()
    {
        soundBase.SetActive(false);
        SoundActivated = false;
    }

    public void OpenKeyBase()
    {
        if (KeyActivated) return;

        keyBase.SetActive(true);
        soundBase.SetActive(false);
        KeyActivated = true;
    }

    public void CloseKeyBase()
    {
        keyBase.SetActive(false);
        KeyActivated= false;
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

    private void SetDefalutSprite()
    {
        foreach (UIButton uiButton in uiButtons)
        {
            uiButton.buttonImage.sprite = uiButton.defaultSprite;
        }
    }

    public void SetSelectSprite()
    {
        OnButtonClicked(uiButtons[0]);
    }
}
