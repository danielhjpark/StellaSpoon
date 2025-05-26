using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeUnLockSystem : MonoBehaviour {
    [SerializeField] GameObject recipePanel;
    [SerializeField] GameObject recipePrefab;
    List<MenuSlot> menuSlots = new List<MenuSlot>();

    public void RecipeListUpdate() {
        List<Recipe> recipeList = RecipeManager.instance.MakeRecipeUnLockList();
        
        foreach (Recipe recipe in recipeList) {
            if(menuSlots.FirstOrDefault(menu => menu.currentRecipe == recipe)) continue;
            GameObject menu = Instantiate(recipePrefab, Vector3.zero, Quaternion.identity);
            menu.transform.SetParent(recipePanel.transform);

            MenuSlot menuSlot = menu.GetComponent<MenuSlot>();
            menuSlot.MenuUISetup(recipe);
            menuSlots.Add(menuSlot);
        }

        RectTransform recipePanelRect = recipePanel.GetComponent<RectTransform>();
        int rectWidth = 400, rectHeight = recipeList.Count * 120;//size + spacing
        recipePanelRect.sizeDelta = new Vector2(rectWidth, rectHeight);
        recipePanelRect.pivot = new Vector2(0.5f, 1);
    }

}