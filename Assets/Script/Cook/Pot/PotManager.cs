using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PotManager : CookManagerBase
{
    private PotBoilingSystem potBoilingSystem;
    private PotSauceSystem potSauceSystem;
    private PotViewportSystem potViewportSystem;
    private PotIngredientSystem potIngredientSystem;

    [Header("UI Objects")]
    [SerializeField] CookUIManager cookUIManager;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("Transform Objects")]
    [SerializeField] Transform dropPos;
    [SerializeField] GameObject dropIngredient;

    [Header("Disable Objects")]
    [SerializeField] GameObject potViewCamera;
    [SerializeField] GameObject uiObject;

    //--------------- Save List ------------------------//
    private Ingredient mainIngredient;
    private List<GameObject> potIngredients = new List<GameObject>();
    private List<IngredientAmount> checkIngredients = new List<IngredientAmount>();
    //------------------------------------------------//

    void Awake()
    {
        CookManager.instance.BindingManager(this);
        cookUIManager.Initialize(this);
        
        potBoilingSystem = GetComponent<PotBoilingSystem>();
        potSauceSystem = GetComponent<PotSauceSystem>();
        potViewportSystem = GetComponent<PotViewportSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSceneView();
        }
    }

    //--------------- virtual Method ----------------------//
    public override void SelectRecipe(Recipe menu)
    {
        base.SelectRecipe(menu);
        //potBoilingSystem.Initialize(menu.boilingSetting);
        StartCoroutine(UseCookingStep());
    }

    public override IEnumerator UseCookingStep()
    {
        yield return StartCoroutine(AddAllIngredients());
        yield return StartCoroutine(AddSauce());
        yield return StartCoroutine(InherentMotion());
        CookCompleteCheck();
    }

    public IEnumerator AddAllIngredients()
    {
        potViewportSystem.PutIngredient();
        Debug.Log("Ingredients Step");
        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {
            ingredientInventory.AddAllIngredientsToRecipe(currentMenu);
        }
        else if(CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            ingredientInventory.AddAllIngredients();
            StartCoroutine(cookUIManager.TimerStart());
        }
        while (true)
        {
            if (CookManager.instance.cookMode == CookManager.CookMode.Select)
            {
                if (RecipeManager.instance.CompareRecipe(currentMenu, checkIngredients)) { break; }
            }
            else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
            {
                if (cookUIManager.TimerEnd()) { break; }
            }
            yield return null;
        }
        yield return StartCoroutine(cookUIManager.HidePanel());

    }

    public override void CookCompleteCheck()
    {
        List<IngredientAmount> checkIngredients = new List<IngredientAmount>();
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

        if (potSauceSystem.sauceType != targetRecipe.tossingSetting.sauceType)
        {
            Debug.Log("Wrong sauce type");
            return;
        }

        //UnLock New Recipe;
        RecipeManager.instance.RecipeUnLock(targetRecipe);
        CookSceneManager.instance.UnloadScene("PotMergeTest", currentMenu);
        Debug.Log("Success");
        return;
    }

    IEnumerator AddSauce()
    {
        Debug.Log("Sauce Step");
        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            potSauceSystem.InitializeMakeMode();
        }
        else
        {
            if (currentMenu.tossingSetting.sauceType == SauceType.None)
            {
                yield break;
            }
            else
            {
                potSauceSystem.Initialize(currentMenu.boilingSetting);
            }
        }

        while (!potSauceSystem.IsLiquidFilled())
        {
            yield return null;
        }
        potViewportSystem.BoilingPot();
    }


    public IEnumerator InherentMotion()
    {
        potBoilingSystem.Initialize(currentMenu.boilingSetting, potIngredients);
        yield return StartCoroutine(potBoilingSystem.StartBoilingSystem());
        yield return StartCoroutine(cookUIManager.TimerStart());
    }



    public override void AddIngredient(GameObject obj, Ingredient ingredient)
    {
        obj.transform.position = dropPos.position;
        AddIngredientList(obj);
        StartCoroutine(cookUIManager.VisiblePanel());

        if(ingredient.ingredientType == IngredientType.Main) {
            mainIngredient = ingredient;
            return;
        }
        IngredientAddAmount(checkIngredients, ingredient, 1);
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


    //-------------------------------------------------------------//

    public void AddIngredientList(GameObject ingredients)
    {
        ingredients.transform.SetParent(dropIngredient.transform);
        foreach (Transform ingredient in ingredients.transform)
        {
            potIngredients.Add(ingredient.gameObject);
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }
    }


    //---------------SceneView Controll--------------//
    public void OpenSceneView()
    {
        potViewCamera.SetActive(true);
        uiObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Change to MainCamera
    public void CloseSceneView()
    {
        potViewCamera.SetActive(false);
        uiObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



}
