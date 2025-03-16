using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PotManager : CookManagerBase
{
    private PotViewportSystem potViewportSystem;
    
    [Header("Transform Objects")]
    [SerializeField] Transform dropPos;
    [SerializeField] GameObject dropIngredient;
    [SerializeField] Transform centerPos;

    [Header("Disable Objects")]
    [SerializeField] GameObject potViewCamera;
    [SerializeField] GameObject uiObject;

    [Header("Button UI Text")]
    [SerializeField] TextMeshProUGUI powerText;

    //--------------- Save List ------------------------//
    private List<GameObject> potIngredients = new List<GameObject>();
    private List<Ingredient> checkIngredients = new List<Ingredient>();

    //-------------Pot Rotate Setting ----------------//
    private float rotatePower = 0;
    public float radius;
    public bool applyRight = true;
    private Coroutine ingredientRotateCoroutine;

    //------------------------------------------------//
    private float completeTime;
    private float currentTime;

    void Awake() {
        CookManager.instance.BindingManager(this);
        potViewportSystem = GetComponent<PotViewportSystem>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            CloseSceneView();
        }

        if(CheckRequireIngredient()) {
            checkIngredients.Clear();
            potViewportSystem.BoilingPot();
        }
    }

    //--------------- virtual Method ----------------------//
    public override void CookCompleteCheck() {
        if(currentTime >= completeTime) {
            CookSceneManager.instance.UnloadScene("PotMergeTest", currentMenu);
        }
            
    }

    public override void SelectRecipe(Recipe menu) {
        base.SelectRecipe(menu);
        potViewportSystem.PutIngredient();
        completeTime = 10f; //수정 필요
        currentTime = 0f;
    }

    public override void AddIngredient(GameObject obj, Ingredient ingredient) {
        obj.transform.position = dropPos.position;
        checkIngredients.Add(ingredient);
        AddIngredientList(obj);
    }

    //-------------------------------------------------------------//
    
    public void AddIngredientList(GameObject ingredients) {
        ingredients.transform.SetParent(dropIngredient.transform);
        foreach(Transform ingredient in ingredients.transform) {
            potIngredients.Add(ingredient.gameObject);
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }
    }

    //-------------------Button----------------------------------//
    
    public void OnIncreasePower() {
        if(rotatePower < 3) rotatePower ++;
        powerText.text = rotatePower.ToString();
        if (ingredientRotateCoroutine == null)
        {
            ingredientRotateCoroutine = StartCoroutine(AddForceWithRotation());
        }
        
    }

    public void OnDecreasePower() {
        if(rotatePower > 0) rotatePower --;
        powerText.text = rotatePower.ToString();
        if (ingredientRotateCoroutine == null)
        {
            ingredientRotateCoroutine = StartCoroutine(AddForceWithRotation());
        }
    }
    //----------------Check System----------------------//
    //메뉴의 재료 갯수로 체크, 정밀 체크 필요할 시 수정 필요
    bool CheckRequireIngredient() {
        if(currentMenu == null || currentMenu.ingredients.Count != checkIngredients.Count) {
            return false;
        }
        else return true;
    }

    //Pot Scene에 접근시 사용
    public bool CheckCookCompleted() {
        if(completeTime >= currentTime) { 
            potViewCamera.SetActive(true);
            return false;
        }
        else {
            return true;
            //CookSceneManager.instance.UnloadScene("PotMergeTest", CookManager.instance.currentMenu);
        }
    }
    
    //---------------SceneView Controll--------------//
    public void OpenSceneView() {
        potViewCamera.SetActive(true);
        uiObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Change to MainCamera
    public void CloseSceneView() {
        potViewCamera.SetActive(false);
        uiObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    IEnumerator AddForceWithRotation() {
        WaitForSeconds addForceTime = new WaitForSeconds(0.1f);
        while (true) {
            CookCompleteCheck();
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
