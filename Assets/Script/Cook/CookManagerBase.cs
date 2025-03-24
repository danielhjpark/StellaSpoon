using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CookManagerBase : MonoBehaviour
{
    protected Recipe currentMenu;
    protected Recipe targetRecipe;

    public virtual void SelectRecipe(Recipe menu) {
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
}
