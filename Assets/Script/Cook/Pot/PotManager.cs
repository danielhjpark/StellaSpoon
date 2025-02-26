using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PotManager : MonoBehaviour
{
    [SerializeField] Transform dropPos;
    [SerializeField] GameObject dropIngredient;
    [SerializeField] Transform centerPos;
    [SerializeField] GameObject potViewCamera;
    [SerializeField] GameObject uiObject;
    private PotSystem potSystem;
    private List<GameObject> potIngredients = new List<GameObject>();
    private List<Ingredient> checkIngredients = new List<Ingredient>();

    private Recipe currentMenu;
    
    private float completeTime;
    private float currentTime;
    public float power;
    public float radius;
    public bool applyRight = true;


    void Awake() {
        CookManager.instance.BindingManager(this);
        potSystem = GetComponent<PotSystem>();
    }

    public void SelectRecipe(Recipe menu) {
        currentMenu = menu;
        potSystem.PutIngredient();
    }


    void Update()
    {
        if(CheckRequireIngredient()) {
            checkIngredients.Clear();
            potSystem.BoilingPot();
            StartCoroutine(AddForceWithRotation());
        }
    }

    //메뉴의 재료 갯수로 체크, 정밀 체크 필요할 시 수정 필요
    bool CheckRequireIngredient() {
        if(currentMenu == null || currentMenu.ingredients.Count != checkIngredients.Count) {
            return false;
        }
        else return true;
    }


    public bool CheckCookCompleted() {
        if(completeTime >= currentTime) { //Pot Scene에 접근
            potViewCamera.SetActive(true);
            return false;
        }
        else {
            return true;
            //CookSceneManager.instance.UnloadScene("PotMergeTest", CookManager.instance.currentMenu);
        }
    }
    
    public void OpenSceneView() {
        potViewCamera.SetActive(true);
        uiObject.SetActive(true);
    }

    //Change to MainCamera
    public void CloseSceneView() {
        potViewCamera.SetActive(false);
        uiObject.SetActive(false);
    }

    public void LocateIngredient(GameObject obj, Ingredient ingredient) {
        obj.transform.position = dropPos.position;
        checkIngredients.Add(ingredient);
        AddIngredient(obj);
    }

    public void AddIngredient(GameObject ingredients) {
        ingredients.transform.SetParent(dropIngredient.transform);
        foreach(Transform ingredient in ingredients.transform) {
            potIngredients.Add(ingredient.gameObject);
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }
        //StartCoroutine(CookManager.instance.cookUIManager.VisiblePanel());
    }




    IEnumerator AddForceWithRotation() {
        WaitForSeconds addForceTime = new WaitForSeconds(0.1f);
        while (true) {
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
                    
                    rb.AddForce(finalForce * 5f * power, ForceMode.Acceleration);
                    //rb.AddForce(forceToCenterOrOutward, ForceMode.Impulse);

                }
            }
            yield return addForceTime;
        }
    }

}
