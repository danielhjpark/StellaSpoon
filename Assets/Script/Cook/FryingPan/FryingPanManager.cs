using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;

public class FryingPanManager : CookManagerBase
{
    [Header("UI Object")]
    [SerializeField] FryingPanUI fryingPanUI;
    [SerializeField] CookUIManager cookUIManager;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("Set Objects")]
    [SerializeField] private Transform dropPos;
    [SerializeField] private GameObject dropIngredient;

    [Header("FryingPan System")]
    [SerializeField] FryingSystem fryingSystem;
    [SerializeField] FryingSauceSystem fryingSauceSystem;

    private List<GameObject> fryingIngredients = new List<GameObject>();
    private List<IngredientAmount> checkIngredients;
    private List<IngredientAmount> currentIngredients;
    //---------------------------------------//
    private GameObject currentIngredient;
    public int firstFryingCount, secondFryingCount;
    public int successFryingCount;

    void Awake()
    {
        CookManager.instance.BindingManager(this);
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

    public override void AddIngredient(GameObject obj, Ingredient ingredient)
    {
        currentIngredient = obj;
        obj.transform.position = dropPos.position;
        obj.transform.SetParent(dropIngredient.transform);
        IngredientAddAmount(checkIngredients, ingredient, 1);
        AddIngredientList(obj);
        StartCoroutine(DropIngredient());
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
            fryingIngredients.Add(ingredient.gameObject);
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }
    }

    IEnumerator DropIngredient()
    {
        fryingPanUI.OnFryingPanUI();
        float time = 0;
        while (true)
        {
            time += Time.deltaTime * 5;
            currentIngredient.transform.localPosition = Vector3.Lerp(dropPos.localPosition, Vector3.zero, time);
            if (currentIngredient.transform.localPosition.y <= 0) break;
            yield return null;
        }
    }

    //--------------FryingPan System Method------------------//
    IEnumerator InherentMotion(int fryingCount)
    {
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
        targetRecipe = FindRecipe(checkIngredients[0].ingredient);
        currentIngredients = targetRecipe.ingredients;
        checkIngredients.Clear();
        // MakeRecipe();
    }

    IEnumerator AddSubIngredient()
    {
        //wokUI.OnFridgeUI();
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
                if (CompareIngredient(currentIngredients, checkIngredients)) break;
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

}
