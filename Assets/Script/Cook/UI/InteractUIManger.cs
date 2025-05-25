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

    [SerializeField] GameObject interactUI;
    [Header("Popup")]
    [SerializeField] Transform popupContainer;
    [SerializeField] GameObject warningTextPrefab;
    [SerializeField] GameObject noticeTextPrefab;
    public AudioSource interactAudioSource;
    public enum TextType { Open, Open2, Open3, Close, Close2, Ingredient, UnlockRecipe, Sleep, FailMenu, ClearMenu }
    private Dictionary<TextType, bool> checkUseText;

    private string[] textList = {
        "���� 6�ú��� �Ĵ��� �� �� �ֽ��ϴ�.",
        "���ϸ� �޴��� �����ؾ� �մϴ�",
        "���� 9�ð� ���� �Ĵ��� �� �� �����ϴ�.",
        "���� 10�ú��� �Ĵ��� ���� �� �ֽ��ϴ�.",
        "���� 12�ð� �Ǿ� �Ĵ� ���� �ݽ��ϴ�.",
        "��� ��ᰡ �����Ǿ� ���� �ݽ��ϴ�",
        "�����ǰ� �رݵǾ����ϴ�.",
        "5�ð��� �������ϴ�.",
        "������ �����Ͽ� ���� �� ����\n ���𰡸� ��������ϴ�.",
        "���� ���ø� ġ���� �մϴ�.",
    };
    public static bool isUseInteractObject;
    public static bool isPlayerNearby;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
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
        if (isUseInteractObject) HideInteractUI();
        else if (!isPlayerNearby) HideInteractUI();
        else if (isPlayerNearby) VisibleInteractUI();
    }

    public void UsingText(TextType textType)
    {
        if (checkUseText[textType])
        {
            return;
        }
        else
        {
            StartCoroutine(CheckUseText(textType));
            GameObject popupText;
            if (textType == TextType.Open || textType == TextType.Open2 ||textType == TextType.Open3||textType == TextType.Close || textType == TextType.ClearMenu)
            {
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

    IEnumerator CheckUseText(TextType textType)
    {
        checkUseText[textType] = true;
        yield return new WaitForSeconds(3f);
        checkUseText[textType] = false;
    }

    public void VisibleInteractUI()
    {
        interactUI.SetActive(true);
    }

    public void HideInteractUI()
    {
        interactUI.SetActive(false);
    }

}
