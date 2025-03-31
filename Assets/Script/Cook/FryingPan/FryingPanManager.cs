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
    private int totalSuccessCount;
    private int successCount;
    void Awake()
    {
        fryingSystem = GetComponent<FryingSystem>();
        fryingSauceSystem = GetComponent<FryingSauceSystem>();
        fryingIngredientSystem = GetComponent<FryingIngredientSystem>();

        CookManager.instance.BindingManager(this);
        CookManager.instance.spawnPoint = dropPos;
        cookUIManager.Initialize(this);

        isCanEscape = true;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCanEscape) CookSceneManager.instance.UnloadScene();
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
        
        if(menu == null || menu.cookType != CookType.Frying) {
            int[] defaultRange = {300, 100, 300};
            firstFryingCount = 2;
            secondFryingCount = 2;
            totalSuccessCount = firstFryingCount + secondFryingCount - 1;
            fryingPanUI.Initialize(defaultRange);
            return;
        } 
        else {
            firstFryingCount = menu.fryingSetting.fryingCount;
            secondFryingCount = menu.fryingSetting.fryingCount;
            totalSuccessCount = firstFryingCount + secondFryingCount - 1;
            fryingPanUI.Initialize(menu.fryingSetting.sectionRange);
        }

    }

    public override IEnumerator UseCookingStep()
    {
        isCanEscape = false;
        yield return StartCoroutine(AddMainIngredient());
        yield return StartCoroutine(InherentMotion(firstFryingCount));
        yield return StartCoroutine(AddSubIngredient());
        yield return StartCoroutine(AddSauce());
        yield return StartCoroutine(InherentMotion(secondFryingCount));
        CookCompleteCheck();
    }

    public override void CookCompleteCheck()
    {
        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {
            if (currentMenu.tossingSetting.tossingCount <= successCount)
            {
                CookSceneManager.instance.UnloadScene("WokMergeTest", currentMenu);
            }
            //fail 조건
            else
            {
                //음쓰 소환
            }
            CookSceneManager.instance.UnloadScene("WokMergeTest", currentMenu);
        }
        else
        {
            if (targetRecipe.cookType != CookType.Frying)
            {
                Debug.Log("Wrong cook type");
                CookSceneManager.instance.UnloadScene();
                return;
            }

            if (!RecipeManager.instance.CompareRecipe(currentMenu, fryingIngredientSystem.checkIngredients))
            {
                Debug.Log("Ingredient mismatch");
                CookSceneManager.instance.UnloadScene();
                return;
            }

            if (successCount < totalSuccessCount)
            {
                Debug.Log("Not enough tossing");
                CookSceneManager.instance.UnloadScene();
                return;
            }

            if (fryingSauceSystem.sauceType != targetRecipe.tossingSetting.sauceType)
            {
                Debug.Log("Wrong sauce type");
                CookSceneManager.instance.UnloadScene();
                return;
            }

            //UnLock New Recipe;
            RecipeManager.instance.RecipeUnLock(targetRecipe);
            UIManager.instance.RecipeUnLockUI();
            CookSceneManager.instance.UnloadScene();
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
            StartCoroutine(cookUIManager.VisiblePanel());
            fryingIngredientSystem.AddSubIngredient(ingredientObject, ingredient);
        }
    }

    //--------------FryingPan System Method------------------//
    IEnumerator InherentMotion(int fryingCount)
    {
        fryingPanUI.OnFryingPanUI();
        fryingSystem.InitializeInherentMotion();
        yield return StartCoroutine(fryingSystem.InherentMotion(fryingCount, (callbackValue) =>
        {
            successCount += callbackValue;
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
        
        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            StartCoroutine(cookUIManager.TimerStart());
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
                if (cookUIManager.TimerEnd()) { break; }
            }
            yield return null;
        }
        yield return StartCoroutine(cookUIManager.HidePanel());
    }

}
