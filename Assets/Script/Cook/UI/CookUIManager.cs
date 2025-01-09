using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookUIManager : MonoBehaviour
{
   [SerializeField] RectTransform inventoryPanel;
    RecipeUIManager recipeUIManager;

    [SerializeField] int panelSpeed;

    void Awake()
    {
        //inventoryPanel = GetComponent<RectTransform>();
        recipeUIManager = GetComponentInChildren<RecipeUIManager>();
        panelSpeed = 5;
    }

    private void OnEnable() {
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
    }
}
