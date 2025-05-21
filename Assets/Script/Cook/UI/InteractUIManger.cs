using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InteractUIManger : MonoBehaviour
{
    static public InteractUIManger instance;

    [SerializeField] GameObject interactUI;
    [SerializeField] GameObject interactTextUI;
    [SerializeField] TextMeshProUGUI interactText;

    public enum TextType { Open, Close, Close2, Ingredient, UnlockRecipe, Sleep, FailMenu }

    private string[] textList = {
        "���� 6�ú��� �Ĵ��� �� �� �ֽ��ϴ�.",
        "���� 10�ú��� �Ĵ��� ���� �� �ֽ��ϴ�.",
        "���� 12�ð� �Ǿ� �Ĵ� ���� �ݽ��ϴ�.",
        "��� ��ᰡ �����Ǿ� ���� �ݽ��ϴ�",
        "�����ǰ� �رݵǾ����ϴ�.",
        "5�ð��� �������ϴ�.",
        "������ �����Ͽ� ���� �� ����\n ���𰡸� ��������ϴ�.",
    };

    public static bool isUseInteractObject;
    public static bool isPlayerNearby;

    public void UsingText(TextType textType)
    {
        interactTextUI.SetActive(true);
        interactText.text = textList[(int)textType];
        StartCoroutine(TextFade());
    }

    IEnumerator TextFade()
    {
        yield return new WaitForSeconds(3f);
        interactTextUI.SetActive(false);
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    void Start()
    {
        isPlayerNearby = false;
        isUseInteractObject = false;
    }

    void Update()
    {
        if (isUseInteractObject) HideInteractUI();
        else if (!isPlayerNearby) HideInteractUI();
        else if (isPlayerNearby) VisibleInteractUI();
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
