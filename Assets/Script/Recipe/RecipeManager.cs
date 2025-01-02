using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeManager : MonoBehaviour
{
    static public RecipeManager instance = null;

    Recipe[] RecipeList; //?ûÑ?ãú ?ç∞?ù¥?Ñ∞ Î≤†Ïù¥?ä§
    public Dictionary<Recipe, bool> RecipeUnlockCheck;
    
    void Awake()
    {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }

        RecipeList = Resources.LoadAll<Recipe>("Scriptable/Recipe");
        RecipeUnlockCheck = new Dictionary<Recipe, bool>();
        RecipeUnlockInit();
        //MakeRecipeUnLockList();
    }

    void RecipeUnlockInit() {
        foreach (Recipe recipe in RecipeList) {
            RecipeUnlockCheck.Add(recipe, true);
        }
    }

    public void RecipeUnLock(Recipe getRecipe) {
        RecipeUnlockCheck[getRecipe] = true;
    }

    public List<Recipe> MakeRecipeUnLockList() {
        List<Recipe> unlockedRecipes = RecipeUnlockCheck
            .Where(Recipe => Recipe.Value) // bool Í∞íÏù¥ true?ù∏ ?ï≠Î™? ?ïÑ?Ñ∞Îß?
            .Select(Recipe => Recipe.Key) // RecipeÎß? Ï∂îÏ∂ú
            .ToList();

        foreach (Recipe recipe in unlockedRecipes) {
        }

        return unlockedRecipes;
    }

    //--------------Î©îÎâ¥ ?Éù?Ñ± ?ó¨Î∂??ôï?ù∏------------------------//
    //?ã®?àò
    public bool IsCanMakeMenu(Recipe recipe) {
        if(recipe == null) return false;
        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount;
            int currentIngredientAmount = IngredientManager.IngredientAmount[currentIngdeient];
            if(currentIngredientAmount < requireIngredientAmount) {
                return false;
            }
        }
        return true;
    }
    //Î≥µÏàò
    public bool IsCanMakeMenu(Recipe recipe, int amount) {
        if(recipe == null) return false;
        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount * amount;
            int currentIngredientAmount = IngredientManager.IngredientAmount[currentIngdeient];
            if(currentIngredientAmount  < requireIngredientAmount) {
                return false;
            }
        }
        return true;
    }

    //----------------?û¨Î£? ?Ç¨?ö©?ïòÍ∏?------------------------//
    public void UseIngredientFromRecipe(Recipe recipe, int amount) {
        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount * amount;
            IngredientManager.IngredientAmount[currentIngdeient] -= requireIngredientAmount;
        }
    }
    //-----------------?û¨Î£? Î∞òÌôò?ïòÍ∏? ----------------------//
        public void RecallIngredientFromRecipe(Recipe recipe, int amount) {
        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
             int requireIngredientAmount = currentIngredient.amount * amount;
            IngredientManager.IngredientAmount[currentIngdeient] += requireIngredientAmount;
        }
    }
}
