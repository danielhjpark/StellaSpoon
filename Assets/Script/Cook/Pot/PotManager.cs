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
    private PotAudioSystem potAudioSystem;
    private PotUI potUI;

    [Header("UI Objects")]
    [SerializeField] CookUIManager cookUIManager;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("Transform Objects")]
    [SerializeField] Transform dropPos;
    [SerializeField] GameObject dropIngredient;

    [Header("Disable Objects")]
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject potViewCamera;
    [SerializeField] GameObject uiObject;

    private Ingredient mainIngredient;
    private List<GameObject> potIngredients = new List<GameObject>();
    private List<IngredientAmount> checkIngredients = new List<IngredientAmount>();

    private int cookCompleteTime;
    private int decreaseCompleteTime;

    void Awake()
    {
        isCanEscape = true;
        CookManager.instance.BindingManager(this);
        CookManager.instance.spawnPoint = dropPos;
        CookManager.instance.isCanUseSideTable = false;
        cookUIManager.Initialize(this);

        potBoilingSystem = GetComponent<PotBoilingSystem>();
        potSauceSystem = GetComponent<PotSauceSystem>();
        potViewportSystem = GetComponent<PotViewportSystem>();
        potAudioSystem = GetComponent<PotAudioSystem>();
        potUI = GetComponent<PotUI>();

        UpgradePot();//Store Unlock upgrade
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isCanEscape)
            {
                CloseSceneView();
            }
            else
            {
                CookManager.instance.isCanUseSideTable = true;
                CookSceneManager.instance.UnloadScene("PotMergeTest");
            }
        }
    }

    private void UpgradePot()
    {
        int unlockStep = RestaurantManager.instance.currentPotLevel;
        switch (unlockStep)
        {
            case 0:
                decreaseCompleteTime = 0;
                break;
            case 1:
                decreaseCompleteTime = 1;
                break;
            case 2:
                decreaseCompleteTime = 3;
                break;
            case 3:
                decreaseCompleteTime = 5;
                break;
        }
    }

    //--------------- Recipe Mode ----------------------//
    public override void SelectRecipe(Recipe menu)
    {
        isCanEscape = false;
        base.SelectRecipe(menu);
        cookCompleteTime = currentMenu.boilingSetting.cookTime;
        cookCompleteTime -= decreaseCompleteTime;
        StartCoroutine(UseCookingStep());
    }

    public void MakeRecipe(Recipe menu)
    {
        isCanEscape = false;
        base.SelectRecipe(menu);

        if (menu == null || menu.cookType != CookType.Boiling)
        {
            cookCompleteTime = 20;
        }
        else
        {
            cookCompleteTime = currentMenu.boilingSetting.cookTime;
        }
        cookCompleteTime -= decreaseCompleteTime;
    }
    //---------------------------------------------------//
    public override void AddIngredient(GameObject obj, Ingredient ingredient)
    {
        obj.transform.position = dropPos.position;
        AddIngredientList(obj);
        StartCoroutine(cookUIManager.VisiblePanel());

        if (ingredient.ingredientType == IngredientType.Main)
        {
            mainIngredient = ingredient;
            potAudioSystem.StartAudioSource(PotAudioSystem.AudioType.MainIngredientDrop);
            //IngredientAddAmount(checkIngredients, ingredient, 1);
            return;
        }
        if (ingredient.ingredientType == IngredientType.Sub)
        {
            IngredientAddAmount(checkIngredients, ingredient, ingredient.ingredientUseCount);
            potAudioSystem.StartAudioSource(PotAudioSystem.AudioType.SubIngredientDrop);
        }

    }

    public override IEnumerator UseCookingStep()
    {
        yield return StartCoroutine(AddAllIngredients());
        yield return StartCoroutine(AddSauce());
        yield return StartCoroutine(potViewportSystem.ButtonView());
        yield return StartCoroutine(InherentMotion());
        CookCompleteCheck();
    }

    public override void CookCompleteCheck()
    {
        string currentSceneName = "PotMergeTest";
        if (cookMode == CookMode.Select)
        {
            if (currentMenu.boilingSetting.rotatePower != potBoilingSystem.rotatePower)
            {
                CookSceneManager.instance.UnloadScene("PotMergeTest", CookManager.instance.failMenu);
                OrderManager.instance.FailMenu(currentMenu);
            }
            else
            {
                CookSceneManager.instance.UnloadScene("PotMergeTest", currentMenu);
            }
            return;
        }

        if (targetRecipe.cookType != CookType.Boiling)
        {
            Debug.Log("Wrong cook type");
            CookSceneManager.instance.UnloadScene(currentSceneName, CookManager.instance.failMenu);
            return;
        }

        if (!RecipeManager.instance.CompareRecipe(currentMenu, checkIngredients))
        {
            Debug.Log("Ingredient mismatch");
            CookSceneManager.instance.UnloadScene(currentSceneName, CookManager.instance.failMenu);
            return;
        }

        if (potSauceSystem.sauceType != targetRecipe.boilingSetting.sauceType)
        {
            Debug.Log("Wrong sauce type");
            CookSceneManager.instance.UnloadScene(currentSceneName, CookManager.instance.failMenu);
            return;
        }

        if (targetRecipe.boilingSetting.rotatePower != potBoilingSystem.rotatePower)
        {
            Debug.Log("Wrong rotatePower");
            CookSceneManager.instance.UnloadScene(currentSceneName, CookManager.instance.failMenu);
            return;
        }

        //UnLock New Recipe;
        RecipeManager.instance.RecipeUnLock(targetRecipe);
        CookSceneManager.instance.UnloadScene("PotMergeTest", currentMenu);
        Debug.Log("Success");
        return;
    }
    //-------------------UseCookingStep -------------------//
    public IEnumerator AddAllIngredients()
    {
        potViewportSystem.PutIngredient();
        StartCoroutine(potViewportSystem.OpenLid());

        //Select && Make Choice
        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {
            isCanEscape = false;
            StartCoroutine(cookUIManager.VisiblePanel());
            ingredientInventory.IngredientAdd(currentMenu.mainIngredient);
        }
        else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            ingredientInventory.AddAllIngredients();
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(cookUIManager.TimerStart(10f));

            //if drop ingredient in pot can't escape this scene
            yield return new WaitUntil(() => potIngredients.Count > 0);
            isCanEscape = false;

            // Find recipe
            yield return new WaitUntil(() => mainIngredient != null);
            targetRecipe = RecipeManager.instance.FindRecipe(mainIngredient);
            MakeRecipe(targetRecipe);

            yield return new WaitForSeconds(0.5f);
        }

        //All of ingredient drop or timeover escape loop
        while (true)
        {
            if (CookManager.instance.cookMode == CookManager.CookMode.Select)
            {
                if (RecipeManager.instance.CompareRecipe(currentMenu, checkIngredients) && mainIngredient != null) { break; }
            }
            else if (CookManager.instance.cookMode == CookManager.CookMode.Make)
            {
                if (cookUIManager.TimerEnd() || currentSubIngredient >= maxSubIngredient) { break; }
            }
            yield return null;
        }
        CookManager.instance.isCanIngredientControll = false;
        yield return StartCoroutine(cookUIManager.HidePanel());

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
            if (currentMenu.boilingSetting.sauceType == SauceType.None)
            {
                yield break;
            }
            else
            {
                potSauceSystem.Initialize(currentMenu.boilingSetting);
            }
        }

        StartCoroutine(cookUIManager.TimerStart(5f));
        while (true)
        {
            if ((cookUIManager.TimerEnd() && !potSauceSystem.startLiquidFilled) || potSauceSystem.IsLiquidFilled())
            {
                cookUIManager.TimerStop();
                break;
            }
            yield return null;
        }
        potUI.SetActiveBottomButton();
    }

    public IEnumerator InherentMotion()
    {
        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            potBoilingSystem.Initialize(cookCompleteTime, potIngredients);
        }
        else
        {
            potBoilingSystem.Initialize(currentMenu.boilingSetting.cookTime, potIngredients); // Binding Setting
        }

        yield return StartCoroutine(potBoilingSystem.StartBoilingSystem()); // Call of Button

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
            ingredient.GetComponent<Rigidbody>().mass = 10;
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }
    }

    //---------------SceneView Controll--------------//
    public void OpenSceneView()
    {
        potAudioSystem.UnPauseAudioSource(PotAudioSystem.AudioType.RotaitionPot);
        mainCamera.SetActive(true);
        CookSceneManager.instance.mainCamera.transform.gameObject.SetActive(false);
        potViewCamera.SetActive(true);
        CanvasGroup canvasGroup = uiObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Change to MainCamera
    public void CloseSceneView()
    {
        potAudioSystem.PauseAudioSource(PotAudioSystem.AudioType.RotaitionPot);
        mainCamera.SetActive(false);
        CookSceneManager.instance.mainCamera.transform.gameObject.SetActive(true);
        potViewCamera.SetActive(false);
        CanvasGroup canvasGroup = uiObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false; // 클릭 불가능하게 설정 (필요 시)
        canvasGroup.blocksRaycasts = false; // 레이캐스트 차단 (필요 시)


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
