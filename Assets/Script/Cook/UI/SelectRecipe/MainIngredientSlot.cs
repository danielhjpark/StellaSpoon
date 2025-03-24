using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MainIngredientSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Image slotImage;
    protected Ingredient ingredient;
    protected bool isEnter;
    protected Color initColor;
    protected Color disableColor;
    protected IngredientInventory ingredientInventory;
    public event Action OnAddedIngredient;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isEnter)
            {
                ingredientInventory.IngredientAdd(ingredient);
                //OnAddedIngredient?.Invoke();
                GameObject ingredientObject = Instantiate(ingredient.ingredientPrefab, Input.mousePosition, ingredient.ingredientPrefab.transform.rotation);
                ingredientObject.transform.SetParent(CookManager.instance.spawnPoint, false);
                CookManager.instance.DropObject(ingredientObject, ingredient);
            }
        }
    }
    public void SlotUISetup(Ingredient ingredient, IngredientInventory ingredientInventory)
    {
        initColor = Color.white;
        disableColor = Color.grey;
        slotImage = GetComponent<Image>();
        this.ingredient = ingredient;
        this.ingredientInventory = ingredientInventory;
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
