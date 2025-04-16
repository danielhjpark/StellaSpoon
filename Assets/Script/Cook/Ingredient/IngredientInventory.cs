using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInventory : MonoBehaviour
{
    RefrigeratorInventory refrigeratorInventory;
    RefrigeratorSlot[] refrigeratorSlots;
    IngredientSlot[] ingredientSlots;
    [SerializeField] GameObject ingredientSlotPrefab;
    [SerializeField] GameObject ingredientSlotParent;
    void Awake()
    {
        refrigeratorInventory = RefrigeratorManager.instance.BindInventory();
        refrigeratorSlots = refrigeratorInventory.refrigeratorSlots;
        ingredientSlots = GetComponentsInChildren<IngredientSlot>();
        foreach(IngredientSlot ingredientSlot in ingredientSlots) {
            ingredientSlot.refrigeratorInventory = refrigeratorInventory;
        }
    }


    void CreateIngredientSlot(Ingredient ingredient, int ingredientCount) {
        GameObject ingredientslot = Instantiate(ingredientSlotPrefab);
        ingredientslot.transform.SetParent(ingredientSlotParent.transform);
        ingredientslot.GetComponent<IngredientSlot>().refrigeratorInventory = refrigeratorInventory;
        ingredientslot.GetComponent<IngredientSlot>().BindingIngredient(ingredient);
        ingredientslot.GetComponent<IngredientSlot>().itemCount = ingredientCount;
        
    }

    // public void AddAllIngredients() {
    //     IngredientSlotClear();
    //     foreach(RefrigeratorSlot refrigeratorSlot in refrigeratorSlots) {
    //         if(refrigeratorSlot.item == null) continue;
    //         foreach(IngredientSlot ingredientSlot in ingredientSlots) {
    //             if(ingredientSlot.IsEmpty()) {
    //                 ingredientSlot.BindingIngredient(refrigeratorSlot.currentIngredient);
    //                 ingredientSlot.itemCount = refrigeratorSlot.itemCount;
    //                 break;
    //             }
    //         }
    //     }
    //     IngredientSlotEmpty();
    // }

    public void AddAllIngredients() {
        IngredientSlotClear();
        foreach(RefrigeratorSlot refrigeratorSlot in refrigeratorSlots) {
            if(refrigeratorSlot.item == null) continue;
            CreateIngredientSlot(refrigeratorSlot.currentIngredient,refrigeratorSlot.itemCount);
        }
        IngredientSlotEmpty();
    }


    public void AddAllIngredientsToRecipe(Recipe recipe) {
        CreateIngredientSlot(recipe.mainIngredient, 1);
        foreach(IngredientAmount ingredient in recipe.ingredients) {
            if(ingredient.ingredient.ingredientUseCount > 0)
                CreateIngredientSlot(ingredient.ingredient, ingredient.amount/ingredient.ingredient.ingredientUseCount);
            else {
                CreateIngredientSlot(ingredient.ingredient, ingredient.amount);
            }
        }
        IngredientSlotEmpty();
    }

    public void AddMainIngredients() {
        IngredientSlotClear();
        foreach(RefrigeratorSlot refrigeratorSlot in refrigeratorSlots) {
            if(refrigeratorSlot.item == null) continue;
            else if(refrigeratorSlot.currentIngredient.ingredientType != IngredientType.Main) continue;
            
            foreach(IngredientSlot ingredientSlot in ingredientSlots) {
                if(ingredientSlot.IsEmpty()) {
                    ingredientSlot.BindingIngredient(refrigeratorSlot.currentIngredient);
                    ingredientSlot.itemCount = refrigeratorSlot.itemCount;
                    break;
                }
            }
        }
        IngredientSlotEmpty();
    }

    public void AddSubIngredients() {
        IngredientSlotClear();
        foreach(RefrigeratorSlot refrigeratorSlot in refrigeratorSlots) {
            if(refrigeratorSlot.item == null) continue;
            if(refrigeratorSlot.currentIngredient.ingredientType != IngredientType.Sub) continue;
            foreach(IngredientSlot ingredientSlot in ingredientSlots) {
                if(ingredientSlot.IsEmpty()) {
                    ingredientSlot.BindingIngredient(refrigeratorSlot.currentIngredient);
                    ingredientSlot.itemCount = refrigeratorSlot.itemCount;
                    break;
                }
            }
        }
        IngredientSlotEmpty();
    }

    public void IngredientAdd(Recipe recipe) {
        foreach(IngredientAmount ingredient in recipe.ingredients) {
            if(ingredient.ingredient.ingredientUseCount > 0)
                CreateIngredientSlot(ingredient.ingredient, ingredient.amount/ingredient.ingredient.ingredientUseCount);
            else {
                CreateIngredientSlot(ingredient.ingredient, ingredient.amount);
            }
        }
    }

    public void IngredientAdd(Ingredient ingredient) {
        CreateIngredientSlot(ingredient, 1);
    }

    public void IngredientSlotClear() {
        foreach(IngredientSlot ingredientSlot in ingredientSlots) {
            ingredientSlot.SlotClear();
            ingredientSlot.gameObject.SetActive(true);
        }
    }

    private void IngredientSlotEmpty() {
        foreach(IngredientSlot ingredientSlot in ingredientSlots) {
            if(ingredientSlot.IsEmpty()) {
                ingredientSlot.gameObject.SetActive(false);
            }
        }
    }
}
