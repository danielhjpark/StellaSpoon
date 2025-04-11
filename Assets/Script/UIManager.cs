using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static public UIManager instance { get; private set; }
    [Header("UI")]
    [SerializeField] GameObject DailyMenuUI;
    [SerializeField] GameObject RefrigeratorUI;
    [SerializeField] GameObject DeviceUI;
    [SerializeField] GameObject NewRecipeUI;
    [SerializeField] GameObject InteractUI;

    [SerializeField] Item[] items;

    [SerializeField] Recipe[] recipes;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RefrigeratorAddIngredient();
        DailyMenuAdd();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DailyMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Inventory.inventoryActivated = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           // DailyMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Inventory.inventoryActivated = false;
        }
    }

    public void NewRecipePanelVisible()
    {
        NewRecipeUI.SetActive(true);

    }

    void RefrigeratorAddIngredient()
    {
        foreach (var item in items)
        {
            RefrigeratorManager.instance.AddItem(item, 10);
        }
    }

    void DailyMenuAdd()
    {
        foreach (Recipe recipe in recipes)
            RecipeManager.instance.RecipeUnLock(recipe);
    }


    public void VisibleInteractUI() {
        InteractUI.SetActive(true);
    }

    public void HideInteractUI() {
        InteractUI.SetActive(false);
    }


    public void RecipeUnLockUI()
    {
        StartCoroutine(RecipeUnLockFade());
    }

    IEnumerator RecipeUnLockFade()
    {
        NewRecipeUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        NewRecipeUI.SetActive(false);

    }


}
