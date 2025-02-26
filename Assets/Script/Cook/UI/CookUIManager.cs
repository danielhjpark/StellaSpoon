using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookUIManager : MonoBehaviour
{
    [SerializeField] RectTransform inventoryPanel;
     [SerializeField] GameObject RecipeSelectPanel;
    SelectRecipeUI selectRecipeUI;

    [SerializeField] int panelSpeed;

    bool isVisible = true;

    void Awake()
    {
        selectRecipeUI = GetComponentInChildren<SelectRecipeUI>();
        panelSpeed = 5;
    }

    private void Update() {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //recipeUIManager.RecipeListSetup();
    }

    public IEnumerator HidePanel() {
        int startPos = -250;
        while(true) {
            startPos += panelSpeed;
            inventoryPanel.anchoredPosition = new Vector2(startPos, inventoryPanel.anchoredPosition.y);
            if(inventoryPanel.anchoredPosition.x >= 250) {
                inventoryPanel.anchoredPosition = new Vector2(250, inventoryPanel.anchoredPosition.y);
                break;
            }
            yield return null;
        }
        isVisible = false;
    }

    public IEnumerator VisiblePanel() {
        int startPos = 250;
        while(true) {
            startPos -= panelSpeed;
            inventoryPanel.anchoredPosition = new Vector2(startPos, inventoryPanel.anchoredPosition.y);
            if(inventoryPanel.anchoredPosition.x <= -250) {
                inventoryPanel.anchoredPosition = new Vector2(-250, inventoryPanel.anchoredPosition.y);
                break;
            }
            yield return null;
        }
        isVisible = true;
    }
}
