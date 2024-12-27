using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailUI : MonoBehaviour
{
    //----------------Menu Data Binding ----------------//
    [SerializeField] TextMeshProUGUI menuName;
    [SerializeField] TextMeshProUGUI menuDescription;
    [SerializeField] Image menuImage;

    //--------------Ingredient Parent--------------------//
    [SerializeField] GameObject ingredients;


    public Recipe currentRecipe;

    // Update is called once per frame
    void Update()
    {
        if(currentRecipe != null) {
            IngredientsUpdate();
        }
    }

    public void DetailUpdate(Recipe recipe) {
        currentRecipe = recipe;

        menuName.text = recipe.menuName;
        menuImage.sprite = recipe.menuImage;
        
        IngredientsUpdate();
    }

    public void IngredientsUpdate() {
        int ingredientPanelCount = ingredients.transform.childCount;
        int ingredientCount = currentRecipe.ingredients.Count;
        
        for(int i = 0; i < ingredientCount; i++) {
            GameObject currnetIngredient = ingredients.transform.GetChild(i).gameObject;
            currnetIngredient.GetComponent<IngredientUI>().IngredientUpdate(currentRecipe.ingredients[i]);
        }
        for(int i = ingredientCount; i <ingredientPanelCount; i++) {
            ingredients.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
