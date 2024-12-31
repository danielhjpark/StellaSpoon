using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInventory : MonoBehaviour
{
    IngredientSlot[] ingredientSlots;

    void Awake()
    {
        ingredientSlots = GetComponentsInChildren<IngredientSlot>();
    }


    public void IngredientAdd(Recipe recipe) {
        foreach(IngredientAmount ingredient in recipe.ingredients) {
            foreach(IngredientSlot ingredientSlot in ingredientSlots) {
                if(ingredientSlot.IsEmpty()) {
                    ingredientSlot.BindingIngredient(ingredient.ingredient);
                    break;
                }
            }
        }
    }

    public void IngredientSlotClear() {
        foreach(IngredientSlot ingredientSlot in ingredientSlots) {
            ingredientSlot.SlotClear();
        }
    }
}
