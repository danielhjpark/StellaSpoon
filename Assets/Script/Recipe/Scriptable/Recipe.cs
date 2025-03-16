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
    public BoilType boilType;
}

public enum BoilType
{
    Boil,
    Steam,
}

[System.Serializable]
public class TossingSetting
{
    public int tossingCount;
}

[System.Serializable]
public class FryingSetting
{
    public int fryingCount;
    [Header("Total Value = 750")]
    public int[] sectionRange = new int[3];
}

[System.Serializable]
public class CuttingSetting
{
    public int cuttingCount;
    public int successRange;
}

[System.Serializable]
public class IngredientAmount
{
    public Ingredient ingredient;
    public int amount;
}
