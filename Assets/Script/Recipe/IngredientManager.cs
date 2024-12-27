using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    static public IngredientManager instance = null;

    Ingredient[] IngredientList; //임시 데이터 베이스
    public Dictionary<Ingredient, int> IngredientAmount; //임시 데이터 베이스
    
    void Awake()
    {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }

        IngredientList = Resources.LoadAll<Ingredient>("Scriptable/Ingredient");
        IngredientAmount = new Dictionary<Ingredient, int>();
        IngredientAmountInit();
    }

    void IngredientAmountInit() {
        foreach (Ingredient ingredient in IngredientList) {
            IngredientAmount.Add(ingredient, 4);
        }
    }

}
