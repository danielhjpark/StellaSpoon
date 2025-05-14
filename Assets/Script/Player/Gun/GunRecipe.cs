using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientRequirement
{
    public string ingredientName;
    public int requiredAmount;
}
[System.Serializable]
public class GunRecipe
{
    public string gunName;
    public string spritePath;
    public int needGold;
    public List<IngredientRequirement> ingredients = new();
}
