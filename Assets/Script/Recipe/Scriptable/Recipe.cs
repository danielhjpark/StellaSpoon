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
}

[System.Serializable]
public class IngredientAmount
{
    public Ingredient ingredient;
    public int amount;
}
