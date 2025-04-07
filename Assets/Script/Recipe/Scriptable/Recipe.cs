using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    [Header("Menu Setting")]
    public string menuName;
    public Sprite menuImage;
    public int menuPrice;
    public GameObject menuPrefab;

    [Header("Ingrdient Setting")]
    public Ingredient mainIngredient;
    public List<IngredientAmount> ingredients;

    [Header("CookType Setting ")]
    public CookType cookType;

    public BoilingSetting boilingSetting;
    public TossingSetting tossingSetting;
    public CuttingSetting cuttingSetting;
    public FryingSetting fryingSetting;

}

public enum CookType
{
    None,
    Cutting,
    Frying,
    Tossing,
    Boiling
}

[System.Serializable]
public class BoilingSetting
{
    public int cookTime;
    public BoilType boilType;
    public SauceType sauceType;
}

[System.Serializable]
public class TossingSetting
{
    public int tossingCount;
    public SauceType sauceType;
}

[System.Serializable]
public class FryingSetting
{
    public int fryingCount;
    public SauceType sauceType;
    [Header("Total Value = 750")]
    public int[] sectionRange = new int[3];
}

[System.Serializable]
public class CuttingSetting
{
    public Item trimItem;
    public int trimItemCount;
    public int cuttingCount;
}

[System.Serializable]
public class IngredientAmount
{
    public Ingredient ingredient;
    public int amount;

    public IngredientAmount(Ingredient ingredient, int amount)
    {
        this.ingredient = ingredient;
        this.amount = amount;
    }
}

public enum SauceType { Brown, Red, White, None };
public enum BoilType { Boil, Steam }


