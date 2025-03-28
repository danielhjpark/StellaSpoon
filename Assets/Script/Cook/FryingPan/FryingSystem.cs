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
    [SerializeField] private Transform grabPos;

    private Vector3 previousPos;

    private GameObject mainIngredient;

    private bool isHalf;
    public int successFryingCount;

    public void Initialize(GameObject mainIngredient)
    {
        this.mainIngredient = mainIngredient;
        previousPos = mainIngredient.transform.position;
        isHalf = false;
        successFryingCount = 0;
        TongsSetting(10);
    }

    void TongsSetting(float ingredientSize)
    {
        SkinnedMeshRenderer tongsMeshRenderer = tongsObject.GetComponent<SkinnedMeshRenderer>();
       // tongsMeshRenderer.SetBlendShapeWeight(0, ingredientSize);
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
                mainIngredient.transform.position = previousPos;
                timeline.Pause();
                isHalf = true;
                break;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                timeline.Play();
                mainIngredient.transform.position = grabPos.position;
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
        mainIngredient.transform.position = previousPos;
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

    public Transform center;     // 불의 Transform (중심)
    public GameObject fryingPanObject;
    public float radius = 1f;    // 회전 반경
    public float speed = 180f;    // 회전 속도 (도/초)

    private float angle = 0f;    // 현재 각도

    private Coroutine rotateRoutine;

    private void Start()
    {
        rotateRoutine = StartCoroutine(RotatePan());
    }

    IEnumerator RotatePan()
    {
        while (true)
        {
            angle += speed * Time.deltaTime * 10f; // 회전 속도 적용
            if (angle >= 360f) angle -= 360f;

            // 각도를 방향벡터로 변환
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

            // 중심점 + 방향벡터 * 반지름으로 위치 갱신
            fryingPanObject.transform.position = center.position + dir * 0.1f * radius;

            yield return null;
        }
    }
}
