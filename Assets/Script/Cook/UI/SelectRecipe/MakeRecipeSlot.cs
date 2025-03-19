using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class MakeRecipeSlot : MonoBehaviour
{
    protected Image slotImage;
    protected Ingredient ingredient;
    protected bool isEnter;
    protected Color initColor;
    protected Color disableColor;
    protected IngredientInventory ingredientInventory;
    public event Action OnAddedIngredient;

    void Start()
    {

    }

    bool IsHaveIngredient()
    {
        return false;
    }

    public void SlotUISetup(Ingredient ingredient)
    {
        initColor = Color.white;
        disableColor = Color.grey;
        slotImage = GetComponent<Image>();
        slotImage.sprite = ingredient.ingredientImage;
    }


}
