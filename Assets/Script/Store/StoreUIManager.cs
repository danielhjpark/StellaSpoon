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

    [Header("-----Cook-----")]
    //현재 조리기구 레벨
    [SerializeField]
    private int currentPanLevel = 1;
    [SerializeField]
    private int currentWorLevel = 1;
    [SerializeField]
    private int currentCuttingBoardLevel = 1;
    [SerializeField]
    private int currentPotLevel = 1;

    //최대 조리기구 레벨
    private int maxPanLevel = 2;
    private int maxWorLevel = 4;
    private int maxCuttingBoardLevel = 3;
    private int maxPotLevel = 4;

    //조리기구 레벨 Text
    [SerializeField]
    private GameObject panLevelText;
    [SerializeField]
    private GameObject worLevelText;
    [SerializeField]
    private GameObject cuttingBoardLevelText;
    [SerializeField]
    private GameObject potLevelText;

    //업그레이드 비용
    [SerializeField]
    private int[] panUpgradeCost;
    [SerializeField]
    private int[] worUpgradeCost;
    [SerializeField]
    private int[] cuttingBoardUpgradeCost;
    [SerializeField]
    private int[] potUpgradeCost;

    //업그레이드 비용 Text
    [SerializeField]
    private GameObject panUpgradeCostText;
    [SerializeField]
    private GameObject worUpgradeCostText;
    [SerializeField]
    private GameObject cuttingBoardUpgradeCostText;
    [SerializeField]
    private GameObject potUpgradeCostText;

    [Header("-----Gun-----")]
    [SerializeField]
    private GameObject gunImage; //image는 resurces 폴더 Gun에 넣어야함.
    [SerializeField]
    private TextMeshProUGUI gunName;
    [SerializeField]
    private TextMeshProUGUI gunStats;

    // 재료 1
    [SerializeField]
    private GameObject Ingredient01;
    [SerializeField]
    private TextMeshProUGUI Ingredient01CurrentCount;
    // 재료 2
    [SerializeField]
    private GameObject Ingredient02;
    [SerializeField]
    private TextMeshProUGUI Ingredient02CurrentCount;
    // 필요한 갯수
    [SerializeField]
    private TextMeshProUGUI gunNeedGold;
    [SerializeField]
    private int tempestCount;
    [SerializeField]
    private int infernoCount;

    [SerializeField]
    private int gunType; //0: TempestFang, 1: InfernoLance


    [Header("Gun Create Cost")]
    [SerializeField]
    private int tempestFangNeedGold = 1500;
    [SerializeField]
    private int infernoLanceNeedGold = 2700;

    private void Start()
    {
        inventory = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)").GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("Inventory 오브젝트를 찾을 수 없습니다.");
        }
        ResetIngredientPurchase();
        SelectTempestFang(); //처음 시작할 때 TempestFang 선택
        LevelCostSetting(); //조리도구상점 업그레이드 비용 설정
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

    private void LevelCostSetting()
    {
        panLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentPanLevel.ToString();
        worLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentWorLevel.ToString();
        cuttingBoardLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentCuttingBoardLevel.ToString();
        potLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentPotLevel.ToString();
        panUpgradeCostText.GetComponent<TextMeshProUGUI>().text =panUpgradeCost[currentPanLevel - 1].ToString();
        worUpgradeCostText.GetComponent<TextMeshProUGUI>().text = worUpgradeCost[currentWorLevel - 1].ToString();
        cuttingBoardUpgradeCostText.GetComponent<TextMeshProUGUI>().text = cuttingBoardUpgradeCost[currentCuttingBoardLevel - 1].ToString();
        potUpgradeCostText.GetComponent<TextMeshProUGUI>().text = potUpgradeCost[currentPotLevel - 1].ToString();
    }
    public void Upgrade(string tools)
    {
        int[] upgradeCost;
        ref int upgradeLevel = ref currentPanLevel; //기본적으로 팬을 참조하지만 switch에서 변경
        bool isMaxLevel = false;

        switch (tools)
        {
            case "Pan":
                upgradeCost = panUpgradeCost;
                upgradeLevel = ref currentPanLevel;
                if(currentPanLevel >= maxPanLevel)
                {
                    Debug.Log("팬 업그레이드 최대 레벨 도달");
                    isMaxLevel = true;
                    return;
                }
                break;
            case "Wor":
                upgradeCost = worUpgradeCost;
                upgradeLevel = ref currentWorLevel;
                if (currentWorLevel >= maxWorLevel)
                {
                    Debug.Log("웍 업그레이드 최대 레벨 도달");
                    isMaxLevel = true;
                    return;
                }
                break;
            case "CuttingBoard":
                upgradeCost = cuttingBoardUpgradeCost;
                upgradeLevel = ref currentCuttingBoardLevel;
                if (currentCuttingBoardLevel >= maxCuttingBoardLevel)
                {
                    Debug.Log("도마 업그레이드 최대 레벨 도달");
                    isMaxLevel = true;
                    return;
                }
                break;
            case "Pot":
                upgradeCost = potUpgradeCost;
                upgradeLevel = ref currentPotLevel;
                if (currentPotLevel >= maxPotLevel)
                {
                    Debug.Log("냄비 업그레이드 최대 레벨 도달");
                    isMaxLevel = true;
                    return;
                }
                break;
            default:
                Debug.Log("잘못된 도구");
                return;
        }

        if(!isMaxLevel) //최대레벨이 아닐 때
        {
            if (upgradeCost[upgradeLevel-1] <=Manager.gold) //보유 골드가 업그레이드 비용보다 많을 때
            {
                upgradeLevel++;
                Manager.gold -= upgradeCost[upgradeLevel - 1];
                Debug.Log(tools + " 업그레이드 완료. 현재 레벨" + upgradeLevel);

                switch (tools)
                {
                    case "Pan":
                        //각각 조리도구 업그레이드 
                        //현재 조리도구의 레벨은 upgradeLevel, current000Level로 가능

                        panLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentPanLevel.ToString();
                        panUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentPanLevel >= maxPanLevel) ? "Max" : panUpgradeCost[currentPanLevel - 1].ToString();
                        break;
                    case "Wor":
                        //각각 조리도구 업그레이드

                        worLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentWorLevel.ToString();
                        worUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentWorLevel >= maxWorLevel) ? "Max" : worUpgradeCost[currentWorLevel - 1].ToString();
                        break;
                    case "CuttingBoard":
                        //각각 조리도구 업그레이드

                        cuttingBoardLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentCuttingBoardLevel.ToString();
                        cuttingBoardUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentCuttingBoardLevel >= maxCuttingBoardLevel) ? "Max" : cuttingBoardUpgradeCost[currentCuttingBoardLevel - 1].ToString();
                        break;
                    case "Pot":
                        //각각 조리도구 업그레이드

                        potLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentPotLevel.ToString();
                        potUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentPotLevel >= maxPotLevel) ? "Max" : potUpgradeCost[currentPotLevel - 1].ToString();
                        break;
                }

            }
            else
            {
                Debug.Log("골드 부족");
            }
        }
    }

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

        // 재료 1 갯수만 추가
        int currentCount1 = inventory.GetItemCount("Ingredient01");
        Ingredient01CurrentCount.text = $"{currentCount1}/{tempestCount}";
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
