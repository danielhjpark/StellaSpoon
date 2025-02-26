using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject DailyMenuUI;
    [SerializeField] GameObject RefrigeratorUI;
    [SerializeField] GameObject DeviceUI;
    [SerializeField] GameObject CookUI;
    [SerializeField] Inventory inventory;

    [SerializeField] Item[] items;

    [SerializeField] IngredientSlot[] ingredientSlot;
    [SerializeField] Ingredient[] ingredients;

    [SerializeField] bool developMode;
    [SerializeField] Recipe[] recipes;
    void Start() {
        InventoryUI();
        DailyMenuAdd();
    }

    void Update()
    {
        OpenUI();
        if(Input.GetKeyDown(KeyCode.T)) {
            OrderManager.instance.OpenRestaurant();
        }
    }


    void OpenUI() {
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            bool currentState = DailyMenuUI.activeSelf;
            DailyMenuUI.SetActive(!currentState);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            bool currentState = RefrigeratorUI.activeSelf;
            RefrigeratorUI.SetActive(!currentState);
            DeviceUI.SetActive(!currentState);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            bool currentState = CookUI.activeSelf;
            CookUI.SetActive(!currentState);
        }
    }

    void InventoryUI() {
        foreach(var item in items) {
            inventory.AcquireItem(item);
        }
    }

    void DailyMenuAdd() {
        foreach (Recipe recipe in recipes)
            DailyMenuManager.dailyMenuList.Add(recipe, 1);
    }
}
