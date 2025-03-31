using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInventory : MonoBehaviour
{
    RefrigeratorInventory refrigeratorInventory;
    RefrigeratorSlot[] refrigeratorSlots;
    IngredientSlot[] ingredientSlots;
    [SerializeField] GameObject ingredientSlotPrefab;

    void Awake()
    {
        refrigeratorInventory = RefrigeratorManager.instance.BindInventory();
        refrigeratorSlots = refrigeratorInventory.refrigeratorSlots;
        ingredientSlots = GetComponentsInChildren<IngredientSlot>();
        foreach(IngredientSlot ingredientSlot in ingredientSlots) {
            ingredientSlot.refrigeratorInventory = refrigeratorInventory;
        }
    }


    void InitIngredientSlot() {
        int sloatCreateCount = refrigeratorSlots.Length;
        
    }

    public void AddAllIngredients() {
        IngredientSlotClear();
        foreach(RefrigeratorSlot refrigeratorSlot in refrigeratorSlots) {
            if(refrigeratorSlot.item == null) continue;
            foreach(IngredientSlot ingredientSlot in ingredientSlots) {
                Debug.Log(refrigeratorSlot.currentIngredient.ingredientName);
                if(ingredientSlot.IsEmpty()) {
                    ingredientSlot.BindingIngredient(refrigeratorSlot.currentIngredient);
                    ingredientSlot.itemCount = refrigeratorSlot.itemCount;
                    break;
                }
            }
        }
        IngredientSlotEmpty();
    }

    public void AddAllIngredientsToRecipe(Recipe recipe) {
        ingredientSlots[0].BindingIngredient(recipe.mainIngredient);
        foreach(IngredientAmount ingredient in recipe.ingredients) {
            foreach(IngredientSlot ingredientSlot in ingredientSlots) {
                if(ingredientSlot.IsEmpty()) {
                    ingredientSlot.BindingIngredient(ingredient.ingredient);
                    break;
                }
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
                Debug.Log(refrigeratorSlot.currentIngredient.ingredientName);
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
                Debug.Log(refrigeratorSlot.currentIngredient.ingredientName);
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
            foreach(IngredientSlot ingredientSlot in ingredientSlots) {
                if(ingredientSlot.IsEmpty()) {
                    ingredientSlot.BindingIngredient(ingredient.ingredient);
                    break;
                }
            }
        }
    }

    public void IngredientAdd(Ingredient ingredient) {
        foreach(IngredientSlot ingredientSlot in ingredientSlots) {
            if(ingredientSlot.IsEmpty()) {
                ingredientSlot.BindingIngredient(ingredient);
                break;
            }
        }
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
