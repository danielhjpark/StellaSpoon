using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    static public IngredientManager instance = null;
    static public Dictionary<string, Ingredient> IngredientList;
    static public Dictionary<Ingredient, int> IngredientAmount; 
    
    void Awake()
    {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }
        IngredientList = new Dictionary<string, Ingredient>();
        IngredientAmount = new Dictionary<Ingredient, int>();
        IngredientAmountInit();
    }

    void IngredientAmountInit() {
        Ingredient[] IngredientScriptable = Resources.LoadAll<Ingredient>("Scriptable/Ingredient");
        foreach (Ingredient ingredient in IngredientScriptable) {
            IngredientList.Add(ingredient.name, ingredient);
            IngredientAmount.Add(ingredient, 0);
        }
    }

    public Ingredient FindIngredient(string IngredientName) {
        return IngredientList[IngredientName];
    }

}
