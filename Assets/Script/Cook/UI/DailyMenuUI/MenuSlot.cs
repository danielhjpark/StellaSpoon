using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image recipeImage;
    [SerializeField] TextMeshProUGUI recipeName;
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

    public void MenuUISetup(Recipe Recipe)
    {
        this.currentRecipe = Recipe;
        recipeImage.sprite = currentRecipe.menuImage;
        recipeName.text = currentRecipe.menuName;
        CreateMenu();

    }

    //?û¨Î£? Î∂?Ï°? ?ãú, ?ñ¥?ëê?ö¥ ?ù¥ÎØ∏Ï?? ?ö®Í≥? Î∞? ÎπÑÌôú?Ñ±?ôî
    void CreateMenu()
    {
        if (IngredientManager.IngredientAmount[currentRecipe.mainIngredient] <= 0)
        {
            recipeImage.color = disableColor;
            return;
        }
        foreach (IngredientAmount currentIngredient in currentRecipe.ingredients)
        {
            Ingredient currentIngdeient = currentIngredient.ingredient;
            int currentIngredientAmount = currentIngredient.amount;
            int requireIngredientAmount = IngredientManager.IngredientAmount[currentIngdeient];
            if (currentIngredientAmount > requireIngredientAmount)
            {
                recipeImage.color = disableColor;
                return;
            }
        }
        recipeImage.color = initColor;
    }

    void SelectMenu()
    {
        dailyMenuManager.DetailUIUpdate(currentRecipe, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectMenu();
    }
}
