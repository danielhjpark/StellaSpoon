using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FryingSystem : MonoBehaviour
{
    [Header("TimeLine")]
    [SerializeField] PlayableDirector timeline;

    [Header("UI Object")]
    [SerializeField] FryingPanUI fryingPanUI;

    [Header("Set Objects")]
    [SerializeField] private GameObject tongsObject;

    private GameObject currentIngredient;
    private bool isHalf;
    public int successFryingCount;
    public void Initialize()
    {
        isHalf = false;
        successFryingCount = 0;
        TongsSetting(10);
    }

    void TongsSetting(float ingredientSize)
    {
        SkinnedMeshRenderer tongsMeshRenderer = tongsObject.GetComponent<SkinnedMeshRenderer>();
        tongsMeshRenderer.SetBlendShapeWeight(0, ingredientSize);
        //tongsObject.transform.localScale = grabPoint;
    }

    public IEnumerator InherentMotion(int fryingCount, System.Action<int> callback)
    {
        if (fryingCount <= 0)
        {
            callback(successFryingCount);
            yield break;
        }
        Coroutine fryingPanUIMark = StartCoroutine(fryingPanUI.MoveMark());
        while (true)
        {
            SetActiveTongs();
            if (timeline.time >= timeline.duration && isHalf)
            {
                isHalf = false;
                break;
            }
            else if (!isHalf && timeline.time >= timeline.duration / 2)
            {
                timeline.Pause();
                isHalf = true;
                break;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                timeline.Play();
                StopCoroutine(fryingPanUIMark);
            }
            else if (fryingPanUI.IsCheckEnd())
            {
                StopCoroutine(fryingPanUIMark);
                break;
            }

            yield return null;
        }
        fryingPanUI.GetCurrentSection();
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(InherentMotion(--fryingCount, callback));

        //CookCompleteCheck();
    }

    void SetActiveTongs()
    {
        float invisibleTime = 0.1f;

        if (!isHalf)
        {
            if (timeline.time >= timeline.duration / 2 - invisibleTime) tongsObject.SetActive(false);
            else if (timeline.time >= invisibleTime) tongsObject.SetActive(true);
        }
        else
        {
            if (timeline.time >= timeline.duration - invisibleTime) tongsObject.SetActive(false);
            else if (timeline.time >= timeline.duration / 2 + invisibleTime) tongsObject.SetActive(true);
        }
    }
}
