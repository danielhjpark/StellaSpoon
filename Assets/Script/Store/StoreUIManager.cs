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

    public void CallChatUI() //��ȭâ ON
    {
        chatUI.SetActive(true);
    }
    public void CloseChatUI() //��ȭâ OFF
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
    private GameObject[] stage2_ingredientButtons; //��� ��ư��


    [Header("Ingredient List")]
    [SerializeField]
    private List<Item> items; //��� ������ ����Ʈ

    [SerializeField]
    private int currentPurchaseCount = 0; //���� ���� ����
    private int currentSelectedIngredientIndex = -1; //���� ���õ� ��� �ε���

    [Header("-----Cook-----")]
    //���� �����ⱸ ����
    [SerializeField]
    private int currentPanLevel = 1;
    [SerializeField]
    private int currentWorLevel = 1;
    [SerializeField]
    private int currentCuttingBoardLevel = 1;
    [SerializeField]
    private int currentPotLevel = 1;

    //�ִ� �����ⱸ ����
    private int maxPanLevel = 2;
    private int maxWorLevel = 4;
    private int maxCuttingBoardLevel = 3;
    private int maxPotLevel = 4;

    //�����ⱸ ���� Text
    [SerializeField]
    private GameObject panLevelText;
    [SerializeField]
    private GameObject worLevelText;
    [SerializeField]
    private GameObject cuttingBoardLevelText;
    [SerializeField]
    private GameObject potLevelText;

    //���׷��̵� ���
    [SerializeField]
    private int[] panUpgradeCost;
    [SerializeField]
    private int[] worUpgradeCost;
    [SerializeField]
    private int[] cuttingBoardUpgradeCost;
    [SerializeField]
    private int[] potUpgradeCost;

    //���׷��̵� ��� Text
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
    private GameObject gunImage; //image�� resurces ���� Gun�� �־����.
    [SerializeField]
    private TextMeshProUGUI gunName;
    [SerializeField]
    private TextMeshProUGUI gunStats;

    // ��� 1
    [SerializeField]
    private GameObject Ingredient01;
    [SerializeField]
    private TextMeshProUGUI Ingredient01CurrentCount;
    // ��� 2
    [SerializeField]
    private GameObject Ingredient02;
    [SerializeField]
    private TextMeshProUGUI Ingredient02CurrentCount;
    // �ʿ��� ����
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
            Debug.LogWarning("Inventory ������Ʈ�� ã�� �� �����ϴ�.");
        }
        ResetIngredientPurchase();
        SelectTempestFang(); //ó�� ������ �� TempestFang ����
        LevelCostSetting(); //������������ ���׷��̵� ��� ����
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
        //�� �ε�� �������� 1 Ŭ���� ���ο� ���� ��� ��ư Ȱ��ȭ
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
            Debug.Log("�߸��� �ε���");
            return;
        }

        currentSelectedIngredientIndex = index;
        Item selectedItem = items[index];
        Debug.Log(selectedItem.itemName + " ���õ�");

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
            Debug.Log("��Ḧ �����ϼ���");
            return;
        }
        Item selectedItem = items[currentSelectedIngredientIndex];
        int totalCost = selectedItem.itemBuyPrice * currentPurchaseCount;

        if (Manager.gold >= totalCost)
        {
            Manager.gold -= totalCost;
            Debug.Log(string.Format("{0} x {1} ���� �Ϸ�. ���� Gold: {2}", selectedItem.itemName, currentPurchaseCount, Manager.gold));

            inventory.AcquireItem(selectedItem, currentPurchaseCount);
            //TODO: �κ��丮�� ������ �߰� �ʿ�

            Debug.Log("������ ����");
            ResetIngredientPurchase();
        }
        else
        {
            Debug.Log("��� ����");
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
        ref int upgradeLevel = ref currentPanLevel; //�⺻������ ���� ���������� switch���� ����
        bool isMaxLevel = false;

        switch (tools)
        {
            case "Pan":
                upgradeCost = panUpgradeCost;
                upgradeLevel = ref currentPanLevel;
                if(currentPanLevel >= maxPanLevel)
                {
                    Debug.Log("�� ���׷��̵� �ִ� ���� ����");
                    isMaxLevel = true;
                    return;
                }
                break;
            case "Wor":
                upgradeCost = worUpgradeCost;
                upgradeLevel = ref currentWorLevel;
                if (currentWorLevel >= maxWorLevel)
                {
                    Debug.Log("�� ���׷��̵� �ִ� ���� ����");
                    isMaxLevel = true;
                    return;
                }
                break;
            case "CuttingBoard":
                upgradeCost = cuttingBoardUpgradeCost;
                upgradeLevel = ref currentCuttingBoardLevel;
                if (currentCuttingBoardLevel >= maxCuttingBoardLevel)
                {
                    Debug.Log("���� ���׷��̵� �ִ� ���� ����");
                    isMaxLevel = true;
                    return;
                }
                break;
            case "Pot":
                upgradeCost = potUpgradeCost;
                upgradeLevel = ref currentPotLevel;
                if (currentPotLevel >= maxPotLevel)
                {
                    Debug.Log("���� ���׷��̵� �ִ� ���� ����");
                    isMaxLevel = true;
                    return;
                }
                break;
            default:
                Debug.Log("�߸��� ����");
                return;
        }

        if(!isMaxLevel) //�ִ뷹���� �ƴ� ��
        {
            if (upgradeCost[upgradeLevel-1] <=Manager.gold) //���� ��尡 ���׷��̵� ��뺸�� ���� ��
            {
                upgradeLevel++;
                Manager.gold -= upgradeCost[upgradeLevel - 1];
                Debug.Log(tools + " ���׷��̵� �Ϸ�. ���� ����" + upgradeLevel);

                switch (tools)
                {
                    case "Pan":
                        //���� �������� ���׷��̵� 
                        //���� ���������� ������ upgradeLevel, current000Level�� ����

                        panLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentPanLevel.ToString();
                        panUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentPanLevel >= maxPanLevel) ? "Max" : panUpgradeCost[currentPanLevel - 1].ToString();
                        break;
                    case "Wor":
                        //���� �������� ���׷��̵�

                        worLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentWorLevel.ToString();
                        worUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentWorLevel >= maxWorLevel) ? "Max" : worUpgradeCost[currentWorLevel - 1].ToString();
                        break;
                    case "CuttingBoard":
                        //���� �������� ���׷��̵�

                        cuttingBoardLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentCuttingBoardLevel.ToString();
                        cuttingBoardUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentCuttingBoardLevel >= maxCuttingBoardLevel) ? "Max" : cuttingBoardUpgradeCost[currentCuttingBoardLevel - 1].ToString();
                        break;
                    case "Pot":
                        //���� �������� ���׷��̵�

                        potLevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currentPotLevel.ToString();
                        potUpgradeCostText.GetComponent<TextMeshProUGUI>().text = (currentPotLevel >= maxPotLevel) ? "Max" : potUpgradeCost[currentPotLevel - 1].ToString();
                        break;
                }

            }
            else
            {
                Debug.Log("��� ����");
            }
        }
    }

    //gunNPC
    public void SelectTempestFang()
    {
        gunType = 0;
        gunImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Gun/TempestFang");
        //gunImage ���� �ʿ�
        gunName.text = "Tempest Fang";
        //gunName ���� �ʿ�
        gunStats.text = "Damage: 20\nFire Rate: 0.2\nRange: 100";
        //gunStats ���� �ʿ�
        gunNeedGold.text = tempestFangNeedGold.ToString();

        // ��� 1 ������ �߰�
        int currentCount1 = inventory.GetItemCount("Ingredient01");
        Ingredient01CurrentCount.text = $"{currentCount1}/{tempestCount}";
    }

    public void SelectInfernoLance()
    {
        gunType = 1;
        gunImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Gun/InfernoLance");
        //gunImage ���� �ʿ�
        gunName.text = "Inferno Lance";
        //gunName ���� �ʿ�
        gunStats.text = "Damage: 30\nFire Rate: 0.1\nRange: 150";
        //gunStats ���� �ʿ�

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
                    //�� ����
                    //���� �Ҹ�
                    //UI ����
                }
                else
                {
                    Debug.Log("��� ����");
                }
                break;
            case 1: //InfernoLance
                if (Manager.gold >= infernoLanceNeedGold)
                {
                    Manager.gold -= infernoLanceNeedGold;
                    //�� ����
                    //���� �Ҹ�
                    //UI ����
                }
                else
                {
                    Debug.Log("��� ����");
                }
                break;
        }
    }
}
