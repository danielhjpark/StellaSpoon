using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ingredient : ScriptableObject {
    public string ingredientName;
    public Sprite ingredientImage;
    public GameObject ingredientPrefab;
    public IngredientCookType ingredientCookType;
}

public enum IngredientCookType {
    Cutting,
    Frying,
    Tossing,
    Boiling
}
