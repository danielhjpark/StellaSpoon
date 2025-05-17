using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class WokManager : CookManagerBase
{
    private WokAudioSystem wokAudioSystem;
    private WokTossingSystem wokTossingSystem;
    private WokSauceSystem wokSauceSystem;
    private WokIngredientSystem wokIngredientSystem;

    [Header("UI Objects")]
    [SerializeField] WokUI wokUI;
    [SerializeField] CookUIManager cookUIManager;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("Transform Objects")]
    [SerializeField] Transform dropPos;

    //Setting
    private int firstTossingCount, secondTossingCount;
    private int successTossingCount;
    private int totalTossingCount;
    private int fireStep;

    void Awake()
    {
        CookManager.instance.BindingManager(this);
        CookManager.instance.spawnPoint = dropPos;
        cookUIManager.Initialize(this);
        //int unlockStep = CookManager.storeUIManager.currentWorLevel;
    }

    void Start()
    {
        isCanEscape = true;
        wokTossingSystem = GetComponent<WokTossingSystem>();
        wokSauceSystem = GetComponent<WokSauceSystem>();
        wokIngredientSystem = GetComponent<WokIngredientSystem>();
        wokAudioSystem = GetComponent<WokAudioSystem>();

        successTossingCount = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isCanEscape)
        {
            CookSceneManager.instance.UnloadScene();
        }
    }

    //--------------------------Virtual Method -----------------------------//

    public override IEnumerator UseCookingStep()
    {
        isCanEscape = false;
        yield return StartCoroutine(AddMainIngredient());//Main ingredient add
        if (firstTossingCount > 0) yield return StartCoroutine(InherentMotion(firstTossingCount));
        yield return StartCoroutine(AddSubIngredient());//Sub ingredient add
        yield return StartCoroutine(AddSauce());//Sauce motion
        yield return StartCoroutine(InherentMotion(secondTossingCount));

        CookCompleteCheck();
    }

    //Select Mode
    public override void SelectRecipe(Recipe menu)
    {
        base.SelectRecipe(menu);

        //Tossing Setting
        firstTossingCount = menu.tossingSetting.firstTossingCount;
        secondTossingCount = menu.tossingSetting.secondTossingCount;
        totalTossingCount = firstTossingCount + secondTossingCount - 1;

        //**Store Setting**//UI Unlock Setting Initialize
        wokUI.Initialize(0);

        //Start Cooking
        StartCoroutine(UseCookingStep());
    }

    //Make Mode
    public void RecipeSetting(Recipe menu)
    {
        base.SelectRecipe(menu);
        //Fail recipe setting
        if (menu == null || menu.cookType != CookType.Tossing)
        {
            int randTossingCount = 2;
            firstTossingCount = randTossingCount;
            secondTossingCount = randTossingCount;
            totalTossingCount = firstTossingCount + secondTossingCount - 1;
        }
        else
        {
            firstTossingCount = menu.tossingSetting.firstTossingCount;
            secondTossingCount = menu.tossingSetting.secondTossingCount;
            totalTossingCount = firstTossingCount + secondTossingCount - 1;
        }
        wokIngredientSystem.InitializeIngredientShader(wokIngredientSystem.mainIngredient, totalTossingCount);
        wokUI.Initialize(0);

    }

    public override void CookCompleteCheck()
    {
        //Select Recipe
        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {
            if (totalTossingCount <= successTossingCount
            && wokUI.CheckFireStep(fireStep))
            {
                CookSceneManager.instance.UnloadScene("WokMergeTest", currentMenu);
            }
            //fail
            else
            {
                CookSceneManager.instance.UnloadScene("WokMergeTest", CookManager.instance.failMenu);
                OrderManager.instance.FailMenu(currentMenu);
            }
            return;
        }
        //Make Recipe
        else
        {
            if (targetRecipe.cookType != CookType.Tossing)
            {
                Debug.Log("Wrong cook type");
                CookSceneManager.instance.UnloadScene("WokMergeTest", CookManager.instance.failMenu);
                return;
            }

            if (!RecipeManager.instance.CompareRecipe(currentMenu, wokIngredientSystem.checkIngredients))
            {
                Debug.Log("Ingredient mismatch");
                CookSceneManager.instance.UnloadScene("WokMergeTest", CookManager.instance.failMenu);
                return;
            }

            if (successTossingCount < totalTossingCount)
            {
                Debug.Log("Not enough tossing");
                CookSceneManager.instance.UnloadScene("WokMergeTest", CookManager.instance.failMenu);
                return;
            }

            if (wokSauceSystem.sauceType != targetRecipe.tossingSetting.sauceType)
            {
                Debug.Log("Wrong sauce type"+wokSauceSystem.sauceType+ ":"+ targetRecipe.tossingSetting.sauceType);
                CookSceneManager.instance.UnloadScene("WokMergeTest", CookManager.instance.failMenu);
                return;
            }

            RecipeManager.instance.RecipeUnLock(targetRecipe);
            CookSceneManager.instance.UnloadScene("WokMergeTest", targetRecipe);
            Debug.Log("Success");
            return;
        }
    }


    public override void AddIngredient(GameObject ingredients, Ingredient ingredient)
    {
        if (ingredient.ingredientType == IngredientType.Main)
        {
            wokIngredientSystem.AddMainIngredient(ingredients, ingredient, totalTossingCount);
        }
        else
        {
            currentSubIngredient++;
            wokIngredientSystem.AddSubIngredient(ingredients, ingredient);
            StartCoroutine(cookUIManager.VisiblePanel());
        }
    }


    //---------------Wok Cooking Method---------------//

    IEnumerator InherentMotion(int tossingCount)
    {
        wokUI.OnWokUI();
        wokTossingSystem.BindTossingObject(wokIngredientSystem.wokIngredients);
        yield return StartCoroutine(wokTossingSystem.WokTossing(tossingCount, (callbackValue) =>
        {
            successTossingCount += callbackValue;
        }));
    }


    IEnumerator AddMainIngredient()
    {
        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {
            //Auto Spawn Main Ingredient 
            GameObject mainIngredient = currentMenu.mainIngredient.ingredientPrefab;
            AddIngredient(Instantiate(mainIngredient, dropPos.position, Quaternion.identity), currentMenu.mainIngredient);

            yield return new WaitUntil(() => wokIngredientSystem.mainIngredient != null);
            yield return new WaitForSeconds(1f);
        }
        else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            ingredientInventory.AddMainIngredients();
            yield return new WaitUntil(() => wokIngredientSystem.mainIngredient != null);
            targetRecipe = RecipeManager.instance.FindRecipe(wokIngredientSystem.checkIngredients[0].ingredient);
            
            RecipeSetting(targetRecipe);
            yield return new WaitForSeconds(0.5f);
        }

        wokIngredientSystem.checkIngredients.Clear();
    }

    IEnumerator AddSubIngredient()
    {
        wokUI.OnFridgeUI();
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
                if (RecipeManager.instance.CompareRecipe(currentMenu, wokIngredientSystem.checkIngredients)
                || currentSubIngredient >= maxSubIngredient) break;
            }
            else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
            {
                if (cookUIManager.TimerEnd() || currentSubIngredient >= maxSubIngredient)
                {
                    cookUIManager.TimerStop();
                    break;
                }
            }
            yield return null;
        }
        yield return StartCoroutine(cookUIManager.HidePanel());
    }

    IEnumerator AddSauce()
    {
        Debug.Log("Add Sauce Step");
        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            wokSauceSystem.InitializeMakeMode(currentMenu.tossingSetting);
        }
        else
        {
            if (currentMenu.tossingSetting.sauceType == SauceType.None)
            {
                yield break;
            }
            else
            {
                wokSauceSystem.Initialize(currentMenu.tossingSetting);
            }
        }

        StartCoroutine(cookUIManager.TimerStart(5f));
        while (true)
        {                
            if (cookUIManager.TimerEnd() || wokSauceSystem.IsLiquidFilled()) { 
                cookUIManager.TimerStop();
                break; 
            }
            yield return null;
        }
    }

}
