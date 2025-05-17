using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class SelectRecipeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Recipe currentRecipe;
    private Image recipeImage;
    private bool isEnter;
    private Color initColor;
    private Color disableColor;
    private IngredientInventory ingredientInventory;
    public event Action OnSelectRecipe;

    private void Awake() {
        recipeImage = GetComponent<Image>();
        initColor = Color.white;
        disableColor = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isEnter)
            {
                //ingredientInventory.gameObject.SetActive(true);
                ingredientInventory.IngredientSlotClear();
                ingredientInventory.IngredientAdd(currentRecipe);
                CookManager.instance.SelectRecipe(currentRecipe);

                //Hide Panel
                OnSelectRecipe?.Invoke();
                this.transform.parent.gameObject.SetActive(false);
                

                //DailyMenu Count decrease
                //DailyMenuManager.instance.DailyMenuRemove(currentRecipe);
            }
        }
    }

    public void RecipeUISetup(Recipe recipe, IngredientInventory ingredientInventory)
    {
        this.ingredientInventory = ingredientInventory;
        currentRecipe = recipe;
        Debug.Log(recipe.menuImage);
        recipeImage.sprite = recipe.menuImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
        recipeImage.color = disableColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
        recipeImage.color = initColor;
    }
}
