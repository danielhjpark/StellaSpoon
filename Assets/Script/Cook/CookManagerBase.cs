using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CookManagerBase : MonoBehaviour
{
    protected Recipe currentMenu;

    public virtual void SelectRecipe(Recipe menu) {
        currentMenu = menu;
    }
    public abstract void CookCompleteCheck();
    
    public abstract void AddIngredient(GameObject obj, Ingredient ingredient);
}
