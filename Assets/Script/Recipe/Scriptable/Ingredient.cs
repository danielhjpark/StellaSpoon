using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ingredient : ScriptableObject {
    public string ingredientName;
    public Sprite ingredientImage;
    public GameObject ingredientPrefab;
    public bool isPublicIngredient;
    public IngredientCookType ingredientCookType;
    public IngredientCookType[] ingredientCookTypes;
}

public enum IngredientCookType {
    None,
    Cutting,
    Frying,
    Tossing,
    Boiling
}

