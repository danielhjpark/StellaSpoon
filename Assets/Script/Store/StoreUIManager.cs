using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class StoreUIManager : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

    [Header("ChatUI")]
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

    [Header("-----ingredient-----")]
    [SerializeField]
    private GameObject buyBase;
    [SerializeField]
    private GameObject sellBase;

    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private TextMeshProUGUI ingredientNeedGold;

    [SerializeField]
    private GameObject[] stage2_ingredientButtons; //재료 버튼들


    [Header("Ingredient List")]
    [SerializeField]
    private List<Item> items; //재료 정보들 리스트

    [SerializeField]
    private int currentPurchaseCount = 0; //현재 구매 갯수
    private int currentSelectedIngredientIndex = -1; //현재 선택된 재료 인덱스



    [Header("-----Gun-----")]
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
    private TextMeshProUGUI gunNeedGold;

    [SerializeField]
    private int gunType; //0: TempestFang, 1: InfernoLance


    [Header("Gun Create Cost")]
    [SerializeField]
    private int tempestFangNeedGold = 1500;
    [SerializeField]
    private int infernoLanceNeedGold = 2700;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        ResetIngredientPurchase();
        SelectTempestFang(); //처음 시작할 때 TempestFang 선택
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //씬 로드시 스테이지 1 클리어 여부에 따라 재료 버튼 활성화
        if (Manager.stage_01_clear)
        {
            for (int i = 0; i < stage2_ingredientButtons.Length; i++)
            {
                stage2_ingredientButtons[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < stage2_ingredientButtons.Length; i++)
            {
                stage2_ingredientButtons[i].SetActive(false);
            }

        }
    }
    //ingredient
    public void SelectBuy()
    {
        buyBase.SetActive(true);
        sellBase.SetActive(false);
    }
    public void SelectSell()
    {
        sellBase.SetActive(true);
        buyBase.SetActive(false);
    }

    public void PlusButton()
    {
        if(currentPurchaseCount < 99)
        {
            currentPurchaseCount++;
            countText.GetComponent<TextMeshProUGUI>().text = currentPurchaseCount.ToString();
            UpdateIngredientTotalCost();
        }
    }
    public void MinusButton()
    {
        if (currentPurchaseCount > 0)
        {
            currentPurchaseCount--;
            countText.GetComponent<TextMeshProUGUI>().text = currentPurchaseCount.ToString();
            UpdateIngredientTotalCost();
        }
    }

    public void SelectedIngredient(int index)
    {
        if(index < 0 || index >= items.Count)
        {
            Debug.Log("잘못된 인덱스");
            return;
        }

        currentSelectedIngredientIndex = index;
        Item selectedItem = items[index];
        Debug.Log(selectedItem.itemName + " 선택됨");

        ResetIngredientPurchase();
    }

    public void ResetIngredientPurchase()
    {
        currentPurchaseCount = 0;
        countText.text = currentPurchaseCount.ToString();
        if (ingredientNeedGold != null)
        {
            ingredientNeedGold.text = 0.ToString();
        }
    }

    private void UpdateIngredientTotalCost()
    {
        if (currentSelectedIngredientIndex < 0)
        {
            return;
        }
        int price = items[currentSelectedIngredientIndex].itemBuyPrice;
        int totalCost = price * currentPurchaseCount;
        if (ingredientNeedGold != null)
        {
            ingredientNeedGold.text  = totalCost.ToString();
        }
    }

    public void BuyIngredient()
    {
        if (currentSelectedIngredientIndex < 0)
        {
            Debug.Log("재료를 선택하세요");
            return;
        }
        Item selectedItem = items[currentSelectedIngredientIndex];
        int totalCost = selectedItem.itemBuyPrice * currentPurchaseCount;

        if (Manager.gold >= totalCost)
        {
            Manager.gold -= totalCost;
            Debug.Log(string.Format("{0} x {1} 구매 완료. 남은 Gold: {2}", selectedItem.itemName, currentPurchaseCount, Manager.gold));

            inventory.AcquireItem(selectedItem, currentPurchaseCount);
            //TODO: 인벤토리에 아이템 추가 필요

            Debug.Log("아이템 구매");
            ResetIngredientPurchase();
        }
        else
        {
            Debug.Log("골드 부족");
        }
    }


    //cook

    //gunNPC
    public void SelectTempestFang()
    {
        gunType = 0;
        gunImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Gun/TempestFang");
        //gunImage 변경 필요
        gunName.text = "Tempest Fang";
        //gunName 변경 필요
        gunStats.text = "Damage: 20\nFire Rate: 0.2\nRange: 100";
        //gunStats 변경 필요

        gunNeedGold.text = tempestFangNeedGold.ToString();

        //재료들 표시해야함. 어떻게 해야함? 다른총 선택하면 갯수가 줄거나 늘어나는데?
        //재료들 count 표시해야함. 0/갯수

    }

    public void SelectInfernoLance()
    {
        gunType = 1;
        gunImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Gun/InfernoLance");
        //gunImage 변경 필요
        gunName.text = "Inferno Lance";
        //gunName 변경 필요
        gunStats.text = "Damage: 30\nFire Rate: 0.1\nRange: 150";
        //gunStats 변경 필요

        gunNeedGold.text = infernoLanceNeedGold.ToString();
    }

    public void CreateGun()
    {
        switch (gunType)
        {
            case 0: //TempestFang
                if (Manager.gold >= tempestFangNeedGold)
                {
                    Manager.gold -= tempestFangNeedGold;
                    //총 생성
                    //재료들 소모
                    //UI 갱신
                }
                else
                {
                    Debug.Log("골드 부족");
                }
                break;
            case 1: //InfernoLance
                if (Manager.gold >= infernoLanceNeedGold)
                {
                    Manager.gold -= infernoLanceNeedGold;
                    //총 생성
                    //재료들 소모
                    //UI 갱신
                }
                else
                {
                    Debug.Log("골드 부족");
                }
                break;
        }
    }
}
