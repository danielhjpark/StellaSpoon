using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static public UIManager instance { get; private set;}
    [Header("UI")]
    [SerializeField] GameObject DailyMenuUI;
    [SerializeField] GameObject RefrigeratorUI;
    [SerializeField] GameObject DeviceUI;
    [SerializeField] GameObject CookUI;
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject NewRecipeUI;

    [SerializeField] Item[] items;

    [SerializeField] Recipe[] recipes;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InventoryUI(); //���� ��û �ؾ�¡
        InventoryUI();
        //DailyMenuAdd();
    }
    void Update()
    {
        OpenUI();
        if (Input.GetKeyDown(KeyCode.T))
        {
            OrderManager.instance.OpenRestaurant();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DailyMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Inventory.inventoryActivated = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DailyMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Inventory.inventoryActivated = false;
        }
    }

    public void NewRecipePanelVisible() {
        NewRecipeUI.SetActive(true);
        
    }
    

    void OpenUI()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bool currentState = DailyMenuUI.activeSelf;
            DailyMenuUI.SetActive(!currentState);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bool currentState = RefrigeratorUI.activeSelf;
            RefrigeratorUI.SetActive(!currentState);
            DeviceUI.SetActive(!currentState);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bool currentState = CookUI.activeSelf;
            CookUI.SetActive(!currentState);
        }
    }

    void InventoryUI()
    {
        foreach (var item in items)
        {
            inventory.AcquireItem(item);
        }
    }

    void DailyMenuAdd()
    {
        foreach (Recipe recipe in recipes)
            DailyMenuManager.dailyMenuList.Add(recipe, 3);
    }

    public void RecipeUnLockUI() {
        StartCoroutine(RecipeUnLockFade());
    }

    IEnumerator RecipeUnLockFade() {
        NewRecipeUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        NewRecipeUI.SetActive(false);
        
    }

    
}
