using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectRecipeUI : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] GameObject recipeParent;
    [SerializeField] GameObject recipePrefab;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("Button UI Objects")]
    [SerializeField] GameObject buttonUIObject;
    [SerializeField] GameObject rightButton;
    [SerializeField] GameObject leftButton;

    private RectTransform recipePanel;
    private int rectViewPos = 0;
    private int viewPosValue = -220;
    private Coroutine movePanel;

    void Start() {
        RecipeListSetup();
        ingredientInventory.gameObject.SetActive(false);
        recipePanel = recipeParent.GetComponent<RectTransform>();
    }

    void Update() {
        HideButton();
    }

    public void RecipeListInit() {
        recipeParent.SetActive(true);   
        foreach (Transform child in recipeParent.transform)
        {
            Destroy(child.gameObject);
        }
        ingredientInventory.IngredientSlotClear();
    }

    public void RecipeUpdate() {
        this.gameObject.SetActive(false);
    }

    public void RecipeListSetup() {
        RecipeListInit();
        int recipeCount = DailyMenuManager.dailyMenuList.Count;
        if(recipeCount <= 0) return;

        foreach (Recipe recipe in DailyMenuManager.dailyMenuList.Keys) {
            if(recipe.cookType != CookManager.instance.currentCookType) continue;
            else if(DailyMenuManager.dailyMenuList[recipe] <= 0) continue;
            GameObject recipeObject = Instantiate(recipePrefab, Vector3.zero, Quaternion.identity);
            recipeObject.transform.SetParent(recipeParent.transform);

            SelectRecipeSlot selectRecipe = recipeObject.GetComponent<SelectRecipeSlot>();
            selectRecipe.RecipeUISetup(recipe, ingredientInventory);
            selectRecipe.OnSelectRecipe += RecipeUpdate;
        }

    }

    public void RecipePanelInit() {
        int correctRecipeCount = recipeParent.transform.childCount;
        int rectWidth = correctRecipeCount * 220, rectHeight = 200;//size + spacing
        recipePanel.sizeDelta = new Vector2(rectWidth, rectHeight);
        recipePanel.pivot = new Vector2(0, 0.5f);

        if(recipeParent.transform.childCount > 3) {
            buttonUIObject.SetActive(true);
        }
        else {
            buttonUIObject.SetActive(false);
        }
    }

    //---------------Button------------------//

    private void HideButton() {
        rightButton.SetActive(true);
        leftButton.SetActive(true); 

        if(rectViewPos <= 0) 
            leftButton.SetActive(false);

        if(rectViewPos >= recipeParent.transform.childCount - 3) 
            rightButton.SetActive(false);
    }

    public void RectMoveRight() {
        if(rectViewPos >= recipeParent.transform.childCount - 3 || movePanel != null) return;
        rectViewPos++;
        int rectWidth = rectViewPos * viewPosValue;
        Vector2 targetPos = new Vector2(rectWidth, 100);
        movePanel = StartCoroutine(MovePanel(targetPos));
    }

    public void RectMoveLeft() {
        if(rectViewPos <= 0 || movePanel != null) return;
        int viewPosValue = -220;
        rectViewPos--;
        int rectWidth = rectViewPos * viewPosValue;
        Vector2 targetPos = new Vector2(rectWidth, 100);
        movePanel = StartCoroutine(MovePanel(targetPos));
    }

    IEnumerator MovePanel(Vector2 targetPos)  {
        while(true) {
            if(Vector2.Distance(recipePanel.anchoredPosition, targetPos) < 2f) {
                recipePanel.anchoredPosition = targetPos;
                break;
            }
            recipePanel.anchoredPosition = Vector2.Lerp(recipePanel.anchoredPosition, targetPos, Time.deltaTime * 5f);
            yield return null;
        }
        movePanel = null;
    }

}
