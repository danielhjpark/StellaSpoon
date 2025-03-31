using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeManager : MonoBehaviour
{
    static public RecipeManager instance = null;

    public Recipe[] RecipeList; //?ûÑ?ãú ?ç∞?ù¥?Ñ∞ Î≤†Ïù¥?ä§
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
            RecipeUnlockCheck.Add(recipe, false);
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
        if(IngredientManager.IngredientAmount[recipe.mainIngredient] <= 0) return false;
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
        if(IngredientManager.IngredientAmount[recipe.mainIngredient] * amount <= 0) return false;
        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount * amount;
            int currentIngredientAmount = IngredientManager.IngredientAmount[currentIngdeient];
            Debug.Log(currentIngredientAmount + " " + requireIngredientAmount);
            if(currentIngredientAmount  < requireIngredientAmount) {
                return false;
            }
        }
        return true;
    }

    //----------------?û¨Î£? ?Ç¨?ö©?ïòÍ∏?------------------------//
    public void UseIngredientFromRecipe(Recipe recipe, int amount) {
        IngredientManager.IngredientAmount[recipe.mainIngredient] -= amount;

        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount * amount;
            IngredientManager.IngredientAmount[currentIngdeient] -= requireIngredientAmount;
        }
    }
    //-----------------?û¨Î£? Î∞òÌôò?ïòÍ∏? ----------------------//
        public void RecallIngredientFromRecipe(Recipe recipe, int amount) {
        IngredientManager.IngredientAmount[recipe.mainIngredient] += amount;

        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
             int requireIngredientAmount = currentIngredient.amount * amount;
            IngredientManager.IngredientAmount[currentIngdeient] += requireIngredientAmount;
        }
    }

    //------------------Find Recipe for Main Ingredient------------------------//
    public Recipe FindRecipe(Ingredient ingredient)
    {
        Ingredient mainIngredient = ingredient;
        Recipe recipe = RecipeList.SingleOrDefault(recipe => recipe.mainIngredient == mainIngredient);
        return recipe;
    }

    public bool CompareRecipe(Recipe currentRecipe, List<IngredientAmount> checkIngredients)
    {
        List<IngredientAmount> currentIngredients = currentRecipe.ingredients;

        bool isCompare = currentIngredients.Count == checkIngredients.Count;
        int findCompareCount = 0;
        foreach (IngredientAmount currentIngredient in currentIngredients)
        {
            foreach (IngredientAmount checkIngredient in checkIngredients)
            {
                if (currentIngredient.ingredient.ingredientName == checkIngredient.ingredient.ingredientName)
                {
                    if (currentIngredient.amount == checkIngredient.amount)
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
