using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class WokManager : CookManagerBase
{
    [Header("UI Objects")]
    [SerializeField] WokUI wokUI;
    [SerializeField] CookUIManager cookUIManager;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("Transform Objects")]
    [SerializeField] Transform dropPos;
    [SerializeField] GameObject dropIngredient;

    [Header("Wok System")]
    [SerializeField] WokTossingSystem wokTossingSystem;
    [SerializeField] WokSauceSystem wokSauceSystem;
    //----------------------------------------------------------------------//
    private List<GameObject> wokIngredients = new List<GameObject>();
    private List<IngredientAmount> checkIngredients = new List<IngredientAmount>();
    //----------------------------------------------------------------------//
    private int firstTossingCount, secondTossingCount;
    private int successTossingCount;
    private int targetTossingCount;
    private List<IngredientAmount> currentIngredients = new List<IngredientAmount>();
    //---------------------------------------------------------------------//

    void Awake()
    {
        CookManager.instance.BindingManager(this);
        cookUIManager.Initialize(this);
    }

    void Start()
    {
        wokTossingSystem = GetComponent<WokTossingSystem>();
        successTossingCount = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CookSceneManager.instance.UnloadScene();
        }
    }

    //--------------------------Virtual Method -----------------------------//

    public override IEnumerator UseCookingStep()
    {
        yield return StartCoroutine(AddMainIngredient());//Sub ingredient add
        yield return StartCoroutine(InherentMotion(firstTossingCount));
        yield return StartCoroutine(AddSubIngredient());//Sub ingredient add
        yield return StartCoroutine(AddSauce());//Sauce motion
        yield return StartCoroutine(InherentMotion(secondTossingCount));

        CookCompleteCheck();
    }

    public override void SelectRecipe(Recipe menu)
    {
        base.SelectRecipe(menu);
        firstTossingCount = menu.tossingSetting.tossingCount;
        secondTossingCount = menu.tossingSetting.tossingCount;
        StartCoroutine(UseCookingStep());
    }

    public void MakeRecipe()
    {
        int randTossingCount = 2;
        if (targetRecipe.tossingSetting.tossingCount <= 0)
        {
            firstTossingCount = randTossingCount;
            secondTossingCount = randTossingCount;
            return;
        }
        firstTossingCount = targetRecipe.tossingSetting.tossingCount + 1;
        secondTossingCount = targetRecipe.tossingSetting.tossingCount + 1;
        targetTossingCount = firstTossingCount + secondTossingCount - 1;
    }


    public override void CookCompleteCheck()
    {
        //success
        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {
            if (currentMenu.tossingSetting.tossingCount <= successTossingCount)
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
            if (targetRecipe.cookType != CookType.Tossing)
            {
                Debug.Log("Wrong cook type");
                return;
            }

            if (!RecipeManager.instance.CompareRecipe(currentMenu, checkIngredients))
            {
                Debug.Log("Ingredient mismatch");
                return;
            }

            if (successTossingCount < targetTossingCount)
            {
                Debug.Log("Not enough tossing");
                return;
            }

            if (wokSauceSystem.sauceType != targetRecipe.tossingSetting.sauceType)
            {
                Debug.Log("Wrong sauce type");
                return;
            }

            //UnLock New Recipe;
            RecipeManager.instance.RecipeUnLock(targetRecipe);
            Debug.Log("Success");
            return;

        }
    }
    public override void AddIngredient(GameObject ingredients, Ingredient ingredient)
    {
        ingredients.transform.position = dropPos.position;
        IngredientAddAmount(checkIngredients, ingredient, 1);
        AddIngredientList(ingredients);
        StartCoroutine(cookUIManager.VisiblePanel());
    }

    void IngredientAddAmount(List<IngredientAmount> list, Ingredient ingredient, int count)
    {
        var existing = list.FirstOrDefault(i => i.ingredient.Equals(ingredient));
        if (existing != null)
        {
            existing.amount += count;
        }
        else
        {
            list.Add(new IngredientAmount(ingredient, count));
        }
    }

    //----------------------------------------------------------------------//
    public void AddIngredientList(GameObject ingredients)
    {
        ingredients.transform.SetParent(dropIngredient.transform);
        foreach (Transform ingredient in ingredients.transform)
        {
            wokIngredients.Add(ingredient.gameObject);
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }

    }

    //---------------Wok Cooking Method---------------//

    IEnumerator InherentMotion(int tossingCount)
    {
        wokTossingSystem.BindTossingObject(wokIngredients);
        yield return StartCoroutine(wokTossingSystem.WokTossing(tossingCount, (callbackValue) =>
        {
            successTossingCount += callbackValue;
        }
        ));
        Debug.Log(successTossingCount);
    }

    IEnumerator AddMainIngredient()
    {
        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {
            GameObject mainIngredient = currentMenu.mainIngredient.ingredientPrefab;
            AddIngredient(Instantiate(mainIngredient, Vector3.zero, Quaternion.identity), currentMenu.mainIngredient);
            yield return new WaitForSeconds(0.5f);
            yield break;
        }
        else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            ingredientInventory.AddMainIngredients();
        }

        int currentCount = checkIngredients.Count;
        while (true)
        {
            if (currentCount < checkIngredients.Count)
            {
                break;
                //cookUIManager.TimerReset();
            }

            yield return null;
        }
        targetRecipe = RecipeManager.instance.FindRecipe(checkIngredients[0].ingredient);
        checkIngredients.Clear();
        MakeRecipe();
    }

    IEnumerator AddSubIngredient()
    {
        wokUI.OnFridgeUI();
        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            StartCoroutine(cookUIManager.TimerStart());
            ingredientInventory.AddSubIngredients();
        }

        int currentCount = checkIngredients.Count;
        while (true)
        {
            if (CookManager.instance.cookMode == CookManager.CookMode.Select)
            {
                if (RecipeManager.instance.CompareRecipe(currentMenu, checkIngredients)) break;
            }
            else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
            {
                if (cookUIManager.TimerEnd()) { break; }
                else if (currentCount < checkIngredients.Count)
                {
                    currentCount = checkIngredients.Count;
                    cookUIManager.TimerReset();
                }
            }
            yield return null;
        }
        yield return StartCoroutine(cookUIManager.HidePanel());
    }

    IEnumerator AddSauce()
    {
        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            wokSauceSystem.InitializeMakeMode();
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
