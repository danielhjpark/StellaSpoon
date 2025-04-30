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



    [Header("-----Gun-----")]
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
        SelectTempestFang(); //ó�� ������ �� TempestFang ����
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

        //���� ǥ���ؾ���. ��� �ؾ���? �ٸ��� �����ϸ� ������ �ٰų� �þ�µ�?
        //���� count ǥ���ؾ���. 0/����

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
