using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;

public class DailyMenuManager : MonoBehaviour
{
    static public DailyMenuManager instance;
    [NonSerialized] static public Dictionary<Recipe, int> dailyMenuList = new Dictionary<Recipe, int>();

    //-----------------------UI Object ----------------------//
    [Header("UI Object")]
    [SerializeField] GameObject dailyMenuPanel;
    [SerializeField] GameObject recipePanel;
    [SerializeField] DetailMenuUI detailMenuUI;
    [SerializeField] GameObject recipePrefab;


    RecipeUnLockSystem recipeUnLockSystem;

    void Awake()
    {
        instance = this;
        recipeUnLockSystem = GetComponent<RecipeUnLockSystem>();
    }

    private void OnEnable()
    {
        //MakeRecipeList();
        recipeUnLockSystem.RecipeListUpdate();
    }


    //-------------------Button----------------------//
    public void AddAmount()
    {
        SoundManager.instance.PlaySound(SoundManager.DailyMenu.Button_);
        detailMenuUI.AddAmount();
    }

    public void RemoveAmount()
    {
        SoundManager.instance.PlaySound(SoundManager.DailyMenu.Button_);
        detailMenuUI.RemoveAmount();
    }

    public void AddMenu()
    {
        SoundManager.instance.PlaySound(SoundManager.DailyMenu.Daily_Menu_Button);
        Recipe currentRecipe = detailMenuUI.currentRecipe;
        int currentAmount = detailMenuUI.currentAmount;
        if (RecipeManager.instance.IsCanMakeMenu(currentRecipe))
        {
            RecipeManager.instance.UseIngredientFromRecipe(currentRecipe, currentAmount);
            RefrigeratorManager.instance.UseIngredientToInventory(currentRecipe, currentAmount);
            this.DailyMenuAdd(currentRecipe, currentAmount);
            detailMenuUI.DetailUIClear();
        }
        else
        {
            InteractUIManger.instance.UsingText(InteractUIManger.TextType.FailAddMenu);
        }
    }

    //--------------Daily Menu Fuc -------------------//
    public void DailyMenuRemove(Recipe currentRecipe)
    {
        if (dailyMenuList.ContainsKey(currentRecipe))
        {
            dailyMenuList[currentRecipe] -= 1;
            if (dailyMenuList[currentRecipe] <= 0)
            {
                dailyMenuList.Remove(currentRecipe);
            }
            //DailyMenuUpdate();
            return;
        }
    }

    void DailyMenuAdd(Recipe currentRecipe, int currentAmount)
    {
        DailyMenuUI[] dailyMenuUIs = dailyMenuPanel.GetComponentsInChildren<DailyMenuUI>();
        DailyMenuUI targetUI = dailyMenuUIs.FirstOrDefault(menu => menu.currentMenu == currentRecipe);

        if (dailyMenuList.ContainsKey(currentRecipe))
        {
            dailyMenuList[currentRecipe] += currentAmount;
            targetUI.AddMenu(currentRecipe, currentAmount);
            return;
        }
        else
        {
            foreach (DailyMenuUI dailyMenuUI in dailyMenuUIs)
            {
                if (dailyMenuUI.IsCanAddMenu())
                {
                    dailyMenuList.Add(currentRecipe, currentAmount);
                    dailyMenuUI.AddMenu(currentRecipe, currentAmount);
                    break;
                }
            }
        }

    }

    public void DailyMenuReset()
    {
        if (dailyMenuList.Count > 0)
        {
            foreach (Recipe recipe in dailyMenuList.Keys)dailyMenuList[recipe] = 0;
        }
        DailyMenuUI[] dailyMenuUIs = dailyMenuPanel.GetComponentsInChildren<DailyMenuUI>();
        foreach (DailyMenuUI dailyMenu in dailyMenuUIs)
        {
            dailyMenu.RemoveMenu();
        }
    }

    //---------------MenuList Create -----------------//

    void MakeRecipeList()
    {
        List<Recipe> recipeList = RecipeManager.instance.MakeRecipeUnLockList();

        foreach (Recipe recipe in recipeList)
        {
            GameObject menu = Instantiate(recipePrefab, Vector3.zero, Quaternion.identity);
            menu.transform.SetParent(recipePanel.transform);
            menu.GetComponent<MenuSlot>().MenuUISetup(recipe);
        }
        RectTransform recipePanelRect = recipePanel.GetComponent<RectTransform>();
        int rectWidth = 400, rectHeight = recipeList.Count * 120;//size + spacing
        recipePanelRect.sizeDelta = new Vector2(rectWidth, rectHeight);
        recipePanelRect.pivot = new Vector2(0.5f, 1);
    }


    public void DetailUIUpdate(Recipe currentRecipe, int Amount)
    {
        detailMenuUI.DetailUpdate(currentRecipe, Amount);
    }

}

