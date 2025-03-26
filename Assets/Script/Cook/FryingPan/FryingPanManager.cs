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
    private List<IngredientAmount> checkIngredients = new List<IngredientAmount>();
    private List<IngredientAmount> currentIngredients = new List<IngredientAmount>();
    //---------------------------------------//
    private int firstFryingCount, secondFryingCount;
    private int successFryingCount;

    void Awake()
    {
        CookManager.instance.BindingManager(this);
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
        fryingPanUI.Initialize(menu.fryingSetting);
        StartCoroutine(UseCookingStep());
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
        Debug.Log("Sauce Step");
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
            fryingPanUI.OnIngredientUI();
        }
        yield return new WaitUntil(() => fryingIngredientSystem.fryingMainIngredient != null);

        targetRecipe = FindRecipe(fryingIngredientSystem.checkIngredients[0].ingredient);
        currentIngredients = targetRecipe.ingredients;
        checkIngredients.Clear();
    }

    IEnumerator AddSubIngredient()
    {
        fryingPanUI.OnIngredientUI();
        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            StartCoroutine(cookUIManager.TimerStart());
            ingredientInventory.AddSubIngredients();
        }

        while (true)
        {
            if (CookManager.instance.cookMode == CookManager.CookMode.Select)
            {
                if (CompareIngredient(currentIngredients, checkIngredients)) { break; }
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
