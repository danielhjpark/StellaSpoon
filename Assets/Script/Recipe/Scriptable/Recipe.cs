using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu]
public class Recipe : ScriptableObject {
    public string menuName;
    public Sprite menuImage;
    public List<IngredientAmount> ingredients;
    public GameObject menuPrefab;
    public CookType recipeCookType;
    public BoilingType boilingType;
    //public WokAmount wokAmount;
}

public enum CookType {
    None,
    Cutting,
    Frying,
    Tossing,
    Boiling
}

public enum BoilingType {
    Boil,
    Steam,
}

[System.Serializable]
public class WokAmount  
{

}

[System.Serializable]
public class IngredientAmount
{
    public Ingredient ingredient;
    public int amount;
}
