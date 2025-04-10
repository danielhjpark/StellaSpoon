using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chatUI;

    public void CallChatUI() //��ȭâ ON
    {
        chatUI.SetActive(true);
    }
    public void CloseChatUI() //��ȭâ OFF
    {
        chatUI.SetActive(false);
    }

    [SerializeField]
    private GameObject gunImage;
    [SerializeField]
    private TextMeshProUGUI gunName;
    [SerializeField]
    private TextMeshProUGUI gunStats;

    [SerializeField]
    private GameObject Ingredient01;
    [SerializeField]
    private TextMeshProUGUI Ingredient01Count;
    [SerializeField]
    private GameObject Ingredient02;
    [SerializeField]
    private TextMeshProUGUI Ingredient02Count;


    [SerializeField]
    private GameObject Gold;

    public void SelectTempestFang()
    {
        //gunImage ���� �ʿ�
        //gunName ���� �ʿ�
        //gunStats ���� �ʿ�
        //���� ǥ���ؾ���. ��� �ؾ���? �ٸ��� �����ϸ� ������ �ٰų� �þ�µ�?
        //���� count ǥ���ؾ���. 0/���� 

    }

    public void SelectInfernoLance()
    {

    }
}
