using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeManager : MonoBehaviour
{
    static public RecipeManager instance = null;

    Recipe[] RecipeList; //?��?�� ?��?��?�� 베이?��
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
            .Where(Recipe => Recipe.Value) // bool 값이 true?�� ?���? ?��?���?
            .Select(Recipe => Recipe.Key) // Recipe�? 추출
            .ToList();

        foreach (Recipe recipe in unlockedRecipes) {
        }

        return unlockedRecipes;
    }

    //--------------메뉴 ?��?�� ?���??��?��------------------------//
    //?��?��
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
    //복수
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

    //----------------?���? ?��?��?���?------------------------//
    public void UseIngredientFromRecipe(Recipe recipe, int amount) {
        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount * amount;
            IngredientManager.IngredientAmount[currentIngdeient] -= requireIngredientAmount;
        }
    }
    //-----------------?���? 반환?���? ----------------------//
        public void RecallIngredientFromRecipe(Recipe recipe, int amount) {
        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
             int requireIngredientAmount = currentIngredient.amount * amount;
            IngredientManager.IngredientAmount[currentIngdeient] += requireIngredientAmount;
        }
    }
}
