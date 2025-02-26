using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRecipeUI : MonoBehaviour
{
    [SerializeField] RectTransform recipePanel;
    [SerializeField] GameObject recipeParent;
    [SerializeField] GameObject recipePrefab;
    [SerializeField] IngredientInventory ingredientInventory;

    Vector2 recipePanelInitSize = new Vector2(2000, 200);

    void Start() {
        RecipeListSetup();
    }

    public void RecipeListInit() {
        recipeParent.SetActive(true);   
        foreach (Transform child in recipeParent.transform)
        {
            Destroy(child.gameObject);//ObjectPool ???ì²? ê°??Š¥?„±
        }
        ingredientInventory.IngredientSlotClear();
    }

    public void RecipeUIHide() {
        this.gameObject.SetActive(false);
    }

    public void RecipeListSetup() {
        RecipeListInit();
        int recipeCount = DailyMenuManager.dailyMenuList.Count;
        if(recipeCount <= 0) return;
        foreach (Recipe recipe in DailyMenuManager.dailyMenuList.Keys) {
            if(recipe.recipeCookType != CookManager.instance.currentCookType) continue;
            GameObject recipeObject = Instantiate(recipePrefab, Vector3.zero, Quaternion.identity);
            recipeObject.transform.SetParent(recipeParent.transform);

            SelectRecipeSlot selectRecipe = recipeObject.GetComponent<SelectRecipeSlot>();
            selectRecipe.RecipeUISetup(recipe, ingredientInventory);
            selectRecipe.OnSelectRecipe += RecipeUIHide;
        }
    }

}
