using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectRecipeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Recipe currentRecipe;
    [SerializeField]Image image;
    bool isEnter;
    private Color initColor;
    private Color disableColor;
    private IngredientInventory ingredientInventory;

    void Start()
    {
        
        initColor = Color.white;
        disableColor = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            if(isEnter) {
                ingredientInventory.IngredientSlotClear();
                ingredientInventory.IngredientAdd(currentRecipe);
                this.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void RecipeUISetup(Recipe recipe, IngredientInventory ingredientInventory) {
        this.ingredientInventory = ingredientInventory;
        currentRecipe = recipe;
        image.sprite = recipe.menuImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
    }
}
