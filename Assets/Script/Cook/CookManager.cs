using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CookManager : MonoBehaviour
{
    static public CookManager instance;
    static public StoreUIManager storeUIManager; 
    //---------------- Cook Managers Setting--------------------//
    private CuttingManager cuttingManager;
    private WokManager wokManager;
    private FryingPanManager fryingPanManager;
    private PotManager potManager;

    //--------------- Shared Variable ---------------------//
    [NonSerialized] public CookType currentCookType;
    [NonSerialized] public Transform spawnPoint;
    [NonSerialized] public Recipe currentMenu;
    [SerializeField] public Recipe failMenu;
    [SerializeField] public GameObject TimerObject;
    [SerializeField][Range(1f, 100f)] public float tiltSauceContainerAcceleration;
    [SerializeField][Range(1f, 100f)] public float SauceAcceleration;
    [SerializeField][Range(1f, 100f)] public float SlideAcceleration;

    //Use Global
    public enum CookMode { Select, Make };
    [NonSerialized] public CookMode cookMode;
    [NonSerialized] public bool isCanIngredientControll;
    [NonSerialized] public bool isCanUseMiddleTable = true;
    [NonSerialized] public bool isCanUseSideTable = true;
    [NonSerialized] public bool isPickUpMenu = false;

    void Awake()
    {
        instance = this;
        storeUIManager = FindObjectOfType<StoreUIManager>();
    }

    public void BindingManager<T>(T manager) where T : CookManagerBase
    {
        spawnPoint = GameObject.FindWithTag("SpawnPoint")?.transform;
        isCanIngredientControll = true;
        if (manager is CuttingManager)
        {
            cuttingManager = manager as CuttingManager;
            currentCookType = CookType.Cutting;
        }
        else if (manager is WokManager)
        {
            wokManager = manager as WokManager;
            currentCookType = CookType.Tossing;
        }
        else if (manager is FryingPanManager)
        {
            fryingPanManager = manager as FryingPanManager;
            currentCookType = CookType.Frying;
        }
        else if (manager is PotManager)
        {
            potManager = manager as PotManager;
            currentCookType = CookType.Boiling;
        }
    }

    public void InteractObject(string objName)
    {
        if (!CookSceneManager.instance.isSceneLoaded)
        {
            if (isPickUpMenu) return;
            //cookMode = CookMode.Select;

            switch (objName)
            {
                case "CuttingBoard":
                    InteractOtherObject(objName);
                    break;
                case "Pot":
                    InteractPotObject();
                    break;
                case "Pan":
                    if (!isCanUseMiddleTable) return;
                    InteractOtherObject(objName);
                    break;
                case "Wok":
                    if (!isCanUseMiddleTable) return;
                    InteractOtherObject(objName);
                    break;
                default:
                    break;
            }
        }
    }

    public void InteractOtherObject(string objName)
    {
        CookSceneManager.instance.LoadScene(objName);
    }

    public void InteractPotObject()
    {
        const string potSceneName = "PotMergeTest";
        CookSceneManager.instance.mainCamera.transform.gameObject.SetActive(false);
        if (CookSceneManager.instance.IsSceneLoaded(potSceneName))
        {
            potManager.OpenSceneView();
        }
        else
        {
            SceneManager.LoadScene(potSceneName, LoadSceneMode.Additive);
            CookSceneManager.instance.currentSceneName = potSceneName;
            CookSceneManager.instance.isSceneLoaded = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SelectRecipe(Recipe currentMenu)
    {
        switch (CookSceneManager.instance.currentSceneName)
        {
            case "CuttingBoardMergeTest":
                cuttingManager.SelectRecipe(currentMenu);
                break;
            case "PotMergeTest":
                potManager.SelectRecipe(currentMenu);
                break;
            case "FryingPanMergeTest":
                fryingPanManager.SelectRecipe(currentMenu);
                break;
            case "WokMergeTest":
                wokManager.SelectRecipe(currentMenu);
                break;
        }
    }

    public void DropObject(GameObject ingredientObject, Ingredient ingredient)
    {
        switch (currentCookType)
        {
            case CookType.Cutting:
                cuttingManager.AddIngredient(ingredientObject, ingredient);
                break;
            case CookType.Frying:
                fryingPanManager.AddIngredient(ingredientObject, ingredient);
                break;
            case CookType.Tossing:
                wokManager.AddIngredient(ingredientObject, ingredient);
                break;
            case CookType.Boiling:
                potManager.AddIngredient(ingredientObject, ingredient);
                break;

        }
    }

}
