using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu]
public class Recipe : ScriptableObject {
    public string menuName;
    public Sprite menuImage;
    public int menuPrice;
    public List<IngredientAmount> ingredients;
    public GameObject menuPrefab;

    public CookType cookType;
    public BoilingSetting boilingSetting;
    public TossingSetting tossingSetting;
    public CuttingSetting cuttingSetting;
    public FryingSetting fryingSetting;

}

public enum CookType {
    None,
    Cutting,
    Frying,
    Tossing,
    Boiling
}

[System.Serializable]
public class BoilingSetting {
    public BoilType boilType;
}

public enum BoilType {
    Boil,
    Steam,
}

[System.Serializable]
public class TossingSetting {
    public int tossingCount;
    public int successRange;
}

[System.Serializable]
public class FryingSetting {
    public int fryingCount;
    public int successRange;
}

[System.Serializable]
public class CuttingSetting {
    public int cuttingCount;
    public int successRange;
}

[System.Serializable]
public class IngredientAmount
{
    public Ingredient ingredient;
    public int amount;
}
