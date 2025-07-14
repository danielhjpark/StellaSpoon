using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class InteractUIManger : MonoBehaviour
{
    static public InteractUIManger instance;

    [Header("Interact")]
    //[SerializeField] GameObject interactUI;
    [SerializeField] InteractUI interactUI;
    [Header("Popup")]
    [SerializeField] Transform popupContainer;
    [SerializeField] GameObject warningTextPrefab;
    [SerializeField] GameObject noticeTextPrefab;

    public AudioSource interactAudioSource;

    public enum TextType { Open, Open2, Open3, Close, Close2, Ingredient, UnlockRecipe, Sleep, FailMenu, ClearMenu, FailAddMenu }
    private Dictionary<TextType, bool> checkUseText;
    private string[] textList = {
        "���� 6�ú��� �Ĵ��� �� �� �ֽ��ϴ�.",
        "���ϸ� �޴��� �����ؾ� �մϴ�",
        "���� 9�ð� ���� �Ĵ��� �� �� �����ϴ�.",
        "���� 10�ú��� �Ĵ��� ���� �� �ֽ��ϴ�.",
        "���� 12�ð� �Ǿ� �Ĵ� ���� �ݽ��ϴ�.",
        "��� ��ᰡ �����Ǿ� ���� �ݽ��ϴ�",
        "�����ǰ� �رݵǾ����ϴ�.",
        "4�ð��� �������ϴ�.",
        "������ �����Ͽ� ���� �� ����\n ���𰡸� ��������ϴ�.",
        "���� ���ø� ġ���� �մϴ�.",
        "��ᰡ �����մϴ�."
    };

    public static bool isUseInteractObject;
    public static bool isPlayerNearby;
    public static GameObject currentInteractObject;

    private Coroutine useTextCoroutine;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        interactAudioSource = SoundManager.instance.audioSfx;
        isPlayerNearby = false;
        isUseInteractObject = false;
        checkUseText = new Dictionary<TextType, bool>();
        foreach (TextType type in Enum.GetValues(typeof(TextType)))
        {
            checkUseText[type] = false;
        }
    }

    void OnDisable()
    {
        isUseInteractObject = false;
        isPlayerNearby = false;
    }

    void Update()
    {
        if (popupContainer == null)
        {
            Transform canvasTransform = GameObject.Find("Canvas")?.gameObject.transform; // Canvas�� ã��
            popupContainer = canvasTransform.Find("PopupContainer")?.gameObject.transform;
        }
        if (isUseInteractObject || !isPlayerNearby || !DeviceManager.isDeactived) HideInteractUI();
        else if (isPlayerNearby) VisibleInteractUI();
    }

    public void UsingText(TextType textType)
    {
        if (useTextCoroutine != null)
        {
            return;
        }
        else
        {
            useTextCoroutine = StartCoroutine(CheckUseText(textType));
            GameObject popupText;
            if (textType == TextType.Open || textType == TextType.Open2 || textType == TextType.Open3 ||
                textType == TextType.Close || textType == TextType.ClearMenu || textType == TextType.FailAddMenu)
            {
                SoundManager.instance.PlayPlayerSFX(SoundManager.EPlayerSfx.Error);
                popupText = Instantiate(warningTextPrefab, popupContainer);
            }
            else
            {
                popupText = Instantiate(noticeTextPrefab, popupContainer);
            }
            popupText.transform.SetAsLastSibling(); // �ؿ� ���̰�
            PopupUI popup = popupText.GetComponent<PopupUI>();
            popup.Setup(textList[(int)textType], 2.5f);
        }
    }

    public void UsingText(string text, bool isWarning)
    {
        if (useTextCoroutine != null)
        {
            return;
        }
        else
        {
            useTextCoroutine = StartCoroutine(CheckUseText());
            GameObject popupText;
            if (isWarning)
            {
                SoundManager.instance.PlayPlayerSFX(SoundManager.EPlayerSfx.Error);
                popupText = Instantiate(warningTextPrefab, popupContainer);
            }
            else
            {
                popupText = Instantiate(noticeTextPrefab, popupContainer);
            }
            popupText.transform.SetAsLastSibling(); // �ؿ� ���̰�
            PopupUI popup = popupText.GetComponent<PopupUI>();
            popup.Setup(text, 2.5f);
        }
    }

    IEnumerator CheckUseText(TextType textType)
    {
        checkUseText[textType] = true;
        yield return new WaitForSeconds(3f);
        checkUseText[textType] = false;
        useTextCoroutine = null;
    }

    IEnumerator CheckUseText()
    {
        yield return new WaitForSeconds(3f);
        useTextCoroutine = null;
    }

    public void VisibleInteractUI()
    {
        interactUI.UseInteractUI(currentInteractObject);
    }

    public void HideInteractUI()
    {
        interactUI.DisableInteractUI();
    }

}
