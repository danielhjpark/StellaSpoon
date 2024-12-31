using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSlot : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Image recipeImage;
    private DailyMenuManager dailyMenuManager;

    private Color initColor;
    private Color disableColor;
    
    public Recipe currentRecipe;


    void Start()
    {
        dailyMenuManager = GetComponentInParent<DailyMenuManager>();    
        initColor = Color.white;
        disableColor = Color.grey;
    }

    void Update()
    {
        CreateMenu();
    }

    public void MenuUISetup(Recipe Recipe) {
        this.currentRecipe = Recipe;
        recipeImage.sprite = currentRecipe.menuImage;
        CreateMenu();

    }

    //재료 부족 시, 어두운 이미지 효과 및 비활성화
    void CreateMenu() {
        foreach(IngredientAmount currentIngredient in currentRecipe.ingredients) {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int currentIngredientAmount = currentIngredient.amount;
            int requireIngredientAmount = IngredientManager.IngredientAmount[currentIngdeient];
            if(currentIngredientAmount > requireIngredientAmount) {
                recipeImage.color = disableColor;
                return;
            }
        }
        recipeImage.color = initColor;
    }

    void SelectMenu() {
        dailyMenuManager.DetailUIUpdate(currentRecipe, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectMenu();

    }
}
