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
    List<GameObject> wokIngredients = new List<GameObject>();
    public GameObject dropIngredient;
    
    bool isTossing = false;
    int tossingCount;

    void Start()
    {
        wokUI.OnWokSystem += CheckTossing;   
    }

    public void StartTossing() {
        tossingCount = 3;
        StartCoroutine(WokTossing());   
        StartCoroutine(cookUIManager.HidePanel());
    }

    public void AddIngredient(GameObject ingredients) {
        foreach(Transform ingredient in ingredients.transform) {
            wokIngredients.Add(ingredient.gameObject);
        }

        foreach(GameObject wokIngredient in wokIngredients) {
            wokIngredient.transform.SetParent(dropIngredient.transform);
            wokIngredient.GetComponent<Rigidbody>().useGravity = true;
            wokIngredient.GetComponent<Collider>().enabled = true;;
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
    }

    void CheckTossing(bool isTossing) {
        this.isTossing = isTossing;
    }

    public void LocateIngredient(GameObject obj) {
       // obj.GetComponent<Rigidbody>().useGravity = true;
        //obj.GetComponent<Collider>().enabled = true;
        obj.transform.position = dropPos.position;
        AddIngredient(obj);
    }
    
    public void AddForceForwardIngredient() {
        foreach(GameObject ingredient in wokIngredients) {
            //ingredient.GetComponent<Rigidbody>().AddForce(Vector3.forward * 3, ForceMode.VelocityChange);
        }
    }

    public void AddForceUpIngredient() {
        foreach(GameObject ingredient in wokIngredients) {
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.VelocityChange);
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.back * 1, ForceMode.VelocityChange);
        }
    }
    
}
