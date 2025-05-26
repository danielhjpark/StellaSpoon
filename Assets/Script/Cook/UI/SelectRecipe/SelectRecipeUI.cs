using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectRecipeUI : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] GameObject recipeParent;
    [SerializeField] GameObject recipePrefab;
    [SerializeField] IngredientInventory ingredientInventory;


    void Start()
    {
        RecipeListSetup();
        ingredientInventory.gameObject.SetActive(false);
    }

    public void RecipeListInit()
    {
        recipeParent.SetActive(true);
        foreach (Transform child in recipeParent.transform)
        {
            Destroy(child.gameObject);
        }
        ingredientInventory.IngredientSlotClear();
    }

    public void RecipeUpdate()
    {
        this.gameObject.SetActive(false);
    }

    public void RecipeListSetup()
    {
        RecipeListInit();
        int recipeCount = DailyMenuManager.dailyMenuList.Count;
        if (recipeCount <= 0) return;

        foreach (Recipe recipe in DailyMenuManager.dailyMenuList.Keys)
        {
            if (recipe.cookType != CookManager.instance.currentCookType) continue;
            else if (DailyMenuManager.dailyMenuList[recipe] <= 0) continue;
            GameObject recipeObject = Instantiate(recipePrefab, Vector3.zero, Quaternion.identity);
            recipeObject.transform.SetParent(recipeParent.transform);

            SelectRecipeSlot selectRecipe = recipeObject.GetComponent<SelectRecipeSlot>();
            selectRecipe.OnSelectRecipe += RecipeUpdate;
            selectRecipe.RecipeUISetup(recipe, ingredientInventory);
            
        }

    }

}
