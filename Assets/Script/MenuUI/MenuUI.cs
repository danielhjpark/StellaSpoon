using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Image recipeImage;
    private MenuSystem recipeUI;

    private Color initColor;
    private Color disableColor;
    
    public Recipe currentRecipe;


    void Start()
    {
        recipeUI = GetComponentInParent<MenuSystem>();    
        initColor = Color.white;
        disableColor = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {
        CanMakeMenu();
    }

    public void MenuUISetup(Recipe Recipe) {
        this.currentRecipe = Recipe;
        recipeImage.sprite = currentRecipe.menuImage;
        CanMakeMenu();

    }

    //재료 부족 시, 어두운 이미지 효과 및 비활성화
    void CanMakeMenu() {
        foreach(IngredientAmount currentIngredient in currentRecipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int currentIngredientAmount = currentIngredient.amount;
            int requireIngredientAmount = IngredientManager.instance.IngredientAmount[currentIngdeient];
            if(currentIngredientAmount > requireIngredientAmount) {
                recipeImage.color = disableColor;
                return;
            }
        }
        recipeImage.color = initColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        recipeUI.DetailUIUpdate(currentRecipe);
    }
}
