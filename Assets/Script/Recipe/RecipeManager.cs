using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeManager : MonoBehaviour
{
    static public RecipeManager instance = null;

    Recipe[] RecipeList; //임시 데이터 베이스
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
            .Where(Recipe => Recipe.Value) // bool 값이 true인 항목 필터링
            .Select(Recipe => Recipe.Key) // Recipe만 추출
            .ToList();

        foreach (Recipe recipe in unlockedRecipes) {
        }

        return unlockedRecipes;
    }

    //----00------메뉴 생성 여부확인 ------------------------//
    public bool IsCanMakeMenu(Recipe recipe) {
        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount;
            int currentIngredientAmount = IngredientManager.instance.IngredientAmount[currentIngdeient];
            if(currentIngredientAmount < requireIngredientAmount) {
                return false;
            }
        }
        return true;
    }

    //----------------재료 사용하기------------------------//
    public void UseIngredientFromRecipe(Recipe recipe) {
        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
             int requireIngredientAmount = currentIngredient.amount;
            IngredientManager.instance.IngredientAmount[currentIngdeient] -= requireIngredientAmount;
        }
    }
    //-----------------재료 반환하기 ----------------------//
        public void RecallIngredientFromRecipe(Recipe recipe) {
        foreach(IngredientAmount currentIngredient in recipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
             int requireIngredientAmount = currentIngredient.amount;
            IngredientManager.instance.IngredientAmount[currentIngdeient] += requireIngredientAmount;
        }
    }
}
