using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUIManger : MonoBehaviour
{
    [SerializeField] GameObject interactUI;
    [SerializeField] GameObject interactText;

    enum textType {Open, Close, Close2, Ingredient, UnlockRecipe, Sleep, FailMenu, LackGold, LackWeapon}
    private string[] textList = {
        "���� 6�ú��� �Ĵ��� �� �� �ֽ��ϴ�.",
        "���� 10�ú��� �Ĵ��� ���� �� �ֽ��ϴ�.",
        "���� 12�ð� �Ǿ� �Ĵ� ���� �ݽ��ϴ�.",
        "��� ��ᰡ �����Ǿ� ���� �ݽ��ϴ�",
        "�����ǰ� �رݵǾ����ϴ�.",
        "������ �����Ͽ� ���� �� ���� ���𰡸� ��������ϴ�.",
        "�ݾ��� �����մϴ�",
        "��ᰡ �����մϴ�"
    }; 

    public static bool isUseInteractObject;
    public static bool isPlayerNearby;

    void Start()
    {
        isPlayerNearby = false;
        isUseInteractObject = false;
    }

    void Update() {
        if(isUseInteractObject) HideInteractUI();
        else if(!isPlayerNearby) HideInteractUI();
        else if(isPlayerNearby) VisibleInteractUI();
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
