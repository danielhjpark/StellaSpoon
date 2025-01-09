using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject DailyMenuUI;
    [SerializeField] GameObject RefrigeratorUI;
    [SerializeField] GameObject DeviceUI;
    [SerializeField] GameObject CookUI;
    [SerializeField] Inventory inventory;
    [SerializeField] Item[] items;

    [SerializeField] IngredientSlot[] ingredientSlot;
    [SerializeField] Ingredient[] ingredients;


    void Start() {
        //InventoryUI();
        for(int i = 0; i < ingredients.Length; i++) {
            ingredientSlot[i].AddIngredient(ingredients[i]);
        }
        
    }
    void Update()
    {
        OpenUI();
        if(Input.GetKeyDown(KeyCode.F)) {
            //InventoryUI();
            //ingredientSlot.AddIngredient(ingredient);
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

}
