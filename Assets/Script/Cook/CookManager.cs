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

    [SerializeField] Transform playerTransfom;
    [SerializeField] LayerMask utensilLayer;
    [SerializeField] LayerMask menuLayer;
    [SerializeField] Text actionText;
    [SerializeField] ServeController serveController;
    float range = 1f;

    private const string potSceneName = "PotMergeTest";

    void Awake()
    {
        instance = this;  
    }

    void Update() {
        CheckLayer();        
    }
    
    public void BindingManager<T>(T manager) where T : CookManagerBase
    {
        spawnPoint = GameObject.FindWithTag("SpawnPoint")?.transform;
        if (manager is CuttingManager) {
            cuttingManager = manager as CuttingManager;
            currentCookType = CookType.Cutting;
        }
        else if (manager is WokManager) {
            wokManager = manager as WokManager;
            currentCookType = CookType.Tossing;
        }
        else if (manager is FryingPanManager) {
            fryingPanManager = manager as FryingPanManager;
            currentCookType = CookType.Frying;
        }
        else if (manager is PotManager) {
            potManager = manager as PotManager;
            currentCookType = CookType.Boiling;
        }
    }

    public void InteractOtherObject(string objName) {
        CookSceneManager.instance.LoadScene(objName); 
    }

    public void InteractPotObject() {
        if(CookSceneManager.instance.IsSceneLoaded(potSceneName)) {
            if(potManager.CheckCookCompleted()) {
                //PutDownMenu();
                //serveController.PickUpMenu(currentMenu);
            }
            else {
                potManager.OpenSceneView();
            }
        }
        else {
            SceneManager.LoadScene(potSceneName, LoadSceneMode.Additive);
            CookSceneManager.instance.currentSceneName = potSceneName;
            CookSceneManager.instance.isSceneLoaded = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    private void CheckLayer() {
        Vector3 rayOrigin = playerTransfom.position + Vector3.up * 0.5f; // 캐릭터 중심에서 약간 위로
        Vector3 rayDirection = playerTransfom.forward; // 캐릭터의 forward 방향
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, utensilLayer))
        {
            ChangeActionText("Utensil");
            ShowActionText(hitInfo.transform.name);
            if(Input.GetKeyDown(KeyCode.F)) {
                InteractObject(hitInfo.transform.name);
            }
            
        }
        else if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, menuLayer))
        {
            ChangeActionText("Menu");
            ShowActionText(hitInfo.transform.name);
            if(Input.GetKeyDown(KeyCode.F)) {
                serveController.PickUpMenu(hitInfo.transform.gameObject);
                Destroy(hitInfo.transform.gameObject);
            }
            
        }
        else actionText.gameObject.SetActive(false);
    }


    private void ChangeActionText(string textType) {
        string defaultText = "Press F";
        string utensilText = "Use Utensil Press F";
        string menuText = "Pick Up Menu Press F";

        switch(textType) {
            case "Menu" : 
                actionText.text = menuText;
                break;
            case "Utensil" :
                actionText.text = utensilText;
                break;
            default :
                actionText.text = defaultText;
                break;
        }

    }

    private void ShowActionText(string hitInfoName) {

        if(CookSceneManager.instance.isSceneLoaded ||(CookSceneManager.instance.IsSceneLoaded(potSceneName) && hitInfoName == "Pot")) {
            actionText.gameObject.SetActive(false);
        }
        else actionText.gameObject.SetActive(true);

    }

    private void InteractObject(string objName) {
        if (!CookSceneManager.instance.isSceneLoaded)
        {
            switch(objName) {
                case "CuttingBoard":
                    InteractOtherObject(objName);
                    break;
                case "Pot":
                    InteractPotObject();
                    break;
                case "Pan":
                    InteractOtherObject(objName);
                    break;
                case "Wok":
                    InteractOtherObject(objName);
                    break;
                default:
                    break;
            }
        }
    }

    public void SelectRecipe(Recipe currentMenu) {
        switch (CookSceneManager.instance.currentSceneName)  {
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


    public void DropObject(GameObject ingredientObject, Ingredient ingredient) {
        switch(ingredient.ingredientCookType) {
            case IngredientCookType.Cutting:
                cuttingManager.AddIngredient(ingredientObject, ingredient);
                break;
            case IngredientCookType.Frying:
                fryingPanManager.AddIngredient(ingredientObject, ingredient);
                break;
            case IngredientCookType.Tossing:
                wokManager.AddIngredient(ingredientObject, ingredient);
                break;
            case IngredientCookType.Boiling:
                potManager.AddIngredient(ingredientObject, ingredient);
                break;

        }        
    }

}
