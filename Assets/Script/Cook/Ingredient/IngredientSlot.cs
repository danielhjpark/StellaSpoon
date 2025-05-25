using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class IngredientSlot : Slot
{
    bool isEmpty;
    public RefrigeratorInventory refrigeratorInventory;
    public SlotToolTip slotToolTip;
    [SerializeField] Image ingredientImage;
    [SerializeField] TextMeshProUGUI ingredientName;
    public Ingredient currentIngredient { get; set; }

    void Start()
    {
        isEmpty = true;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        currentIngredient = ingredient;
        ingredientImage.sprite = ingredient.ingredientImage;
        //ingredientName.text = ingredient.ingredientName;
        isEmpty = false;
    }

    public void BindingIngredient(Ingredient ingredient)
    {
        currentIngredient = ingredient;
        ingredientImage.sprite = ingredient.ingredientImage;
        //ingredientName.text = ingredient.ingredientName;
        isEmpty = false;
        slotToolTip.HideToolTip();
    }

    public Ingredient GetIngredient()
    {
        return currentIngredient;
    }

    public bool IsEmpty()
    {
        return isEmpty;
    }

    public void SlotClear()
    {
        this.gameObject.transform.SetAsLastSibling();
        currentIngredient = null;
        ingredientImage.sprite = null;
        //ingredientName.text = null;
        isEmpty = true;
    }
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        slotToolTip.ShowToolTip(currentIngredient.ingredientText, transform.position);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        slotToolTip.HideToolTip();
    }
    //Hide parent func
    public override void OnEndDrag(PointerEventData eventData) {}
    
}
