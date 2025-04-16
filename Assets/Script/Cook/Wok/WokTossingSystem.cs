using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
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
    [SerializeField] WokIngredientSystem wokIngredientSystem;
    private WokAudioSystem wokAudioSystem;

    private List<GameObject> wokIngredients = new List<GameObject>();
    private int successTossingCount;
    private bool isTossing = false;

    void Start() {
        wokIngredientSystem = GetComponent<WokIngredientSystem>();
        wokAudioSystem = GetComponent<WokAudioSystem>();
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
        wokTimeLine.time = 0;
        bool isStartTimeLine = false;
        bool isAddForce = false;
        bool isUseSauce = false;

        Coroutine wokUIMark = StartCoroutine(wokUI.MoveMark());
        wokAudioSystem.StartAudioSource(WokAudioSystem.AudioType.WokDefault);
        while(true) {
            if(wokTimeLine.time >= wokTimeLine.duration) break;
            else if(wokUI.IsCheckEnd()) break;

            if(Input.GetKeyDown(KeyCode.V)) {
                if(isTossing && !isStartTimeLine) {
                    isStartTimeLine = true;
                    successTossingCount++;
                    wokTimeLine.Play();
                    wokAudioSystem.StartAudioSource(WokAudioSystem.AudioType.WokTossing);
                    AddForceForwardIngredient();
                    StopCoroutine(wokUIMark);
                }
                else {
                    StopCoroutine(wokUIMark);
                    break;
                }
            }

            //Add Motion
            if(!isAddForce && wokTimeLine.time >= wokTimeLine.duration * 0.45) {
                isAddForce = true;
                isUseSauce = true;
                AddForceUpIngredient();
                StartCoroutine(wokSauceSystem.UseSauce());
                wokIngredientSystem.ApplyIngredientShader();
            }
            yield return null;
        }
        if(!isUseSauce) {
            wokIngredientSystem.ApplyIngredientShader();
            yield return StartCoroutine(wokSauceSystem.UseSauce());
        }
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(WokTossing(--tossingCount, callback));
    }


    private void AddForceForwardIngredient() {
        foreach(GameObject ingredient in wokIngredients) {
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.forward * 1, ForceMode.VelocityChange);
        }
    }

    private void AddForceUpIngredient() {
        foreach(GameObject ingredient in wokIngredients) {
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.VelocityChange);
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.back * 1, ForceMode.VelocityChange);
        }
    }

    void CheckTossing(bool isTossing) {
        this.isTossing = isTossing;
    }


}
