using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CookUIManager : MonoBehaviour
{
    [SerializeField] RectTransform inventoryPanel;
    [SerializeField] GameObject SelectRecipePanel;
    [SerializeField] GameObject SelectModePanel;
    [SerializeField] GameObject TimerPanel;
    CookManagerBase currentCookManager;
    TimerSystem timerSystem;
    private int panelSpeed;

    void Start()
    {
        panelSpeed = 5;
        SelectRecipePanel.SetActive(false);
        if(TimerPanel != null && TimerPanel.TryGetComponent<TimerSystem>(out TimerSystem timerSystem)) {
            this.timerSystem = timerSystem;
            TimerPanel.SetActive(false);
        }
        
        
    }

    public void Initialize(CookManagerBase cookManagerBase)
    {
        this.currentCookManager = cookManagerBase;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //-------------------Button-------------------
    public void SelectRecipeMode()
    {
        inventoryPanel.gameObject.SetActive(false);
        SelectRecipePanel.SetActive(true);
        SelectModePanel.SetActive(false);
        CookManager.instance.cookMode = CookManager.CookMode.Select;
    }

    public void MakeRecipeMode()
    {
        inventoryPanel.gameObject.SetActive(true);
        SelectModePanel.SetActive(false);
        //inventoryPanel.GetComponent<IngredientInventory>().AddAllIngredients();
        CookManager.instance.cookMode = CookManager.CookMode.Make;
        StartCoroutine(VisiblePanel());
        StartCoroutine(currentCookManager.UseCookingStep());
    }

    public IEnumerator TimerStart(float second)
    {
        TimerPanel.SetActive(true);
        yield return StartCoroutine(timerSystem.TimerStart(second));
        TimerPanel.SetActive(false);
    }

    public void TimerReset()
    {
        timerSystem.TimerReset();
    }

    public bool TimerEnd()
    {
        return timerSystem.TimerEnd();
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
        inventoryPanel.gameObject.SetActive(true);
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
