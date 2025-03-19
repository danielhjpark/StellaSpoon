using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class MakeRecipeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]Image slotImage;
    private Ingredient ingredient;
    bool isEnter;
    private Color initColor;
    private Color disableColor;
    private IngredientInventory ingredientInventory;
    public event Action OnSelectRecipe;

    void Start()
    {
        initColor = Color.white;
        disableColor = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            if(isEnter && IsHaveIngredient()) {

                ingredientInventory.IngredientAdd(ingredient);
            }
        }
    }

    bool IsHaveIngredient() {
        return false;
    }

    public void SlotUISetup(Ingredient ingredient) {
        
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
