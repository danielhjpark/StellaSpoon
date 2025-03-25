using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CookManagerBase : MonoBehaviour
{
    protected Recipe currentMenu;
    protected Recipe targetRecipe;

    public virtual void SelectRecipe(Recipe menu)
    {
        currentMenu = menu;
    }
    public abstract void CookCompleteCheck();
    public abstract IEnumerator UseCookingStep();
    public abstract void AddIngredient(GameObject obj, Ingredient ingredient);

    protected Recipe FindRecipe(Ingredient ingredient)
    {
        // Ingredient mainIngredient = ingredient;
        // List<Recipe> recipe = RecipeManager.instance.RecipeList.
        //     Where(recipe => recipe.ingredients.
        //     Any(target => target.ingredient == mainIngredient)).ToList();

        Ingredient mainIngredient = ingredient;
        Recipe recipe = RecipeManager.instance.RecipeList.SingleOrDefault(recipe => recipe.mainIngredient == mainIngredient);

        Debug.Log(recipe.menuName);

        return recipe;
    }

    public bool CompareIngredient(List<IngredientAmount> currentIngredients, List<IngredientAmount> targetIngredients)
    {
        bool isCompare = currentIngredients.Count == targetIngredients.Count;
        int findCompareCount = 0;

        foreach (IngredientAmount currentIngredient in currentIngredients)
        {
            foreach (IngredientAmount targetIngredient in targetIngredients)
            {
                if (currentIngredient.ingredient.ingredientName == targetIngredient.ingredient.ingredientName)
                {
                    if (currentIngredient.amount == targetIngredient.amount)
                    {
                        findCompareCount++;
                        break;
                    }
                }
            }
        }
        if (isCompare) return currentIngredients.Count == findCompareCount;
        else return isCompare;
    }

}
