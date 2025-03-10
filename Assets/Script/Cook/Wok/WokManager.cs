using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class WokManager : CookManagerBase
{
    [Header("TimeLine")]
    [SerializeField] PlayableDirector wokTimeLine;

    [Header("UI Objects")]
    [SerializeField] WokUI wokUI;
    [SerializeField] CookUIManager cookUIManager;
    
    [Header("Transform Objects")]
    [SerializeField] Transform dropPos;
    [SerializeField] GameObject dropIngredient;

    //----------------------------------------------------------------------//
    private List<GameObject> wokIngredients = new List<GameObject>();
    private List<Ingredient> checkIngredients = new List<Ingredient>();
    
    //----------------------------------------------------------------------//
    private bool isTossing = false;
    private int tossingCount;
    private int successTossingCount;
    

    void Awake() {
        CookManager.instance.BindingManager(this);
    }

    void Start()
    {
        wokUI.OnWokSystem += CheckTossing;   
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            CookSceneManager.instance.UnloadScene();
        }
        if(CheckRequireIngredient()) {
            checkIngredients.Clear();
            wokUI.OnWokUI();
            StartTossing();
        }
    }
    //--------------------------Virtual Method -----------------------------//
    public override void CookCompleteCheck() {
        //success
        if(currentMenu.tossingSetting.tossingCount <= successTossingCount){
            CookSceneManager.instance.UnloadScene("WokMergeTest", currentMenu);
        } 
        //fail 조건
        else {
            //음쓰 소환
        } 
        CookSceneManager.instance.UnloadScene("WokMergeTest", currentMenu);
    }
    
    public override void AddIngredient(GameObject obj, Ingredient ingredient) {
        obj.transform.position = dropPos.position;
        checkIngredients.Add(ingredient);
        AddIngredientList(obj);
    }

    //----------------------------------------------------------------------//
    public void AddIngredientList(GameObject ingredients) {
        ingredients.transform.SetParent(dropIngredient.transform);
        foreach(GameObject ingredient in ingredients.transform) {
            wokIngredients.Add(ingredient.gameObject);
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }
        StartCoroutine(cookUIManager.VisiblePanel());
    }

    bool CheckRequireIngredient() {
        if(currentMenu == null || currentMenu.ingredients.Count != checkIngredients.Count) {
            return false;
        }
        else return true;
    }

    public void StartTossing() {
        tossingCount = 3;
        StartCoroutine(WokTossing());   
        StartCoroutine(cookUIManager.HidePanel());
    }

    IEnumerator WokTossing() {
        bool isStartTimeLine = false;
        bool isAddForce = false;
        Coroutine wokUIMark = StartCoroutine(wokUI.MoveMark());

        while(true) {
            if(Input.GetKeyDown(KeyCode.V)&& !isStartTimeLine) {
                if(isTossing) {
                    wokTimeLine.time = 0;
                    wokTimeLine.Play();
                    isAddForce = false;
                    isStartTimeLine = true;
                    AddForceForwardIngredient();
                    StopCoroutine(wokUIMark);
                }
                else {
                    StopCoroutine(wokUIMark);
                    break;
                }
            }
            if(!isAddForce && wokTimeLine.time >= wokTimeLine.duration * 0.45) {
                isAddForce = true;
                AddForceUpIngredient();
            }
            else if(wokTimeLine.time >= wokTimeLine.duration) {
                 break;
            }
            else if(wokUI.IsCheckEnd()) break;

            yield return null;
        }

        tossingCount--;
        yield return new WaitForSeconds(0.5f);
        if(tossingCount > 0) StartCoroutine(WokTossing());
        else {
            CookSceneManager.instance.UnloadScene("WokMergeTest", currentMenu);
        }
    }

    void CheckTossing(bool isTossing) {
        this.isTossing = isTossing;
    }

    public void AddForceForwardIngredient() {
        foreach(GameObject ingredient in wokIngredients) {
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.forward * 3, ForceMode.VelocityChange);
        }
    }

    public void AddForceUpIngredient() {
        foreach(GameObject ingredient in wokIngredients) {
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.VelocityChange);
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.back * 1, ForceMode.VelocityChange);
        }
    }


    
}
