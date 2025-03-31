using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CookManagerBase : MonoBehaviour
{
    protected Recipe currentMenu;
    protected Recipe targetRecipe;
    protected bool isCanEscape;

    public virtual void SelectRecipe(Recipe menu)
    {
        currentMenu = menu;
    }
    public abstract void CookCompleteCheck();
    public abstract IEnumerator UseCookingStep();
    public abstract void AddIngredient(GameObject obj, Ingredient ingredient);


}
