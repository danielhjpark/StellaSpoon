using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DailyMenuManager : MonoBehaviour
{
    [NonSerialized] static public Dictionary<Recipe, int> dailyMenuList = new Dictionary<Recipe, int>();

    //-----------------------UI Object ----------------------//
    
    [SerializeField] GameObject dailyMenuPanel;
    [SerializeField] GameObject recipePanel;
    [SerializeField] DetailMenuUI detailMenuUI;
    
    //----------------------------------------------------//
    [SerializeField] GameObject recipePrefab;

    void Start(){
        DailyMenuInit();
        MakeRecipeList();
    }

    void DailyMenuInit() {
        dailyMenuList =  new Dictionary<Recipe, int>();
    }

    //-------------------Button----------------------//
    public void AddAmount() {
        detailMenuUI.AddAmount();
    }

    public void RemoveAmount() {
        detailMenuUI.RemoveAmount();
    }
    
    public void AddMenu() {
        Recipe currentRecipe = detailMenuUI.currentRecipe;
        int currentAmount = detailMenuUI.currentAmount;
        if(RecipeManager.instance.IsCanMakeMenu(currentRecipe)) {
            RecipeManager.instance.UseIngredientFromRecipe(currentRecipe, currentAmount);
            RefrigeratorManager.instance.UseIngredientToInventory(currentRecipe, currentAmount);
            DailyMenuUpdate(currentRecipe, currentAmount);
            detailMenuUI.DetailUIClear();
        }     
    }

    //--------------Daily Menu Fuc -------------------//
    void DailyMenuUpdate(Recipe currentRecipe, int currentAmount) {
        if(dailyMenuList.ContainsKey(currentRecipe)) {
            dailyMenuList[currentRecipe] += 1;
            return;
        }
        else {
            DailyMenuUI[] dailyMenuUIs = dailyMenuPanel.GetComponentsInChildren<DailyMenuUI>();
            foreach(DailyMenuUI dailyMenuUI in dailyMenuUIs) {
                if(dailyMenuUI.IsCanAddMenu()) {
                    dailyMenuList.Add(currentRecipe, currentAmount);
                    dailyMenuUI.AddMenu(currentRecipe, currentAmount);
                    break;
                }
            }
        }
    }
    //---------------MenuList Create -----------------//

    void MakeRecipeList() {
        List<Recipe> recipeList = RecipeManager.instance.MakeRecipeUnLockList();

        foreach (Recipe recipe in recipeList) {
            GameObject menu = Instantiate(recipePrefab, Vector3.zero, Quaternion.identity);
            menu.transform.SetParent(recipePanel.transform);
            menu.GetComponent<MenuSlot>().MenuUISetup(recipe);
        }
    }

    public void DetailUIUpdate(Recipe currentRecipe, int Amount) {
        detailMenuUI.DetailUpdate(currentRecipe, Amount);
    }
}
