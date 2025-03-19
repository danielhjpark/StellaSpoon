using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookUIManager : MonoBehaviour
{
    [SerializeField] RectTransform inventoryPanel;
    [SerializeField] GameObject SelectRecipePanel;
    [SerializeField] GameObject MakeRecipePanel;
    [SerializeField] GameObject SelectModePanel;

    private int panelSpeed;

    void Start()
    {
        panelSpeed = 5;
        SelectRecipePanel.SetActive(false);
        MakeRecipePanel.SetActive(false);
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //-------------------Button-------------------
    public void SelectRecipeMode()
    {
        SelectRecipePanel.SetActive(true);
        SelectModePanel.SetActive(false);
    }

    public void MakeRecipeMode()
    {
        MakeRecipePanel.SetActive(true);
        SelectModePanel.SetActive(false);
    }


    public IEnumerator HidePanel()
    {
        int startPos = -250;
        while (true)
        {
            startPos += panelSpeed;
            inventoryPanel.anchoredPosition = new Vector2(startPos, inventoryPanel.anchoredPosition.y);
            if (inventoryPanel.anchoredPosition.x >= 250)
            {
                inventoryPanel.anchoredPosition = new Vector2(250, inventoryPanel.anchoredPosition.y);
                break;
            }
            yield return null;
        }
    }

    public IEnumerator VisiblePanel()
    {
        int startPos = 250;
        while (true)
        {
            startPos -= panelSpeed;
            inventoryPanel.anchoredPosition = new Vector2(startPos, inventoryPanel.anchoredPosition.y);
            if (inventoryPanel.anchoredPosition.x <= -250)
            {
                inventoryPanel.anchoredPosition = new Vector2(-250, inventoryPanel.anchoredPosition.y);
                break;
            }
            yield return null;
        }
    }
}
