using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class SelectRecipeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Recipe currentRecipe;
    [SerializeField]Image image;
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
            if(isEnter) {
                ingredientInventory.IngredientSlotClear();
                ingredientInventory.IngredientAdd(currentRecipe);
                CookManager.instance.SelectRecipe(currentRecipe);
                this.transform.parent.gameObject.SetActive(false);
                OnSelectRecipe?.Invoke();
            }
        }
    }

    public void RecipeUISetup(Recipe recipe, IngredientInventory ingredientInventory) {
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
