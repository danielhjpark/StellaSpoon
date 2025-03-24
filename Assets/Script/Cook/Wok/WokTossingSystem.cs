using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Playables;

public class WokTossingSystem : MonoBehaviour
{
    [Header("TimeLine")]
    [SerializeField] PlayableDirector wokTimeLine;

    [Header("UI Objects")]
    [SerializeField] WokUI wokUI;

    [Header("Wok System")]
    [SerializeField] WokSauceSystem wokSauceSystem;
    //----------------------------------------------------------------------//
    private List<GameObject> wokIngredients = new List<GameObject>();
    //----------------------------------------------------------------------//
    private int successTossingCount;
    private bool isTossing = false;
    //---------------------------------------------------------------------//

    void Start() {
        wokUI.OnWokSystem += CheckTossing;   
    }

    public void BindTossingObject(List<GameObject> wokIngredients) {
        this.wokIngredients = wokIngredients;
        successTossingCount = 0;
    }
    public IEnumerator WokTossing(int tossingCount, System.Action<int> callback) {
        
        if(tossingCount <= 0) {
            callback(successTossingCount);
            yield break;
        }

        wokUI.OnWokUI();
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
                    successTossingCount++;
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

        wokSauceSystem.IncreaseLiquidLevel();
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(WokTossing(--tossingCount, callback));
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
    void CheckTossing(bool isTossing) {
        this.isTossing = isTossing;
    }


}
