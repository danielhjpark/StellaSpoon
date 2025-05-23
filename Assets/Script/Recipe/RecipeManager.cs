using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeManager : MonoBehaviour
{
    static public RecipeManager instance = null;

    [SerializeField] Recipe[] BasicRecipes;
    public Dictionary<string, Recipe> RecipeList; //?ûÑ?ãú ?ç∞?ù¥?Ñ∞ Î≤†Ïù¥?ä§
    public Dictionary<Recipe, bool> RecipeUnlockCheck;
    private GameObject NewRecipeUI;
    private DeviceRecipeUI deviceRecipeUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this) Destroy(this.gameObject);
        }

        Transform canvasTransform = GameObject.Find("Canvas")?.transform; // Canvas∏¶ √£±‚
        NewRecipeUI = canvasTransform.Find("NewRecipePanel")?.gameObject; // MapPanel¿ª √£±‚
        deviceRecipeUI = canvasTransform.Find("PARENT_Recipe(DeactivateThis)").gameObject.GetComponent<DeviceRecipeUI>();
        RecipeUnLockInit();
    }

    void RecipeUnLockInit()
    {
        RecipeList = new Dictionary<string, Recipe>();
        RecipeUnlockCheck = new Dictionary<Recipe, bool>();
        Recipe[] RecipeScriptable = Resources.LoadAll<Recipe>("Scriptable/Recipe");

        foreach (Recipe recipe in RecipeScriptable)
        {
            RecipeList.Add(recipe.name, recipe);
            //RecipeUnlockCheck.Add(recipe, false);
            RecipeUnlockCheck.Add(recipe, true);
        }

        foreach (Recipe BasicRecipe in BasicRecipes)
        {
            RecipeUnlockCheck[BasicRecipe] = true;
        }
    }
    //--------------- RecipeUnLock System -----------------//
    public void RecipeUnLock(Recipe getRecipe)
    {
        if (getRecipe != null && !RecipeUnlockCheck[getRecipe])
        {
            RecipeUnlockCheck[getRecipe] = true;
            Debug.Log("RecipeUnLock : " + getRecipe.name);
            RecipeUnLockUI();
        }
    }

    public void RecipeUnLockUI()
    {
        NewRecipeUI.SetActive(true);
        StartCoroutine(RecipeUnLockFade());
        UpdateRecipeDevice();
    }

    IEnumerator RecipeUnLockFade()
    {
        NewRecipeUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        NewRecipeUI.SetActive(false);
    }

    void UpdateRecipeDevice()
    {
        deviceRecipeUI.UnlockRecipe();
    }

    //----- Get all of unlock recipelist -------------//
    public List<Recipe> MakeRecipeUnLockList()
    {
        List<Recipe> unlockedRecipes = RecipeUnlockCheck
            .Where(Recipe => Recipe.Value) // bool Í∞íÏù¥ true?ù∏ ?ï≠Î™? ?ïÑ?Ñ∞Îß?
            .Select(Recipe => Recipe.Key) // RecipeÎß? Ï∂îÏ∂ú
            .ToList();

        return unlockedRecipes;
    }

    //--------------Î©îÎâ¥ ?Éù?Ñ± ?ó¨Î∂??ôï?ù∏------------------------//
    //?ã®?àò
    public bool IsCanMakeMenu(Recipe recipe)
    {
        if (recipe == null) return false;
        if (IngredientManager.IngredientAmount[recipe.mainIngredient] <= 0) return false;
        foreach (IngredientAmount currentIngredient in recipe.ingredients)
        {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount;
            int currentIngredientAmount = IngredientManager.IngredientAmount[currentIngdeient];
            if (currentIngredientAmount < requireIngredientAmount)
            {
                return false;
            }
        }
        return true;
    }
    //Î≥µÏàò
    public bool IsCanMakeMenu(Recipe recipe, int amount)
    {
        if (recipe == null) return false;
        if (IngredientManager.IngredientAmount[recipe.mainIngredient] * amount <= 0) return false;
        foreach (IngredientAmount currentIngredient in recipe.ingredients)
        {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount * amount;
            int currentIngredientAmount = IngredientManager.IngredientAmount[currentIngdeient];
            Debug.Log(currentIngredientAmount + " " + requireIngredientAmount);
            if (currentIngredientAmount < requireIngredientAmount)
            {
                return false;
            }
        }
        return true;
    }

    //----------------?û¨Î£? ?Ç¨?ö©?ïòÍ∏?------------------------//
    public void UseIngredientFromRecipe(Recipe recipe, int amount)
    {
        IngredientManager.IngredientAmount[recipe.mainIngredient] -= amount;

        foreach (IngredientAmount currentIngredient in recipe.ingredients)
        {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount * amount;
            IngredientManager.IngredientAmount[currentIngdeient] -= requireIngredientAmount;
        }
    }

    //-----------------?û¨Î£? Î∞òÌôò?ïòÍ∏? ----------------------//
    public void RecallIngredientFromRecipe(Recipe recipe, int amount)
    {
        IngredientManager.IngredientAmount[recipe.mainIngredient] += amount;

        foreach (IngredientAmount currentIngredient in recipe.ingredients)
        {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int requireIngredientAmount = currentIngredient.amount * amount;
            IngredientManager.IngredientAmount[currentIngdeient] += requireIngredientAmount;
        }
    }

    //-----------------    Find Recipe   -----------------------//
    public Recipe FindRecipe(string recipeName)
    {
        //Recipe targetRecipe = RecipeList.Values.SingleOrDefault(recipe => recipe.name.Contains(recipeName));

        Recipe targetRecipe = RecipeList.Values.SingleOrDefault(recipe => recipeName.Contains(recipe.name));
        Debug.Log(targetRecipe);
        return targetRecipe;
    }

    public Recipe FindRecipe(Ingredient ingredient)
    {
        Ingredient mainIngredient = ingredient;
        Recipe targetRecipe = RecipeList.Values.SingleOrDefault(recipe => recipe.mainIngredient == mainIngredient);
        return targetRecipe;
    }

    //-------------Check compare recipe with ingredients ---------//
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
