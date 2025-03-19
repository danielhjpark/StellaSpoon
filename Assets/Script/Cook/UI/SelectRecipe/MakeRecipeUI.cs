using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MakeRecipeUI : MonoBehaviour
{
    [SerializeField] RefrigeratorInventory refrigeratorInventory;
    [SerializeField] GameObject[] ingredientParents;
    [SerializeField] GameObject slotPrefab;

    [SerializeField] GameObject mainIngredientUI;
    [SerializeField] GameObject subIngredientUI;
    [SerializeField] GameObject sauceIngredientUI;

    [SerializeField] Ingredient ingredient;

    void Start() {
        CombineIngredient(ingredient);
    }

    void Update()
    {
        
    }


    public void MainIngredientList() {

        foreach (Ingredient ingredient in  IngredientManager.IngredientAmount.Keys)
        {
            GameObject slotObject = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
            switch(ingredient.ingredientType) {
                case IngredientType.Main:
                    slotObject.transform.SetParent(mainIngredientUI.transform);
                    break;
                case IngredientType.Sub:
                    slotObject.transform.SetParent(subIngredientUI.transform);
                    break;
                case IngredientType.Sauce:
                    slotObject.transform.SetParent(sauceIngredientUI.transform);
                    break;
            }
            
            MakeRecipeSlot makeRecipeSlot = slotObject.GetComponent<MakeRecipeSlot>();
            makeRecipeSlot.SlotUISetup(ingredient);
            //selectRecipe.OnSelectRecipe += RecipeUpdate;
        }
        //ingredientInventory.IngredientSlotClear();
    }

    public void NextUI() {
        
    }

    public void CheckCanMakeNewRecipe() {
        bool isCanMakeNewRecipe = true;
    
        if(isCanMakeNewRecipe) {

        }
        else {

        }
    }

    List<Recipe> CombineIngredient(Ingredient ingredient) {
        Ingredient mainIngredient = ingredient;
        List<Recipe> recipe = RecipeManager.instance.RecipeList.
            Where(recipe =>recipe.ingredients.
            Any(target => target.ingredient == mainIngredient)).ToList();
        Debug.Log(recipe[0].menuName);

        return recipe;
    }

}
