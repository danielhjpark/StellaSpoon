using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TrimIngredientSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Image slotImage;
    protected Ingredient ingredient;
    protected bool isEnter;
    protected Color initColor;
    protected Color disableColor;
    protected IngredientInventory ingredientInventory;
    public Recipe currentRecipe;
    public event Action OnSelectIngredient;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isEnter)
            {
                ingredientInventory.IngredientAdd(ingredient);
                CookManager.instance.SelectRecipe(currentRecipe);
                // GameObject ingredientObject = Instantiate(ingredient.ingredientPrefab, Input.mousePosition, ingredient.ingredientPrefab.transform.rotation);
                // ingredientObject.transform.SetParent(CookManager.instance.spawnPoint, false);
                // CookManager.instance.DropObject(ingredientObject, ingredient);

                OnSelectIngredient?.Invoke();

            }
        }
    }
    public void SlotUISetup(Recipe recipe, IngredientInventory ingredientInventory)
    {
        initColor = Color.white;
        disableColor = Color.grey;
        //slotImage = GetComponent<Image>();
        slotImage = this.transform.GetChild(0).GetComponent<Image>();

        this.ingredient = recipe.mainIngredient;
        this.ingredientInventory = ingredientInventory;
        this.currentRecipe = recipe;
        slotImage.sprite = ingredient.ingredientImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
        slotImage.color = disableColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
        slotImage.color = initColor;
    }
}
