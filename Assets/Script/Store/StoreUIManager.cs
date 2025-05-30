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

    [Header("-----ingredient-----")]
    [SerializeField]
    private List<string> sellItemNames = new List<string>
{
    "Ruby_Root",
    "Allirubium",
    "Barcrose_Meet",
    "Hornavia_Meet",
    "AdenBear_LegMeet",
    "Red_Barcrose_Meet",
    "Red_Hornavia_Meet",
    "Cont_AdenBear_Meet",
    "Alivery_Meet",
    "Glasta",
    "Bypin",
    "Fermos_Sell",
    "Silver_Wolf_Rib",
    "Black_Fermos_Meet",
    "Nova_Wolf_Meet"
};
    [SerializeField]
    private GameObject buyBase;
    [SerializeField]
    private GameObject sellBase;

    [SerializeField]
    public TextMeshProUGUI countText;
    [SerializeField]
    public TextMeshProUGUI ingredientNeedGold;

    [SerializeField]
    private GameObject[] stage2_ingredientButtons; // 스테이지 2 전용 재료 버튼들
    [SerializeField]
    private GameObject[] sellItemButtons; // 팔 재료 버튼

    [SerializeField]
    private List<Item> items; //재료 정보들 리스트

    [SerializeField]
    private int currentPurchaseCount = 0; //현재 구매 갯수
    private int currentSelectedIngredientIndex = -1; //현재 선택된 재료 인덱스

    public enum CurrentState
    {
        Buy,
        Sell
    }
    public CurrentState currentState;

    [Header("-----Cook-----")]
    //조리기구 레벨 Text
    [SerializeField]
    private GameObject panLevelText;
    [SerializeField]
    private GameObject worLevelText;
    [SerializeField]
    private GameObject cuttingBoardLevelText;
    [SerializeField]
    private GameObject potLevelText;

    //현재 조리기구 레벨
    [SerializeField]
    static public int currentPanLevel = 1;
    [SerializeField]
    static public int currentWorLevel = 1;
    [SerializeField]
    static public int currentCuttingBoardLevel = 1;
    [SerializeField]
    static public int currentPotLevel = 1;

    //최대 조리기구 레벨
    private int maxPanLevel = 2;
    private int maxWorLevel = 4;
    private int maxCuttingBoardLevel = 3;
    private int maxPotLevel = 4;
    

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

    [SerializeField]
    private TextMeshProUGUI panUpgradeDetailText;
    [SerializeField]
    private TextMeshProUGUI worUpgradeDetailText;
    [SerializeField]
    private TextMeshProUGUI cuttingBoardUpgradeDetailText;
    [SerializeField]
    private TextMeshProUGUI potUpgradeDetailText;

    [SerializeField]
    private List<string> panUpgradeList = new List<string>();
    [SerializeField]
    private List<string> worUpgradeList = new List<string>();
    [SerializeField]
    private List<string> cuttingBoardUpgradeList = new List<string>();
    [SerializeField]
    private List<string> potUpgradeList = new List<string>();

    [Header("-----Gun-----")]
    [SerializeField]
    private GameObject gunImage; //image는 resurces 폴더 Gun에 넣어야함.
    [SerializeField]
    private TextMeshProUGUI gunName;
    [SerializeField]
    private TextMeshProUGUI gunStats;
    // 필요한 갯수
    [SerializeField]
    private TextMeshProUGUI gunNeedGold;

    [SerializeField]
    private int tempestBTCount;
    [SerializeField]
    private int tempestRHBCount;
    [SerializeField]
    private int tempestABLCount;
    [SerializeField]
    private int tempestVerCount;
    [SerializeField]
    private int tempestScrCount;

    [SerializeField]
    private int infernoRBLCount;
    [SerializeField]
    private int infernoBFFCount;
    [SerializeField]
    private int infernoNWCCount;
    [SerializeField]
    private int infernoInfCount;
    [SerializeField]
    private int infernoScrCount;

    [SerializeField]
    private int gunType; //0: TempestFang, 1: InfernoLance


    [Header("Gun Create Cost")]
    [SerializeField]
    private int tempestFangNeedGold = 1500;
    [SerializeField]
    private int infernoLanceNeedGold = 2700;

    [Header("-----Gun UI-----")]
    [SerializeField] private GameObject gunImage1;
    [SerializeField] private TextMeshProUGUI gunName1;
    [SerializeField] private TextMeshProUGUI gunStats1;
    [SerializeField] private TextMeshProUGUI[] ingredientCounts; // 0~4
    [SerializeField] private GameObject[] ingredientImage;
    [SerializeField] private TextMeshProUGUI gunNeedGold1;

    [Header("Gun Recipes")]
    [SerializeField] private GunRecipe[] gunRecipes;

    private void Start()
    {
        inventory = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)").GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("Inventory 오브젝트를 찾을 수 없습니다.");
        }
        ResetIngredientPurchase();
        SelectGun(0);
        LevelCostSetting(); //조리도구상점 업그레이드 비용 설정
        UpdateAllSellButtons();
    }

    public void CallChatUI() //대화창 ON
    {
        chatUI.SetActive(true);
    }
    public void CloseChatUI() //대화창 OFF
    {
        chatUI.SetActive(false);
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
    public void PlusButton()
    {
        if (currentState == CurrentState.Sell)
        {
            //가지고 있는 갯수보다 많이 선택 불가능
            if (currentPurchaseCount < inventory.GetItemCount(items[currentSelectedIngredientIndex].name))
            {
                currentPurchaseCount++;
                countText.GetComponent<TextMeshProUGUI>().text = currentPurchaseCount.ToString();
                UpdateIngredientTotalCost();
            }
        }
        else if (currentState == CurrentState.Buy)
        {
            //구매 갯수 99개까지 가능
            if (currentPurchaseCount < 99)
            {
                currentPurchaseCount++;
                countText.GetComponent<TextMeshProUGUI>().text = currentPurchaseCount.ToString();
                UpdateIngredientTotalCost();
            }
        }
        SoundManager.instance.PlaySound(SoundManager.Store.Button);
    }
    public void MinusButton()
    {
        if (currentPurchaseCount > 0)
        {
            currentPurchaseCount--;
            countText.GetComponent<TextMeshProUGUI>().text = currentPurchaseCount.ToString();
            UpdateIngredientTotalCost();
        }
        SoundManager.instance.PlaySound(SoundManager.Store.Button);
    }

    public void SelectedIngredient(int index)
    {
        if (index < 0 || index >= items.Count)
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
        int price;
        if (currentState == CurrentState.Sell)
        {
            price = items[currentSelectedIngredientIndex].itemSellPrice;
        }
        else if (currentState == CurrentState.Buy)
        {
            price = items[currentSelectedIngredientIndex].itemBuyPrice;
        }
        else
        {
            Debug.Log("잘못된 상태");
            return;
        }
        int totalCost = price * currentPurchaseCount;
        if (ingredientNeedGold != null)
        {
            ingredientNeedGold.text = totalCost.ToString();
        }
    }

    public void BuyIngredient()
    {
        if (currentSelectedIngredientIndex < 0)
        {
            PopupManager.Instance.ShowPopup("재료를 선택하세요.");
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
            UpdateAllSellButtons();
            SoundManager.instance.PlaySound(SoundManager.Store.Daily_Menu_Button);
        }
        else
        {
            PopupManager.Instance.ShowPopup("금액이 부족합니다.");
        }
    }

    public void SellIngredient()
    {
        if (currentSelectedIngredientIndex < 0)
        {
            PopupManager.Instance.ShowPopup("재료를 선택하세요.");
            return;
        }
        Item selectedItem = items[currentSelectedIngredientIndex];
        int totalCost = selectedItem.itemSellPrice * currentPurchaseCount;


        Manager.gold += totalCost;
        Debug.Log(string.Format("{0} x {1} 판매 완료. ", selectedItem.itemName, currentPurchaseCount));

        inventory.DecreaseItemCount(selectedItem.name, currentPurchaseCount);
        //TODO: 인벤토리에서 해당 아이템 제거

        Debug.Log("아이템 판매");
        ResetIngredientPurchase();
        UpdateAllSellButtons();
        SoundManager.instance.PlaySound(SoundManager.Store.Daily_Menu_Button);
    }
    public void UpdateAllSellButtons()
    {
        for (int i = 0; i < sellItemNames.Count; i++)
        {
            string itemName = sellItemNames[i];
            int itemCount = inventory.GetItemCount(itemName);

            if (i < sellItemButtons.Length)
            {
                sellItemButtons[i].SetActive(itemCount > 0);
            }
        }
    }
    //cook

    private void LevelCostSetting()
    {
        panLevelText.GetComponent<TextMeshProUGUI>().text = "Lv: " + currentPanLevel.ToString();
        worLevelText.GetComponent<TextMeshProUGUI>().text = "Lv: " + currentWorLevel.ToString();
        cuttingBoardLevelText.GetComponent<TextMeshProUGUI>().text = "Lv: " + currentCuttingBoardLevel.ToString();
        potLevelText.GetComponent<TextMeshProUGUI>().text = "Lv: " + currentPotLevel.ToString();
        panUpgradeCostText.GetComponent<TextMeshProUGUI>().text = panUpgradeCost[currentPanLevel - 1].ToString();
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
                if (currentPanLevel >= maxPanLevel)
                {
                    Debug.Log("팬 업그레이드 최대 레벨 도달");
                    isMaxLevel = true;
                }
                break;
            case "Wor":
                upgradeCost = worUpgradeCost;
                upgradeLevel = ref currentWorLevel;
                if (currentWorLevel >= maxWorLevel)
                {
                    Debug.Log("웍 업그레이드 최대 레벨 도달");
                    isMaxLevel = true;
                }
                break;
            case "CuttingBoard":
                upgradeCost = cuttingBoardUpgradeCost;
                upgradeLevel = ref currentCuttingBoardLevel;
                if (currentCuttingBoardLevel >= maxCuttingBoardLevel)
                {
                    Debug.Log("도마 업그레이드 최대 레벨 도달");
                    isMaxLevel = true;
                }
                break;
            case "Pot":
                upgradeCost = potUpgradeCost;
                upgradeLevel = ref currentPotLevel;
                if (currentPotLevel >= maxPotLevel)
                {
                    Debug.Log("냄비 업그레이드 최대 레벨 도달");
                    isMaxLevel = true;
                }
                break;
            default:
                Debug.Log("잘못된 도구");
                return;
        }

        if (!isMaxLevel) //최대레벨이 아닐 때
        {
            if (upgradeCost[upgradeLevel - 1] <= Manager.gold) //보유 골드가 업그레이드 비용보다 많을 때
            {
                Manager.gold -= upgradeCost[upgradeLevel - 1];
                upgradeLevel++;
                Debug.Log(tools + " 업그레이드 완료. 현재 레벨" + upgradeLevel);
                UpgradeCookDetail(tools);
                switch (tools)
                {
                    case "Pan":
                        //각각 조리도구 업그레이드 
                        //현재 조리도구의 레벨은 upgradeLevel, current000Level로 가능

                        panLevelText.GetComponent<TextMeshProUGUI>().text = "Lv: " + currentPanLevel.ToString();
                        panUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentPanLevel >= maxPanLevel) ? "Max" : panUpgradeCost[currentPanLevel - 1].ToString();
                        break;
                    case "Wor":
                        //각각 조리도구 업그레이드

                        worLevelText.GetComponent<TextMeshProUGUI>().text = "Lv: " + currentWorLevel.ToString();
                        worUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentWorLevel >= maxWorLevel) ? "Max" : worUpgradeCost[currentWorLevel - 1].ToString();
                        break;
                    case "CuttingBoard":
                        //각각 조리도구 업그레이드

                        cuttingBoardLevelText.GetComponent<TextMeshProUGUI>().text = "Lv: " + currentCuttingBoardLevel.ToString();
                        cuttingBoardUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentCuttingBoardLevel >= maxCuttingBoardLevel) ? "Max" : cuttingBoardUpgradeCost[currentCuttingBoardLevel - 1].ToString();
                        break;
                    case "Pot":
                        //각각 조리도구 업그레이드

                        potLevelText.GetComponent<TextMeshProUGUI>().text = "Lv: " + currentPotLevel.ToString();
                        potUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentPotLevel >= maxPotLevel) ? "Max" : potUpgradeCost[currentPotLevel - 1].ToString();
                        break;
                }
                SoundManager.instance.PlaySound(SoundManager.Store.Daily_Menu_Button);

            }
            else
            {
                PopupManager.Instance.ShowPopup("골드 부족");
                Debug.Log("골드 부족");
            }
        }
        else
        {
            SoundManager.instance.PlaySound(SoundManager.Store.Daily_Menu_Button);
        }
    }

    private void UpgradeCookDetail(string name)
    {
        switch (name)
        {
            case "Pan":
                    panUpgradeDetailText.text = panUpgradeList[currentPanLevel - 1];
                break;
            case "Wor":
                    worUpgradeDetailText.text = worUpgradeList[currentWorLevel - 1];
                break;
            case "CuttingBoard":
                    cuttingBoardUpgradeDetailText.text = cuttingBoardUpgradeList[currentCuttingBoardLevel - 1];
                break;
            case "Pot":
                    potUpgradeDetailText.text = potUpgradeList[currentPotLevel - 1];
                break;
        }
    }

    //gunNPC
    public void SelectGun(int index)
    {
        if (index < 0 || index >= gunRecipes.Length) return;

        if (index == 0)
        {
            gunStats.text = "Damage: 10\nWeight: 3\nSpeed: 0.15";
        }
        else if (index == 1)
        {
            gunStats.text = "Damage: 70\nWeight: 10\nSpeed: 0.8";
        }

        gunType = index;
        GunRecipe recipe = gunRecipes[gunType];

        gunImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(recipe.spritePath);
        gunName.text = recipe.gunName;
        gunNeedGold.text = recipe.needGold.ToString();

        UpdateIngredientUI(recipe);
    }

    private void UpdateIngredientUI(GunRecipe recipe)
    {
        for (int i = 0; i < ingredientCounts.Length; i++)
        {
            if (i < recipe.ingredients.Count)
            {
                var ing = recipe.ingredients[i];
                int currentCount = inventory.GetItemCount(ing.ingredientName);
                ingredientCounts[i].text = $"{currentCount}/{ing.requiredAmount}";
                ingredientImage[i].SetActive(true);
                ingredientImage[i].GetComponent<Image>().sprite = ing.ingredientSprite;
            }
            else
            {
                ingredientCounts[i].text = "-";
                ingredientImage[i].SetActive(false); // 남는 슬롯은 숨김
            }
        }
    }

    public void CreateGun()
    {
        GunRecipe recipe = gunRecipes[gunType];

        if (RifleManager.instance.tempestFang == true || RifleManager.instance.infernoLance == true)
        {
            PopupManager.Instance.ShowPopup("이미 제작된 총입니다.");
            return;
        }

        if (Manager.gold < recipe.needGold)
        {
            PopupManager.Instance.ShowPopup("금액이 부족합니다.");
            return;
        }

        foreach (var ing in recipe.ingredients)
        {
            if (inventory.GetItemCount(ing.ingredientName) < ing.requiredAmount)
            {
                Debug.Log($"재료 부족: {ing.ingredientName}");
                PopupManager.Instance.ShowPopup("재료가 부족합니다.");
                return;
            }
        }

        Manager.gold -= recipe.needGold;
        foreach (var ing in recipe.ingredients)
        {
            inventory.DecreaseItemCount(ing.ingredientName, ing.requiredAmount);
        }

        UpdateIngredientUI(recipe);

        // 획득 플래그 설정
        if (gunType == 0)
            RifleManager.instance.tempestFang = true;
        else if (gunType == 1)
            RifleManager.instance.infernoLance = true;

        Debug.Log($"{recipe.gunName} 생성 완료");
        SoundManager.instance.PlaySound(SoundManager.Store.Daily_Menu_Button);
    }
}
