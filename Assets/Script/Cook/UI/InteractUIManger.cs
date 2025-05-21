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
        "오후 6시부터 식당을 열 수 있습니다.",
        "오후 10시부터 식당을 닫을 수 있습니다.",
        "오전 12시가 되어 식당 문을 닫습니다.",
        "모든 재료가 소진되어 문을 닫습니다",
        "레시피가 해금되었습니다.",
        "5시간이 지났습니다.",
        "조리에 실패하여 먹을 수 없는\n 무언가를 만들었습니다.",
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
