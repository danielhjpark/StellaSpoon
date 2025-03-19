using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class SelectRecipeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Recipe currentRecipe;
    Image image;
    bool isEnter;
    private Color initColor;
    private Color disableColor;
    private IngredientInventory ingredientInventory;
    public event Action OnSelectRecipe;

    void Start()
    {
        image = GetComponent<Image>();
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
                ingredientInventory.gameObject.SetActive(true);
                ingredientInventory.IngredientSlotClear();
                ingredientInventory.IngredientAdd(currentRecipe);
                CookManager.instance.SelectRecipe(currentRecipe);

                //Hide Panel
                this.transform.parent.gameObject.SetActive(false);
                OnSelectRecipe?.Invoke();

                //DailyMenu Count decrease
                DailyMenuManager.instance.DailyMenuRemove(currentRecipe);
                OrderManager.instance.UpdateMenu();
            }
        }
    }

    public void RecipeUISetup(Recipe recipe, IngredientInventory ingredientInventory)
    {
        this.ingredientInventory = ingredientInventory;
        currentRecipe = recipe;
        image.sprite = recipe.menuImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
        image.color = disableColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
        image.color = initColor;
    }
}
