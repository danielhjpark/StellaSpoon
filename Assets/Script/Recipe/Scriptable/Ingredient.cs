using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ingredient : ScriptableObject {

    
    public string ingredientName;
    public string ingredientText;
    public Sprite ingredientImage;
    public GameObject ingredientPrefab;
    public IngredientType ingredientType;
    public IngredientCookType ingredientCookType;
    public IngredientCookType[] ingredientCookTypes;
    public int ingredientUseCount;
}

public enum IngredientType {
    Main, 
    Sub,
    Trim
}

public enum IngredientCookType {
    None,
    Cutting,
    Frying,
    Tossing,
    Boiling
}

