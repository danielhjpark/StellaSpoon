using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class WokManager : CookManagerBase
{
    [Header("Audio")]
    [SerializeField] WokAudioSystem wokAudioSystem;

    [Header("UI Objects")]
    [SerializeField] WokUI wokUI;
    [SerializeField] CookUIManager cookUIManager;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("Transform Objects")]
    [SerializeField] Transform dropPos;
    [SerializeField] GameObject dropIngredient;

    [Header("Wok System")]
    WokTossingSystem wokTossingSystem;
    WokSauceSystem wokSauceSystem;
    WokIngredientSystem wokIngredientSystem;

    //----------------------------------------------------------------------//
    private List<GameObject> wokIngredients = new List<GameObject>();
    private List<IngredientAmount> checkIngredients = new List<IngredientAmount>();
    private List<IngredientAmount> currentIngredients = new List<IngredientAmount>();
    //----------------------------------------------------------------------//
    private int firstTossingCount, secondTossingCount;
    private int successTossingCount;
    private int totalTossingCount;

    private GameObject mainIngredient;
    //---------------------------------------------------------------------//

    void Awake()
    {
        CookManager.instance.BindingManager(this);
        CookManager.instance.spawnPoint = dropPos;
        cookUIManager.Initialize(this);
    }

    void Start()
    {
        wokTossingSystem = GetComponent<WokTossingSystem>();
        wokSauceSystem = GetComponent<WokSauceSystem>();
        wokIngredientSystem = GetComponent<WokIngredientSystem>();

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

    public override void SelectRecipe(Recipe menu)
    {
        base.SelectRecipe(menu);
        firstTossingCount = menu.tossingSetting.firstTossingCount;
        secondTossingCount = menu.tossingSetting.secondTossingCount;
        totalTossingCount = firstTossingCount + secondTossingCount - 1;
        StartCoroutine(UseCookingStep());
    }

    public void RecipeSetting(Recipe menu)
    {
        base.SelectRecipe(menu);
        if (menu == null || menu.cookType != CookType.Tossing)
        {
            //int[] defaultRange = {300, 150, 300};
            int randTossingCount = 2;
            firstTossingCount = randTossingCount;
            secondTossingCount = randTossingCount;
            return;
        }
        else
        {
            firstTossingCount = menu.tossingSetting.firstTossingCount;
            secondTossingCount = menu.tossingSetting.secondTossingCount;
        }

    }

    public override void CookCompleteCheck()
    {
        //success
        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {
            if (totalTossingCount <= successTossingCount)
            {
                CookSceneManager.instance.UnloadScene("WokMergeTest", currentMenu);
                return;
            }
            //fail ���� ��ȯ
            else
            {
                CookSceneManager.instance.UnloadScene("WokMergeTest", CookManager.instance.failMenu);
                return;
            }

        }
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
                Debug.Log("Wrong sauce type");
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
            wokIngredientSystem.AddMainIngredient(ingredients, ingredient);
        }
        else
        {
            //StartCoroutine(cookUIManager.VisiblePanel());
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
            yield return new WaitForSeconds(0.5f);
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
                if (RecipeManager.instance.CompareRecipe(currentMenu, wokIngredientSystem.checkIngredients)) break;
            }
            else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
            {
                if (cookUIManager.TimerEnd()) { break; }
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

        while (!wokSauceSystem.IsLiquidFilled())
        {
            yield return null;
        }
    }

}
