using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chatUI;

    public void CallChatUI() //대화창 ON
    {
        chatUI.SetActive(true);
    }
    public void CloseChatUI() //대화창 OFF
    {
        chatUI.SetActive(false);
    }

    [SerializeField]
    private GameObject gunImage; //image는resurces 폴더 Gun에 넣어야함.
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
        //gunImage 변경 필요
        gunName.text = "Tempest Fang";
        //gunName 변경 필요
        gunStats.text = "Damage: 20\nFire Rate: 0.2\nRange: 100";
        //gunStats 변경 필요

        //재료들 표시해야함. 어떻게 해야함? 다른총 선택하면 갯수가 줄거나 늘어나는데?
        //재료들 count 표시해야함. 0/갯수

    }

    public void SelectInfernoLance()
    {
        gunImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Gun/InfernoLance");
        //gunImage 변경 필요
        gunName.text = "Inferno Lance";
        //gunName 변경 필요
        gunStats.text = "Damage: 30\nFire Rate: 0.1\nRange: 150";
        //gunStats 변경 필요
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
