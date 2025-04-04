using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;

public class FryingSystem : MonoBehaviour
{
    [Header("TimeLine")]
    [SerializeField] PlayableDirector timeline;

    [Header("UI Object")]
    [SerializeField] FryingPanUI fryingPanUI;

    [Header("Set Objects")]
    [SerializeField] private GameObject tongsObject;
    [SerializeField] private Transform grabPos;
    [SerializeField] private Transform previousParent;
    [SerializeField] private GameObject fryingPanObject;
    [SerializeField] private Transform center; 

    private GameObject mainIngredient;
    List<GameObject> mainIngredientPart = new List<GameObject>();
    List<Vector3> mainIngredientPreviousPos = new List<Vector3>();

    private bool isHalf;
    public int successFryingCount;

    private int grabNum;
    Queue<int> grabQueue;

    public void Initialize(GameObject mainIngredient)
    {
        this.mainIngredient = mainIngredient;

        foreach(Transform t in mainIngredient.transform) {
            mainIngredientPart.Add(t.gameObject);
            mainIngredientPreviousPos.Add(t.transform.localPosition);
        }


    }

    public void InitializeInherentMotion() {
        isHalf = false;
        successFryingCount = 0;
        GrabOrder();
    }

    void GrabOrder() {
        System.Random random = new System.Random();
        grabQueue = new Queue<int>(Enumerable.Range(0, mainIngredientPart.Count).OrderBy(_ => random.Next()));
    }

    void TongsSetting(float ingredientSize)
    {
        SkinnedMeshRenderer tongsMeshRenderer = tongsObject.GetComponent<SkinnedMeshRenderer>();
        MeshRenderer meshRenderer = mainIngredientPart[0].GetComponent<MeshRenderer>();
        Vector3 size = meshRenderer.bounds.size;

    }

    void lilToonShaderSet() {
        Renderer targetRenderer = mainIngredientPart[0].GetComponent<Renderer>();
        Material mat = targetRenderer.material;
        if (mat.HasProperty("_Color2nd"))
        {
            Color currentColor = mat.GetColor("_Color2nd"); // 기존 색상 가져오기
            currentColor.a -= 0.1f; // 알파 값만 변경
            mat.SetColor("_Color2nd", currentColor); // 새로운 색상 적용
        }
    }

    IEnumerator RotatePan()
    {
        float radius = 1f;    // 회전 반경
        float speed = 180f;    // 회전 속도 (도/초)
        float angle = 0f;    // 현재 각도
        while (true)
        {
            angle += speed * Time.deltaTime; // 회전 속도 적용
            if (angle >= 360f) angle -= 360f;

            // 각도를 방향벡터로 변환
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

            // 중심점 + 방향벡터 * 반지름으로 위치 갱신
            fryingPanObject.transform.position = center.position + dir * 0.1f * radius;

            yield return null;
        }
    } 

    public IEnumerator InherentMotion(int fryingCount, System.Action<int> callback)
    {
        if (fryingCount <= 0)
        {
            GrabOrder();
            callback(successFryingCount);
            yield break;
        }

        Coroutine fryingPanUIMark = StartCoroutine(fryingPanUI.MoveMark());
        Coroutine rotateRoutine = StartCoroutine(RotatePan());

        while (true)
        {
            
            SetActiveTongs();
            if (isHalf && timeline.time >= timeline.duration)
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

            if (Input.GetKeyDown(KeyCode.V) && timeline.state != PlayState.Playing)
            {
                timeline.Play();
                GrabObject();
                StopCoroutine(fryingPanUIMark);
                StopCoroutine(rotateRoutine);
            }
            else if (fryingPanUI.IsCheckEnd())
            {
                StopCoroutine(fryingPanUIMark);
                StopCoroutine(rotateRoutine);
                break;
            }

            yield return null;
        }
        if(fryingPanUI.GetCurrentSection()) {successFryingCount++;}
        ReturnObject();
        StopCoroutine(rotateRoutine);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(InherentMotion(--fryingCount, callback));

    }

    private void GrabObject() {
        lilToonShaderSet();
        if(grabQueue.Count != 0) grabNum = grabQueue.Dequeue();
        mainIngredientPart[grabNum].transform.SetParent(grabPos);
        mainIngredientPart[grabNum].transform.localPosition = Vector3.zero;
    }

    private void ReturnObject() {
        mainIngredientPart[grabNum].transform.SetParent(previousParent);
        mainIngredientPart[grabNum].transform.localPosition = mainIngredientPreviousPos[grabNum];
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
