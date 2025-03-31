using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class RefrigeratorManager : MonoBehaviour
{
    static public RefrigeratorManager instance;
    [SerializeField] GameObject refrigeratorObject;
    [SerializeField] RefrigeratorInventory refrigeratorInventory;

    bool inventoryActivated;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }
    }

    void Start()
    {
        inventoryActivated = true;

    }

    public RefrigeratorInventory BindInventory() {
        return refrigeratorInventory;
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
        refrigeratorObject.SetActive(true);
        inventoryActivated = false;
        DailyMenuIngredients();
    }

    public void OpenAllIngredientsInventory() {
        refrigeratorObject.SetActive(true);
        inventoryActivated = false;
        //AllofIngredients();
    }


    public void CloseRefrigeratorInventory() {
        refrigeratorObject.SetActive(false);
        inventoryActivated = true;
    }

    //-----------------Inventory Setup-----------------------//
    // void AllofIngredients() {
    //     foreach(KeyValuePair<Ingredient, int> currentMenu in IngredientManager.IngredientAmount) {
    //         RefrigeratorSlot slot = CheckEmptySlot();
    //         if(slot != null) {
    //             slot.AddIngredient(currentMenu.Key, currentMenu.Value);
    //         }
    //     }
    // }

    void DailyMenuIngredients() {
        if(DailyMenuManager.dailyMenuList.Count <= 0) {return;}

        Dictionary<Ingredient, int> requireIngredients = new Dictionary<Ingredient, int> ();
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
        // foreach(KeyValuePair<Ingredient, int> currentMenu in requireIngredients) {
        //     RefrigeratorSlot slot = CheckEmptySlot();
        //     if(slot != null) {
        //         slot.AddIngredient(currentMenu.Key, currentMenu.Value);
        //     }
        // }

    }

    //----------------Use Ingredients------------------------//
    public void UseIngredientToInventory(Recipe recipe, int amount) {
        refrigeratorInventory.UseIngredient(recipe.mainIngredient, amount);

        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount * amount;
            refrigeratorInventory.UseIngredient(currentIngdeient, requireIngredientAmount);
        }
    }
    //-----------------Recall Ingredients ----------------------//
    public void RecallIngredientToInventory(Recipe recipe, int amount) {
        refrigeratorInventory.RecallIngredient(recipe.mainIngredient, amount);

        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount * amount;
            refrigeratorInventory.RecallIngredient(currentIngdeient,requireIngredientAmount);
        }
    }

}
