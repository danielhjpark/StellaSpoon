using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MakeRecipeUI : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] RefrigeratorInventory refrigeratorInventory;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("IngredientUI")]
    [SerializeField] GameObject mainIngredientUI;
    [SerializeField] GameObject mainIngredientParent;
    [SerializeField] GameObject sauceIngredientUI;
    [SerializeField] GameObject sauceIngredientParent;
    [SerializeField] GameObject slotPrefab;

    [Header("ScrollView")]
    [SerializeField] ScrollViewSystem scrollViewSystem;

    private List<Ingredient> addedIngredient;

    //Button
    public void Start()
    {
        MainIngredientList();
        sauceIngredientUI.SetActive(false);
    }

    void Update()
    {

    }

    //Event
    public void OnAddedIngredient(Ingredient addIngredient)
    {
        addedIngredient.Add(addIngredient);
    }

    public void MainIngredientList()
    {
        ingredientInventory.IngredientSlotClear();
        foreach (KeyValuePair<Ingredient, int> ingredientPair in IngredientManager.IngredientAmount)
        {
            Ingredient ingredient = ingredientPair.Key;
            int ingredientAmount = ingredientPair.Value;
            if (ingredientAmount <= 0) continue;
            GameObject slotObject = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
            switch (ingredient.ingredientType)
            {
                case IngredientType.Main:
                    slotObject.transform.SetParent(mainIngredientParent.transform);
                    slotObject.AddComponent<MainIngredientSlot>();
                    slotObject.GetComponent<MainIngredientSlot>().SlotUISetup(ingredient, ingredientInventory);
                    break;
                // case IngredientType.Sauce:
                //     slotObject.transform.SetParent(sauceIngredientParent.transform);
                //     slotObject.AddComponent<SauceIngredientSlot>();
                //     slotObject.GetComponent<MainIngredientSlot>().SlotUISetup(ingredient, ingredientInventory);
                //     break;
                default:
                    ingredientInventory.IngredientAdd(ingredient);
                    break;

            }
            //selectRecipe.OnSelectRecipe += RecipeUpdate;
        }
        //ingredientInventory.IngredientSlotClear();
    }

    public void NextUI()
    {

    }

    public void CheckCanMakeNewRecipe()
    {
        bool isCanMakeNewRecipe = true;

        if (isCanMakeNewRecipe)
        {

        }
        else
        {

        }
    }

}
