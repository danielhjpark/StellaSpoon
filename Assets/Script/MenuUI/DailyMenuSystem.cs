using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DailyMenuSystem : MonoBehaviour
{
    [NonSerialized] public Dictionary<Recipe, int> dailyMenuList;

    //-----------------------UI Object ----------------------//
    
    [SerializeField] GameObject MenuPanel;
    [SerializeField] DetailUI detailUI;
    

    void Start(){
        DailyMenuInit();
    }

    //-------------------Button----------------------//
    public void AddMenu() {
        Recipe currentRecipe = detailUI.currentRecipe;
        if(RecipeManager.instance.IsCanMakeMenu(currentRecipe)) {
            RecipeManager.instance.UseIngredientFromRecipe(currentRecipe);
            DailyMenuUpdate(currentRecipe);
        }
        
    }

    //--------------Daily Menu Fuc -------------------//
    void DailyMenuUpdate(Recipe currentRecipe) {
        if(dailyMenuList.ContainsKey(currentRecipe)) {
            dailyMenuList[currentRecipe] += 1;
            return;
        }
        else {
            SelectMenuUI[] selectMenuUIs = MenuPanel.GetComponentsInChildren<SelectMenuUI>();
            foreach(SelectMenuUI selectMenuUI in selectMenuUIs) {
                if(selectMenuUI.IsCanAddMenu()) {
                    dailyMenuList.Add(currentRecipe, 1);
                    selectMenuUI.AddMenu(currentRecipe);
                    break;
                }

            }
        }

    }

    void DailyMenuInit() {
        dailyMenuList =  new Dictionary<Recipe, int>();
    }

}
