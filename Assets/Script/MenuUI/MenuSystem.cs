using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] DetailUI detailUI;
    [SerializeField] GameObject menuPrefab;
    
    
    void Start()
    {
        MakeRecipeList();
    }

    void MakeRecipeList() {
        List<Recipe> recipeList = RecipeManager.instance.MakeRecipeUnLockList();

        foreach (Recipe recipe in recipeList) {
            GameObject menu = Instantiate(menuPrefab, Vector3.zero, Quaternion.identity);
            menu.transform.SetParent(this.transform);
            menu.GetComponent<MenuUI>().MenuUISetup(recipe);
        }
    }

    public void DetailUIUpdate(Recipe currentRecipe) {
        detailUI.DetailUpdate(currentRecipe);
    }
}
