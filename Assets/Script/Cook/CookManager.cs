using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CookManager : MonoBehaviour
{
    static public CookManager instance;
    public CookUIManager cookUIManager;

    [SerializeField] CuttingManager cuttingManager;
    [SerializeField] WokManager wokManager;
    [SerializeField] FryingPanManager fryingPanManager;
    [SerializeField] PotManager potManager;
    
    void Start()
    {
        instance = this;
    }

    public void DropObject(GameObject ingredientObject, Ingredient ingredient) {
        switch(ingredient.ingredientCookType) {
            case IngredientCookType.Cutting:
                cuttingManager.LocateIngredient(ingredientObject);
                break;
            case IngredientCookType.Frying:
                fryingPanManager.LocateIngredient(ingredientObject);
                break;
            case IngredientCookType.Tossing:
                wokManager.LocateIngredient(ingredientObject);
                break;
            case IngredientCookType.Boiling:
                potManager.LocateIngredient(ingredientObject);
                break;

        }
        
    }

}
