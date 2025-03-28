using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;

public class FryingPanManager : CookManagerBase
{
    //------------"FryingPan System"---------------//
    private FryingSystem fryingSystem;
    private FryingSauceSystem fryingSauceSystem;
    private FryingIngredientSystem fryingIngredientSystem;

    [Header("UI Object")]
    [SerializeField] FryingPanUI fryingPanUI;
    [SerializeField] CookUIManager cookUIManager;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("Set Objects")]
    [SerializeField] private Transform dropPos;
    [SerializeField] private GameObject dropIngredient;

    private List<GameObject> fryingIngredients = new List<GameObject>();
    //---------------------------------------//
    private int firstFryingCount, secondFryingCount;
    private int successFryingCount;

    void Awake()
    {
        CookManager.instance.BindingManager(this);
        cookUIManager.Initialize(this);
        fryingSystem = GetComponent<FryingSystem>();
        fryingSauceSystem = GetComponent<FryingSauceSystem>();
        fryingIngredientSystem = GetComponent<FryingIngredientSystem>();

    }

    void Update()
    {
        //Before Select UI => Can cancel use utensil
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentMenu == null)
                CookSceneManager.instance.UnloadScene();
        }
    }

    //--------------- virtual Method ----------------------//
    public override void SelectRecipe(Recipe menu)
    {
        base.SelectRecipe(menu);
        firstFryingCount = menu.fryingSetting.fryingCount;
        secondFryingCount = menu.fryingSetting.fryingCount;
        fryingPanUI.Initialize(menu.fryingSetting.sectionRange);
        StartCoroutine(UseCookingStep());
    }

    public void RecipeSetting(Recipe menu) {
        base.SelectRecipe(menu);
        
        if(menu.cookType != CookType.Frying) {
            int[] defaultRange = {300, 100, 300};
            firstFryingCount = 2;
            secondFryingCount = 2;
            fryingPanUI.Initialize(defaultRange);
            return;
        } 
        else {
            firstFryingCount = menu.fryingSetting.fryingCount;
            secondFryingCount = menu.fryingSetting.fryingCount;
            fryingPanUI.Initialize(menu.fryingSetting.sectionRange);
        }

    }

    public override IEnumerator UseCookingStep()
    {
        yield return StartCoroutine(AddMainIngredient());
        yield return StartCoroutine(InherentMotion(firstFryingCount));
        yield return StartCoroutine(AddSubIngredient());
        yield return StartCoroutine(AddSauce());
        yield return StartCoroutine(InherentMotion(secondFryingCount));
        CookCompleteCheck();
    }

    public override void CookCompleteCheck()
    {
        CookSceneManager.instance.UnloadScene("FryingPanMergeTest", currentMenu);
    }
    //----------------------- Ingredient System ----------------------------------//


    public override void AddIngredient(GameObject ingredientObject, Ingredient ingredient)
    {
        Debug.Log(ingredient.ingredientType);
        if (ingredient.ingredientType == IngredientType.Main)
        {
            fryingIngredientSystem.AddMainIngredient(ingredientObject, ingredient);
        }
        else
        {
            fryingIngredientSystem.AddSubIngredient(ingredientObject, ingredient);
        }
    }

    //--------------FryingPan System Method------------------//
    IEnumerator InherentMotion(int fryingCount)
    {
        fryingPanUI.OnFryingPanUI();
        //wokTossingSystem.BindTossingObject(wokIngredients);
        yield return StartCoroutine(fryingSystem.InherentMotion(fryingCount, (callbackValue) =>
        {
            successFryingCount += callbackValue;
        }
        ));
    }

    IEnumerator AddSauce()
    {
        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            fryingSauceSystem.InitializeMakeMode();
        }
        else
        {
            if (currentMenu.tossingSetting.sauceType == SauceType.None)
            {
                yield break;
            }
            else
            {
                fryingSauceSystem.Initialize(currentMenu.fryingSetting);
            }
        }

        while (!fryingSauceSystem.IsLiquidFilled())
        {
            yield return null;
        }
    }

    IEnumerator AddMainIngredient()
    {
        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {
            GameObject mainIngredient = Instantiate(currentMenu.mainIngredient.ingredientPrefab, Vector3.zero, Quaternion.identity);
            AddIngredient(mainIngredient, currentMenu.mainIngredient);
            yield return new WaitForSeconds(0.5f);

        }
        else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            ingredientInventory.AddMainIngredients();
            yield return new WaitUntil(() => fryingIngredientSystem.fryingMainIngredient != null);
            targetRecipe = RecipeManager.instance.FindRecipe(fryingIngredientSystem.checkIngredients[0].ingredient);
            RecipeSetting(targetRecipe);
            yield return new WaitForSeconds(0.5f);
            
        }
        
        fryingIngredientSystem.checkIngredients.Clear();
        fryingSystem.Initialize(fryingIngredientSystem.fryingMainIngredient);
    }

    IEnumerator AddSubIngredient()
    {
        fryingPanUI.OnIngredientUI();
        StartCoroutine(cookUIManager.VisiblePanel());
        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            StartCoroutine(cookUIManager.TimerStart());
            ingredientInventory.AddSubIngredients();
        }

        while (true)
        {
            if (CookManager.instance.cookMode == CookManager.CookMode.Select)
            {
                if (RecipeManager.instance.CompareRecipe(currentMenu, fryingIngredientSystem.checkIngredients)) { break; }
            }
            else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
            {
                if (cookUIManager.TimerEnd()) { break; }
            }
            yield return null;
        }
        yield return StartCoroutine(cookUIManager.HidePanel());
    }

}
