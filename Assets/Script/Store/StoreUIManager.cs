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
    private GameObject gunImage; //image��resurces ���� Gun�� �־����.
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
    private TextMeshProUGUI Gold;

    public void SelectTempestFang()
    {
        gunImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Gun/TempestFang");
        //gunImage ���� �ʿ�
        gunName.text = "Tempest Fang";
        //gunName ���� �ʿ�
        gunStats.text = "Damage: 20\nFire Rate: 0.2\nRange: 100";
        //gunStats ���� �ʿ�

        //���� ǥ���ؾ���. ��� �ؾ���? �ٸ��� �����ϸ� ������ �ٰų� �þ�µ�?
        //���� count ǥ���ؾ���. 0/����

    }

    public void SelectInfernoLance()
    {
        gunImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Gun/InfernoLance");
        //gunImage ���� �ʿ�
        gunName.text = "Inferno Lance";
        //gunName ���� �ʿ�
        gunStats.text = "Damage: 30\nFire Rate: 0.1\nRange: 150";
        //gunStats ���� �ʿ�
    }

    private void Update()
    {
        updateGold();
    }

    public void updateGold()
    {
        Gold.text = Manager.Gold.ToString();
    }
}
