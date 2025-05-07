using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CookUIManager : MonoBehaviour
{
    [SerializeField] RectTransform inventoryPanel;
    [SerializeField] GameObject SelectRecipePanel;
    [SerializeField] GameObject SelectModePanel;
    [SerializeField] GameObject TimerPanel;
    CookManagerBase currentCookManager;
    TimerSystem timerSystem;
    private int panelSpeed = 10;
    private Coroutine inventoryCoroutine;

    void Awake()
    {
        SelectRecipePanel.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
        if (TimerPanel != null && TimerPanel.TryGetComponent<TimerSystem>(out TimerSystem timerSystem))
        {
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
        SelectModePanel.SetActive(false);
        SelectRecipePanel.SetActive(true);
        CookManager.instance.cookMode = CookManager.CookMode.Select;
    }

    public void MakeRecipeMode()
    {
        SelectModePanel.SetActive(false);
        CookManager.instance.cookMode = CookManager.CookMode.Make;

        StartCoroutine(VisiblePanel());
        StartCoroutine(currentCookManager.UseCookingStep());
    }

    //-------------------Timer---------------------//

    public IEnumerator TimerStart(float second)
    {
        TimerPanel.SetActive(true);
        yield return StartCoroutine(timerSystem.TimerStart(second));
        TimerPanel.SetActive(false);
    }

    public void TimerStop()
    {
        TimerPanel.SetActive(false);
        timerSystem.TimerStop();
    }

    public bool TimerEnd()
    {
        return timerSystem.TimerEnd();
    }

    //--------------------Inventory--------------------------//

    public IEnumerator HidePanel()
    {
        inventoryPanel.gameObject.SetActive(true);
        if(inventoryCoroutine != null) yield return new WaitUntil(() => inventoryCoroutine != null);
        inventoryCoroutine = StartCoroutine(HideActive());
    }

    public IEnumerator HideActive()
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
        inventoryCoroutine = null;
    }


    public IEnumerator VisiblePanel()
    {
        inventoryPanel.gameObject.SetActive(true);
        if(inventoryCoroutine != null) yield return new WaitUntil(() => inventoryCoroutine != null);
        inventoryCoroutine = StartCoroutine(VisibleActive());
    }

    private IEnumerator VisibleActive() {
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
        inventoryCoroutine = null;
    }

}
