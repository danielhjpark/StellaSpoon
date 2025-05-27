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
    public int ingredientUseCount;
}

public enum IngredientType
{
    Main,
    Sub,
    Trim,
    End
}



