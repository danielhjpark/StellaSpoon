using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrigeratorManager : MonoBehaviour
{

    [SerializeField] GameObject refrigeratorInventory;
    [SerializeField] GameObject cookInventory;
    [SerializeField] DailyMenuManager dailyMenuManager;
    RefrigeratorSlot[] refrigeratorSlots;
    RefrigeratorInventory[] refrigeratorInventories;
    RefrigeratorSlot[] cookSlots;

    bool inventoryActivated;

    void Start()
    {
        refrigeratorInventories = this.GetComponentsInChildren<RefrigeratorInventory>();
        refrigeratorSlots = refrigeratorInventory.GetComponentsInChildren<RefrigeratorSlot>();
        cookSlots = cookInventory.GetComponentsInChildren<RefrigeratorSlot>();
        inventoryActivated = true;

    }

    void Update() {
        OpenInventorySystem();
    }

    //------------------Button----------------------//
    private int currentInventoryPage = 0;

    void NextInventory() {

    }

    void PreviousInventory() {

    }
    //----------------Input System------------------//
    void OpenInventorySystem() {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenDailyIngredeintsInventory();
        }
        else if(Input.GetKeyDown(KeyCode.CapsLock)) {
            OpenAllIngredientsInventory();
        }
    }

    //------------Inventory Controll ------------------//
    public void OpenDailyIngredeintsInventory()
    {
        refrigeratorInventory.SetActive(true);
        inventoryActivated = false;
        SlotClear();
        DailyMenuIngredients();
    }

    public void OpenAllIngredientsInventory() {
        refrigeratorInventory.SetActive(true);
        inventoryActivated = false;
        SlotClear();
        AllofIngredients();
    }


    public void CloseRefrigeratorInventory() {
        refrigeratorInventory.SetActive(false);
        inventoryActivated = true;
    }

    //-----------------Inventory Setup-----------------------//
    //모든 재료 Slot에 추가 
    void AllofIngredients() {
        foreach(KeyValuePair<Ingredient, int> currentMenu in IngredientManager.IngredientAmount) {
            RefrigeratorSlot slot = CheckEmptySlot();
            if(slot != null) {
                slot.AddIngredient(currentMenu.Key, currentMenu.Value);
            }
        }
    }

    //당일메뉴 재료 Slot에 추가
    void DailyMenuIngredients() {
        if(DailyMenuManager.dailyMenuList.Count <= 0) {return;}

        Dictionary<Ingredient, int> requireIngredients = new Dictionary<Ingredient, int> ();
        //DailyMenu의 재료들 카운팅
        foreach (KeyValuePair<Recipe, int> currentMenu in DailyMenuManager.dailyMenuList)
        {
            foreach(IngredientAmount currentIngredient in currentMenu.Key.ingredients) {
                int count = currentIngredient.amount * currentMenu.Value;
                if(!requireIngredients.ContainsKey(currentIngredient.ingredient))  {
                    requireIngredients.Add(currentIngredient.ingredient, 0);
                }
                requireIngredients[currentIngredient.ingredient] += count;
            }
        }

        //카운팅 된 재료들 Slot에 바인딩
        foreach(KeyValuePair<Ingredient, int> currentMenu in requireIngredients) {
            RefrigeratorSlot slot = CheckEmptySlot();
            if(slot != null) {
                slot.AddIngredient(currentMenu.Key, currentMenu.Value);
            }
        }

    }

    RefrigeratorSlot CheckEmptySlot() {
        foreach(RefrigeratorSlot slot in refrigeratorSlots) {
            if(slot.IsEmpty()) return slot;
        }
        Debug.Log("Not found Slot");
        return null;
    }

    void SlotClear() {
        foreach(RefrigeratorSlot slot in refrigeratorSlots) {
            slot.SlotClear();
        }
    }


}
