using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class WokManager : MonoBehaviour
{
    [SerializeField] WokUI wokUI;
    [SerializeField] CookUIManager cookUIManager;
    [SerializeField] Transform dropPos;
    [SerializeField] PlayableDirector wokTimeLine;
    [SerializeField] GameObject dropIngredient;
    private List<GameObject> wokIngredients = new List<GameObject>();
    private List<Ingredient> checkIngredients = new List<Ingredient>();
    
    private bool isTossing = false;
    public int tossingCount;
    

    void Awake() {
        CookManager.instance.BindingManager(this);
    }

    void Start()
    {
        wokUI.OnWokSystem += CheckTossing;   
    }

    void Update() {
        if(CheckRequireIngredient()) {
            checkIngredients.Clear();
            wokUI.OnWokUI();
            StartTossing();
        }
    }
    
    private Recipe currentMenu;
    public void SelectRecipe(Recipe menu) {
        currentMenu = menu;
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

    public void LocateIngredient(GameObject obj, Ingredient ingredient) {
       // obj.GetComponent<Rigidbody>().useGravity = true;
        //obj.GetComponent<Collider>().enabled = true;
        obj.transform.position = dropPos.position;
        checkIngredients.Add(ingredient);
        AddIngredient(obj);
    }

    public void AddIngredient(GameObject ingredients) {
        foreach(Transform ingredient in ingredients.transform) {
            
        }
        ingredients.transform.SetParent(dropIngredient.transform);
        foreach(GameObject ingredient in ingredients.transform) {
            wokIngredients.Add(ingredient.gameObject);
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }
        StartCoroutine(cookUIManager.VisiblePanel());
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
