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
    [SerializeField] private GameObject tongs;

    private GameObject currentIngredient;
    private bool isHalf;
    public int successFryingCount;
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        isHalf = false;
        successFryingCount = 0;

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
            if (timeline.time >= timeline.duration / 2 - invisibleTime) tongs.SetActive(false);
            else if (timeline.time >= invisibleTime) tongs.SetActive(true);
        }
        else
        {
            if (timeline.time >= timeline.duration - invisibleTime) tongs.SetActive(false);
            else if (timeline.time >= timeline.duration / 2 + invisibleTime) tongs.SetActive(true);
        }
    }
}
