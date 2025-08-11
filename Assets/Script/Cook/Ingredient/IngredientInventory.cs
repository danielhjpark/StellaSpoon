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
    [SerializeField] SlotToolTip slotToolTip;
    Dictionary<Ingredient, bool> checkCreateIngredient = new Dictionary<Ingredient, bool>();

    void Awake()
    {
        refrigeratorInventory = RefrigeratorManager.instance.BindInventory();
        refrigeratorSlots = refrigeratorInventory.refrigeratorSlots;
        ingredientSlots = GetComponentsInChildren<IngredientSlot>();
        foreach(IngredientSlot ingredientSlot in ingredientSlots) {
            ingredientSlot.refrigeratorInventory = refrigeratorInventory;
        }
    }


    void CreateIngredientSlot(Ingredient ingredient, int ingredientCount)
    {
        if (!checkCreateIngredient.ContainsKey(ingredient))
            checkCreateIngredient.Add(ingredient, true);
        else if (!checkCreateIngredient[ingredient])
        {
            checkCreateIngredient[ingredient] = true;
        }
        else return;

        GameObject ingredientslot = Instantiate(ingredientSlotPrefab);
        ingredientslot.transform.SetParent(ingredientSlotParent.transform);
        ingredientslot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        ingredientslot.GetComponent<IngredientSlot>().refrigeratorInventory = refrigeratorInventory;
        ingredientslot.GetComponent<IngredientSlot>().slotToolTip = slotToolTip;
        ingredientslot.GetComponent<IngredientSlot>().BindingIngredient(ingredient);
        ingredientslot.GetComponent<IngredientSlot>().itemCount = ingredientCount;


    }

    // Pot Use This
    public void AddAllIngredients() {
        IngredientSlotClear();
        foreach(RefrigeratorSlot refrigeratorSlot in refrigeratorSlots) {
            if(refrigeratorSlot.item == null) continue;
            else if(refrigeratorSlot.currentIngredient.ingredientType == IngredientType.Trim) continue;
            CreateIngredientSlot(refrigeratorSlot.currentIngredient,refrigeratorSlot.itemCount);
        }
        IngredientSlotEmpty();
    }

    // Pot Use This
    public void AddAllIngredientsToRecipe(Recipe recipe) {
        CreateIngredientSlot(recipe.mainIngredient, 1);
        foreach(IngredientAmount ingredient in recipe.ingredients) {
            if(ingredient.ingredient.ingredientUseCount > 0)
                CreateIngredientSlot(ingredient.ingredient, ingredient.ingredient.ingredientUseCount);
            else {
                CreateIngredientSlot(ingredient.ingredient, ingredient.amount);
            }
        }
        IngredientSlotEmpty();
    }

    // Other Utensil this
    public void AddMainIngredients() {
        IngredientSlotInit();
        foreach(RefrigeratorSlot refrigeratorSlot in refrigeratorSlots) {
            if(refrigeratorSlot.item == null) continue;
            else if(refrigeratorSlot.currentIngredient.ingredientType != IngredientType.Main) continue;
            
            CreateIngredientSlot(refrigeratorSlot.currentIngredient,refrigeratorSlot.itemCount);
        }
        IngredientSlotEmpty();
    }

    public void AddSubIngredients() {
        IngredientSlotInit();
        foreach(RefrigeratorSlot refrigeratorSlot in refrigeratorSlots) {
            if(refrigeratorSlot.item == null) continue;
            else if(refrigeratorSlot.currentIngredient.ingredientType != IngredientType.Sub) continue;
            CreateIngredientSlot(refrigeratorSlot.currentIngredient,refrigeratorSlot.itemCount);
        }
        IngredientSlotEmpty();
    }
    
    public void AddTrimIngredients()
    {
        IngredientSlotInit();
        foreach (RefrigeratorSlot refrigeratorSlot in refrigeratorSlots)
        {
            if (refrigeratorSlot.item == null) continue;
            else if (refrigeratorSlot.currentIngredient.ingredientType != IngredientType.Trim) continue;
            CreateIngredientSlot(refrigeratorSlot.currentIngredient, refrigeratorSlot.itemCount);
        }
        IngredientSlotEmpty();
    }

    //--------------------------------------------------//

    public void IngredientAdd(Recipe recipe) {
        foreach (IngredientAmount ingredient in recipe.ingredients)
        {
            if (ingredient.ingredient.ingredientUseCount > 0)
                CreateIngredientSlot(ingredient.ingredient, ingredient.amount);
            else
            {
                CreateIngredientSlot(ingredient.ingredient, ingredient.amount);
            }
        }
    }

    public void IngredientAdd(Ingredient ingredient) {
        CreateIngredientSlot(ingredient, 1);
    }

    //-------------- Clear & Disable --------------------//
    public void IngredientSlotInit() {
        foreach (Transform ingredientSlot in ingredientSlotParent.transform)
        {
            if (ingredientSlot.GetComponent<IngredientSlot>().currentIngredient != null)
            {
                checkCreateIngredient[ingredientSlot.GetComponent<IngredientSlot>().currentIngredient] = false;
            }         
            Destroy(ingredientSlot.gameObject);
            
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
