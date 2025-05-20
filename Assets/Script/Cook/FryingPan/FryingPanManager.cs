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
    private FryingPanAudioSystem fryingPanAudioSystem;

    [Header("UI Object")]
    [SerializeField] FryingPanUI fryingPanUI;
    [SerializeField] CookUIManager cookUIManager;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("Set Objects")]
    [SerializeField] private Transform dropPos;
    [SerializeField] private GameObject dropIngredient;

    //-------------Frying Setting------------------//
    private FryingStep fryingStep;
    private int firstFryingCount, secondFryingCount;
    private int totalSuccessCount;
    private int successCount;

    void Awake()
    {
        fryingSystem = GetComponent<FryingSystem>();
        fryingSauceSystem = GetComponent<FryingSauceSystem>();
        fryingIngredientSystem = GetComponent<FryingIngredientSystem>();
        fryingPanAudioSystem = GetComponent<FryingPanAudioSystem>();

        CookManager.instance.BindingManager(this);
        CookManager.instance.spawnPoint = dropPos;
        CookManager.instance.isCanUseMiddleTable = false;
        cookUIManager.Initialize(this);

        isCanEscape = true;


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CookManager.instance.isCanUseMiddleTable = true;
            if (isCanEscape) CookSceneManager.instance.UnloadScene();
        }
    }

    //--------------- virtual Method ----------------------//
    public override void SelectRecipe(Recipe menu)
    {
        base.SelectRecipe(menu);

        fryingStep = menu.fryingSetting.fryingStep;
        firstFryingCount = menu.fryingSetting.firstFryingCount;
        secondFryingCount = menu.fryingSetting.secondFryingCount;
        totalSuccessCount = firstFryingCount + secondFryingCount - 1;
        //fryingPanUI.Initialize(true);
        StartCoroutine(UseCookingStep());
    }

    public void RecipeSetting(Recipe menu)
    {
        base.SelectRecipe(menu);

        if (menu == null || menu.cookType != CookType.Frying) {
            fryingStep = FryingStep.Medium;
            firstFryingCount = 2;
            secondFryingCount = 2;
            totalSuccessCount = firstFryingCount + secondFryingCount - 1;
        }
        else {
            fryingStep = menu.fryingSetting.fryingStep;
            firstFryingCount = menu.fryingSetting.firstFryingCount;
            secondFryingCount = menu.fryingSetting.secondFryingCount;
            totalSuccessCount = firstFryingCount + secondFryingCount - 1;
        }
        //fryingPanUI.Initialize(true);
    }

    public override IEnumerator UseCookingStep()
    {
        isCanEscape = false;
        yield return StartCoroutine(AddMainIngredient());
        yield return StartCoroutine(FireControl());
        yield return StartCoroutine(InherentMotion(firstFryingCount));
        yield return StartCoroutine(AddSubIngredient());
        yield return StartCoroutine(AddSauce());
        yield return StartCoroutine(InherentMotion(secondFryingCount));
        CookCompleteCheck();
    }

    public override void CookCompleteCheck()
    {
        string currentSceneName = "FryingPanMergeTest";

        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {
            if (totalSuccessCount <= successCount)
            {
                CookSceneManager.instance.UnloadScene(currentSceneName, currentMenu);
                return;
            }
            //fail Á¶°Ç
            else
            {
                CookSceneManager.instance.UnloadScene(currentSceneName, CookManager.instance.failMenu);
                OrderManager.instance.FailMenu(currentMenu);
                return;
            }
        }
        else
        {
            if (targetRecipe.cookType != CookType.Frying)
            {
                Debug.Log("Wrong cook type");
                CookSceneManager.instance.UnloadScene(currentSceneName, CookManager.instance.failMenu);
                return;
            }

            if (!RecipeManager.instance.CompareRecipe(currentMenu, fryingIngredientSystem.checkIngredients))
            {
                Debug.Log("Ingredient mismatch");
                CookSceneManager.instance.UnloadScene(currentSceneName, CookManager.instance.failMenu);
                return;
            }

            if (successCount < totalSuccessCount)
            {
                Debug.Log("Not enough tossing"+successCount +":"+totalSuccessCount);
                CookSceneManager.instance.UnloadScene(currentSceneName, CookManager.instance.failMenu);
                return;
            }

            if (fryingSauceSystem.sauceType != targetRecipe.tossingSetting.sauceType)
            {
                Debug.Log("Wrong sauce type" +fryingSauceSystem.sauceType + "" + targetRecipe.tossingSetting.sauceType);
                CookSceneManager.instance.UnloadScene(currentSceneName, CookManager.instance.failMenu);
                return;
            }

            if (!fryingPanUI.CheckFireStep(targetRecipe.tossingSetting.firePower))
            {
                CookSceneManager.instance.UnloadScene(currentSceneName, CookManager.instance.failMenu);
                return;
            }

            //UnLock New Recipe;
            RecipeManager.instance.RecipeUnLock(targetRecipe);
            CookSceneManager.instance.UnloadScene(currentSceneName, targetRecipe);
            Debug.Log("Success");
            return;

        }
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
            currentSubIngredient++;
            StartCoroutine(cookUIManager.VisiblePanel());
            fryingIngredientSystem.AddSubIngredient(ingredientObject, ingredient);
        }
    }

    //--------------FryingPan System Method------------------//

    IEnumerator FireControl()
    {
        bool fryingPanUnlock = false;
        fryingPanUI.Initialize(fryingPanUnlock);
        if (fryingPanUnlock) yield break;
        StartCoroutine(cookUIManager.TimerStart(3f));
        fryingPanUI.OnFireControlUI();
        
        while (true)
        {
            if (cookUIManager.TimerEnd())
            {
                cookUIManager.TimerStop();
                fryingPanUI.OffFireControlUI();
                break;
            }
            yield return null;
        }

    }

    IEnumerator InherentMotion(int fryingCount)
    {
        fryingPanUI.OnFryingPanUI();
        fryingSystem.InitializeInherentMotion();

        yield return StartCoroutine(fryingSystem.InherentMotion(fryingCount, (callbackValue) =>
        {
            successCount += callbackValue;
        }
        ));
        //fryingPanAudioSystem.StopAudioSource(FryingPanAudioSystem.AudioType.Frying);
    }

    IEnumerator AddSauce()
    {
        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            fryingSauceSystem.InitializeMakeMode(secondFryingCount);
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
        StartCoroutine(cookUIManager.TimerStart(10f));
        while (true)
        {                
            if (cookUIManager.TimerEnd() || fryingSauceSystem.IsLiquidFilled()) { 
                cookUIManager.TimerStop();
                break; 
            }
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
        fryingSystem.Initialize(fryingIngredientSystem.fryingMainIngredient, fryingStep, totalSuccessCount);
    }

    IEnumerator AddSubIngredient()
    {
        fryingPanUI.OnIngredientUI();

        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            StartCoroutine(cookUIManager.TimerStart(10f));
            ingredientInventory.AddSubIngredients();
            StartCoroutine(cookUIManager.VisiblePanel());
        }

        while (true)
        {
            if (CookManager.instance.cookMode == CookManager.CookMode.Select)
            {
                if (RecipeManager.instance.CompareRecipe(currentMenu, fryingIngredientSystem.checkIngredients)) { break; }
            }
            else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
            {
                if (cookUIManager.TimerEnd() || currentSubIngredient >= maxSubIngredient) { 
                    cookUIManager.TimerStop();
                    break; 
                }
            }
            yield return null;
        }
        yield return StartCoroutine(cookUIManager.HidePanel());
    }

}
