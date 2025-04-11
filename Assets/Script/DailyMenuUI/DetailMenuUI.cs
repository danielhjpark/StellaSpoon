using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailMenuUI : MonoBehaviour
{
    //----------------Menu Data Binding ----------------//
    [SerializeField] TextMeshProUGUI menuName;
    [SerializeField] TextMeshProUGUI menuDescription;
    [SerializeField] TextMeshProUGUI menuAmount;
    [SerializeField] TextMeshProUGUI menuPrice;
    [SerializeField] Image menuImage;
    
    //--------------Ingredient Parent--------------------//
    [SerializeField] GameObject ingredients;

    
    public Recipe currentRecipe;
    public int currentAmount;
    
    void Update()
    {
         if(currentRecipe != null) {
             IngredientsUpdate(currentAmount);
         }
    }

    //-----------------Button-------------------------//

    public void AddAmount() {
        int checkAmount = currentAmount + 1;
        if(RecipeManager.instance.IsCanMakeMenu(currentRecipe, checkAmount)) {
            DetailUpdate(currentRecipe, checkAmount);
        }
    }

    public void RemoveAmount() {
        int checkAmount = currentAmount - 1;
        if(checkAmount < 1) { return;}

        if(RecipeManager.instance.IsCanMakeMenu(currentRecipe, checkAmount)) {
            DetailUpdate(currentRecipe, checkAmount);
        }
    }

    //-------------------------------------------//
    public void DetailUpdate(Recipe recipe, int Amount) {
        currentRecipe = recipe;
        currentAmount = Amount;

        menuAmount.text = currentAmount.ToString();
        menuName.text = recipe.menuName;
        menuImage.sprite = recipe.menuImage;
        menuPrice.text = recipe.menuPrice.ToString();
        
        IngredientsUpdate(Amount);
    }

    public void IngredientsUpdate(int Amount) {
        int ingredientPanelCount = ingredients.transform.childCount;
        int ingredientCount = currentRecipe.ingredients.Count;

        GameObject currnetIngredient = ingredients.transform.GetChild(0).gameObject;
        currnetIngredient.SetActive(true);
        currnetIngredient.GetComponent<IngredientUI>().IngredientUpdate(currentRecipe.mainIngredient, Amount);

        for(int i = 0; i < ingredientCount; i++) {
            
            currnetIngredient = ingredients.transform.GetChild(i + 1).gameObject;
            currnetIngredient.SetActive(true);
            currnetIngredient.GetComponent<IngredientUI>().IngredientUpdate(currentRecipe.ingredients[i], Amount);
        }
        for(int i = ingredientCount + 1; i < ingredientPanelCount; i++) {
            ingredients.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void DetailUIClear() {
        menuAmount.text = null;
        menuName.text = null;
        menuImage.sprite = null;

        for(int i = 0; i < currentRecipe.ingredients.Count + 1; i++) {
            ingredients.transform.GetChild(i).gameObject.SetActive(false);
        }

        currentRecipe = null;
        currentAmount = 0;
    }
}
