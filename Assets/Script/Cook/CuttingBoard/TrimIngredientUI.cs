using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TrimIngredientUI : MonoBehaviour
{
    [SerializeField] CookUIManager cookUIManager;

    [Header("Inventory")]
    [SerializeField] RefrigeratorInventory refrigeratorInventory;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("IngredientUI")]
    [SerializeField] GameObject trimIngredientUI;
    [SerializeField] GameObject trimIngredientUIParent;
    [SerializeField] GameObject slotPrefab;

    [Header("ScrollView")]
    [SerializeField] ScrollViewSystem scrollViewSystem;

    //Button
    public void Start()
    {
        //TrimIngredientList();
        TrimRecipeList();
    }

    public void UIDisable()
    {
        this.gameObject.SetActive(false);

    }

    public void TrimRecipeList()
    {
        ingredientInventory.IngredientSlotClear();
        foreach (Recipe recipe in RecipeManager.instance.RecipeList)
        {
            if (recipe.cookType == CookType.Cutting && CheckRequireIngredient(recipe))
            {
                GameObject slotObject = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
                slotObject.transform.SetParent(trimIngredientUIParent.transform);
                slotObject.AddComponent<TrimIngredientSlot>();
                slotObject.GetComponent<TrimIngredientSlot>().SlotUISetup(recipe, ingredientInventory);
                slotObject.GetComponent<TrimIngredientSlot>().OnSelectIngredient += UIDisable;
            }
        }
    }

    private bool CheckRequireIngredient(Recipe recipe)
    {
        if (IngredientManager.IngredientAmount[recipe.mainIngredient] > 0)
        {
            return true;
        }
        return false;
    }


    // public void TrimIngredientList()
    // {
    //     ingredientInventory.IngredientSlotClear();
    //     foreach (KeyValuePair<Ingredient, int> ingredientPair in IngredientManager.IngredientAmount)
    //     {
    //         Ingredient ingredient = ingredientPair.Key;
    //         int ingredientAmount = ingredientPair.Value;
    //         if (ingredientAmount <= 0) continue;
    //         GameObject slotObject = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
    //         switch (ingredient.ingredientType)
    //         {
    //             case IngredientType.Trim:
    //                 slotObject.transform.SetParent(trimIngredientUIParent.transform);
    //                 slotObject.AddComponent<TrimIngredientSlot>();
    //                 slotObject.GetComponent<TrimIngredientSlot>().SlotUISetup(ingredient, ingredientInventory);
    //                 slotObject.GetComponent<TrimIngredientSlot>().OnSelectIngredient += UIDisable;
    //                 break;

    //         }
    //     }
    // }


}
