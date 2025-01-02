using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RefrigeratorInventory : Inventory
{
    RefrigeratorSlot[] refrigeratorSlots;
    List<Item> items;
    void Start()
    {
        refrigeratorSlots = this.GetComponentsInChildren<RefrigeratorSlot>();
        items = new List<Item>();
        foreach (RefrigeratorSlot slot in refrigeratorSlots) {
            slot.OnSlotUpdate += SlotUpate;
        }
    }

    public void SlotUpate() {
        foreach(Ingredient ingredient in IngredientManager.IngredientAmount.Keys.ToList()) {
             IngredientManager.IngredientAmount[ingredient] = 0;
        }

        foreach (RefrigeratorSlot slot in refrigeratorSlots) {
            if(slot.item != null) {
                string ingredient = slot.item.itemName;
                int ingredientAmount = slot.itemCount;
                Ingredient currentIngredient = IngredientManager.instance.FindIngredient(ingredient);
                IngredientManager.IngredientAmount[currentIngredient] += ingredientAmount;
            }
        }
    }

    public void UseIngredient(Ingredient currentIngredient, int count) {
        int currentCount = count;
        foreach (RefrigeratorSlot slot in refrigeratorSlots) {
            if(slot.item != null) {
                if(slot.currentIngredient == currentIngredient) {
                    if(slot.itemCount < currentCount) {
                        currentCount -= slot.itemCount;
                        slot.UseItem(slot.itemCount);
                    }
                    else {
                        slot.UseItem(currentCount);
                        if(IngredientManager.IngredientAmount[currentIngredient] <= 0) items.Add(slot.previousItem);
                        return;
                    }
                }
            }
        }
    }

    public void RecallIngredient(Ingredient currentIngredient, int Count) {
        foreach (RefrigeratorSlot slot in refrigeratorSlots) {
            if(slot.currentIngredient == currentIngredient) {
                slot.RecallItem(Count);
                return;
            }
        }

        foreach(Item item in items) {
            if(item.itemName == currentIngredient.ingredientName) {
                foreach(RefrigeratorSlot slot in refrigeratorSlots) {
                    if(slot.item == null) {
                        slot.previousItem = item;
                        slot.RecallItem(Count);
                        items.Remove(item);
                        return;
                    }
                }
            }
        }


    }


}
