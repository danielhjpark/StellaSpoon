using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CookManagerBase : MonoBehaviour
{
    public enum CookMode { Select, Make };
    protected CookMode cookMode;

    protected Recipe currentMenu;
    protected Recipe targetRecipe;

    protected bool isCanEscape;
    protected const int maxSubIngredient = 3;
    protected int currentSubIngredient = 0;

    public virtual void SelectRecipe(Recipe menu)
    {
        currentMenu = menu;
    }
    public abstract void CookCompleteCheck();
    public abstract IEnumerator UseCookingStep();
    public abstract void AddIngredient(GameObject obj, Ingredient ingredient);


}
