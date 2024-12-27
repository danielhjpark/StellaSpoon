using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class IngredientUI : MonoBehaviour
{
    [SerializeField] Image ingredientImage;
    [SerializeField] TextMeshProUGUI ingredientName;
    [SerializeField] TextMeshProUGUI ingredientCount;

    void Update()
    {
        
    }

    public void IngredientUpdate(IngredientAmount currentIngredient) {
        Ingredient ingredient = currentIngredient.ingredient;
        int ingredientAmount = currentIngredient.amount;

        int TotalIngredientAmount = IngredientManager.instance.IngredientAmount[ingredient];
        int RequireIngredientAmount = ingredientAmount;

        ingredientImage.sprite = ingredient.ingredientImage;
        ingredientName.text = ingredient.ingredientName;
        ingredientCount.text = TotalIngredientAmount +"/" + RequireIngredientAmount;
    }
}
