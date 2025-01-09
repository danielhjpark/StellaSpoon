using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientSlot : MonoBehaviour
{
     bool isEmpty;
    [SerializeField] Image ingredientImage;
    [SerializeField] TextMeshProUGUI ingredientName;
    public Ingredient currentIngredient{get; set;}

    void Start()
    {
        isEmpty = true;
    }

    public void AddIngredient(Ingredient ingredient) {
        currentIngredient = ingredient;
        ingredientImage.sprite = ingredient.ingredientImage;
        ingredientName.text = ingredient.ingredientName;
        isEmpty = false;
    }

    public void BindingIngredient(Ingredient ingredient) {
        currentIngredient = ingredient;
        ingredientImage.sprite = ingredient.ingredientImage;
        ingredientName.text = ingredient.ingredientName;
        isEmpty = false;
    }

    public Ingredient GetIngredient() {
        return currentIngredient;
    }

    public bool IsEmpty() {
        return isEmpty;
    }

    public void SlotClear() {
        ingredientImage.sprite = null;
        ingredientName.text = null;
        isEmpty = true;
    }
}
