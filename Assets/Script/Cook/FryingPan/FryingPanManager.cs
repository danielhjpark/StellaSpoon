using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FryingPanManager : MonoBehaviour
{
    [SerializeField] FryingPanUI fryingPanUI;
    [SerializeField] PlayableDirector timeline;
    

    [Header("Set Objects")]
    public GameObject tongs;
    public Transform dropPos;
    public GameObject ingredientParent;

    private GameObject currentIngredient;
    private bool isHalf;
    public int fryingCount = 3;

    void Start() {
        tongs.SetActive(false);
    }


    public void LocateIngredient(GameObject obj) {
        currentIngredient = obj;
        obj.transform.position = dropPos.position;
        obj.transform.SetParent(ingredientParent.transform);
        StartCoroutine(DropIngredient());      
    }

    IEnumerator DropIngredient() {
        float time = 0;
        while(true) {
            time += Time.deltaTime * 5;
            currentIngredient.transform.localPosition = Vector3.Lerp(dropPos.localPosition, Vector3.zero, time);
            if(currentIngredient.transform.localPosition.y <= 0) break;
            yield return null;
        }
        isHalf = false;
        StartCoroutine(StartFryingIngredient());
    }

    IEnumerator StartFryingIngredient() {
        Coroutine fryingPanUIMark = StartCoroutine(fryingPanUI.MoveMark());
        while(true) {
            SetActiveTongs();
            if(timeline.time >= timeline.duration && isHalf) {
                isHalf = false;
                break;
            }
            else if (!isHalf && timeline.time >= timeline.duration/2) {
                timeline.Pause();
                isHalf = true;
                break;
            }     

            if(Input.GetKeyDown(KeyCode.V)) {
                timeline.Play();  
                StopCoroutine(fryingPanUIMark);
            }   
            else if(fryingPanUI.IsCheckEnd()) {
                StopCoroutine(fryingPanUIMark);
                break;
            }

            yield return null;
        }
        fryingPanUI.GetCurrentSection();
        yield return new WaitForSeconds(0.5f);
        fryingCount--;
        if(fryingCount > 0) StartCoroutine(StartFryingIngredient());
    }

    void SetActiveTongs() {
        float invisibleTime = 0.1f;

        if(!isHalf) {
            if(timeline.time >= timeline.duration/2 - invisibleTime) tongs.SetActive(false);
            else if(timeline.time >= invisibleTime) tongs.SetActive(true);
        }
        else {
            if(timeline.time >= timeline.duration - invisibleTime) tongs.SetActive(false);
            else if(timeline.time >= timeline.duration/2 + invisibleTime) tongs.SetActive(true);
        }
    }
}
