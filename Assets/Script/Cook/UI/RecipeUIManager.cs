using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUIManager : MonoBehaviour
{
    [SerializeField] RectTransform recipePanel;
    [SerializeField] GameObject recipeParent;
    [SerializeField] GameObject recipePrefab;
    [SerializeField] IngredientInventory ingredientInventory;

    Vector2 recipePanelInitSize = new Vector2(2000, 200);

    void Start()
    {
        
    }

    public void RecipeListInit() {
        recipeParent.SetActive(true);   
        foreach (Transform child in recipeParent.transform)
        {
            Destroy(child.gameObject);//ObjectPool 대체 가능성
        }
        ingredientInventory.IngredientSlotClear();
        
    }

    public void RecipeListSetup() {
        int recipeCount = DailyMenuManager.dailyMenuList.Count;
        if(recipeCount <= 0) return;
        RecipeListInit();
        foreach (Recipe recipe in DailyMenuManager.dailyMenuList.Keys) {
            GameObject recipeObject = Instantiate(recipePrefab, Vector3.zero, Quaternion.identity);
            recipeObject.transform.SetParent(recipeParent.transform);
            recipeObject.GetComponent<SelectRecipeUI>().RecipeUISetup(recipe, ingredientInventory);
        }
    }

}
