using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PotManager : CookManagerBase
{
    [Header("UI Objects")]
    [SerializeField] CookUIManager cookUIManager;
    [SerializeField] IngredientInventory ingredientInventory;

    [Header("Transform Objects")]
    [SerializeField] Transform dropPos;
    [SerializeField] GameObject dropIngredient;
    [SerializeField] Transform centerPos;

    [Header("Disable Objects")]
    [SerializeField] GameObject potViewCamera;
    [SerializeField] GameObject uiObject;

    [Header("Button UI Text")]
    [SerializeField] TextMeshProUGUI powerText;

    [Header("Wok System")]
    [SerializeField] PotSauceSystem potSauceSystem;
    private PotViewportSystem potViewportSystem;

    //--------------- Save List ------------------------//
    private List<GameObject> potIngredients = new List<GameObject>();
    private List<Ingredient> checkIngredients = new List<Ingredient>();
    private List<IngredientAmount> currentIngredients = new List<IngredientAmount>();
    //-------------Pot Rotate Setting ----------------//
    private float rotatePower = 0;
    public float radius;
    public bool applyRight = true;
    private Coroutine ingredientRotateCoroutine;

    //------------------------------------------------//
    private float completeTime;
    private float currentTime;

    void Awake()
    {
        CookManager.instance.BindingManager(this);
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
        potViewportSystem.PutIngredient();
        completeTime = 10f; //수정 필요
        currentTime = 0f;
        StartCoroutine(UseCookingStep());
    }

    public override IEnumerator UseCookingStep()
    {
        yield return StartCoroutine(AddAllIngredients());
        yield return StartCoroutine(AddSauce());
        yield return StartCoroutine(InherentMotion());

    }

    public IEnumerator AddAllIngredients()
    {
        Debug.Log("Ingredients Step");
        if (CookManager.instance.cookMode == CookManager.CookMode.Select)
        {

        }
        else
        {

        }
        while (true)
        {
            if (CheckRequireIngredient()) break;
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

        if (!CompareIngredient(currentIngredients, checkIngredients))
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

    public bool CompareIngredient(List<IngredientAmount> currentIngredients, List<IngredientAmount> targetIngredients)
    {
        bool isCompare = currentIngredients.Count == targetIngredients.Count;
        int findCompareCount = 0;

        foreach (IngredientAmount currentIngredient in currentIngredients)
        {
            foreach (IngredientAmount targetIngredient in targetIngredients)
            {
                if (currentIngredient.ingredient.ingredientName == targetIngredient.ingredient.ingredientName)
                {
                    if (currentIngredient.amount == targetIngredient.amount)
                    {
                        findCompareCount++;
                        break;
                    }
                }
            }
        }
        if (isCompare) return currentIngredients.Count == findCompareCount;
        else return isCompare;
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
        yield return null;
    }





    public override void AddIngredient(GameObject obj, Ingredient ingredient)
    {
        obj.transform.position = dropPos.position;
        checkIngredients.Add(ingredient);
        AddIngredientList(obj);
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

    //-------------------Button----------------------------------//

    public void OnIncreasePower()
    {
        if (rotatePower < 3) rotatePower++;
        powerText.text = rotatePower.ToString();
        if (ingredientRotateCoroutine == null)
        {
            ingredientRotateCoroutine = StartCoroutine(AddForceWithRotation());
        }

    }

    public void OnDecreasePower()
    {
        if (rotatePower > 0) rotatePower--;
        powerText.text = rotatePower.ToString();
        if (ingredientRotateCoroutine == null)
        {
            ingredientRotateCoroutine = StartCoroutine(AddForceWithRotation());
        }
    }
    //----------------Check System----------------------//
    //메뉴의 재료 갯수로 체크, 정밀 체크 필요할 시 수정 필요
    bool CheckRequireIngredient()
    {
        if (currentMenu == null || currentMenu.ingredients.Count != checkIngredients.Count)
        {
            return false;
        }
        else return true;
    }

    //Pot Scene에 접근시 사용
    public bool CheckCookCompleted()
    {
        if (completeTime >= currentTime)
        {
            potViewCamera.SetActive(true);
            return false;
        }
        else
        {
            return true;
            //CookSceneManager.instance.UnloadScene("PotMergeTest", CookManager.instance.currentMenu);
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


    IEnumerator AddForceWithRotation()
    {
        WaitForSeconds addForceTime = new WaitForSeconds(0.1f);
        while (true)
        {
            if (currentTime >= completeTime)
            {
                break;
            }
            foreach (GameObject obj in potIngredients)
            {
                if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    Vector3 center = centerPos.position;
                    Vector3 position = obj.transform.position;
                    Vector3 direction = position - center;

                    float distance = direction.magnitude;
                    Vector3 normal = direction.normalized;

                    Vector3 forceDirection = applyRight
                        ? Vector3.Cross(normal, Vector3.up).normalized  // 시계방향
                        : -Vector3.Cross(normal, Vector3.up).normalized; // 반시계방향
                    Vector3 forceToCenterOrOutward = (distance > radius * 0.3f)
                        ? -direction.normalized  // 바깥 방향
                        : direction.normalized; // 중심 방향

                    Vector3 finalForce = (forceDirection) + (forceToCenterOrOutward * 3f);

                    rb.AddForce(finalForce * 5f * rotatePower, ForceMode.Acceleration);
                    //rb.AddForce(forceToCenterOrOutward, ForceMode.Impulse);

                }
            }
            currentTime += 0.1f;
            yield return addForceTime;
        }
    }

}
