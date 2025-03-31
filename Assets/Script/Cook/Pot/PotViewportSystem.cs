using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PotViewportSystem : MonoBehaviour
{
    [Header("TimeLine")]
    [SerializeField] PlayableDirector lidTimeline;

    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    private CinemachineTrackedDolly dolly;

    //------------Button In Game View Objects----------------//
    [Header("Button Objects")]
    [SerializeField] GameObject buttonViewObject;
    [SerializeField] GameObject buttonUIObject;

    [Header("Off UI Panel")]
    [SerializeField] CanvasGroup ingredientInventoryPanel;


    private bool movingForward = true;
    private bool isRewinding = false;
    private float viewportSpeed = 2f; 

    void Start()
    {
        dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        dolly.m_PathPosition = 1;

        ingredientInventoryPanel.interactable = false;   // 버튼 등 이벤트 차단
        ingredientInventoryPanel.blocksRaycasts = false;
    }

    void Update()
    {
        if (isRewinding)
        {
            lidTimeline.time -= Time.deltaTime * viewportSpeed; // time 값을 직접 감소
            if (lidTimeline.time <= 0)
            {
                lidTimeline.time = 0;
                isRewinding = false;
            }
            lidTimeline.Evaluate(); // 즉시 적용
        }
    }

    // Button Change TopView /// Upper Button Active
    public void PutIngredient() {
        StartCoroutine(TopView());
    }

    // Button Change TopView  /// down Button Active
    public void BoilingPot() {
        StartCoroutine(ButtonView());
    }


    //////////////////////////////////////////////


    public void PlayForward()
    {
        lidTimeline.Play();
        lidTimeline.time = 0; // 타임라인 시작점
        lidTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1); // 정방향 재생
    }

    public void PlayBackward()
    {
        isRewinding = true;
        lidTimeline.Pause();
    }

    IEnumerator TopView() {
        PlayForward();
        buttonViewObject.SetActive(true);
        buttonUIObject.SetActive(false);
        while(movingForward) {
            dolly.m_PathPosition += viewportSpeed * Time.deltaTime;
            if (dolly.m_PathPosition >= 2) // 최대 WayPoint 도달
                movingForward = false;
            yield return null;
        }
        ingredientInventoryPanel.interactable = true; 
        ingredientInventoryPanel.blocksRaycasts = true;

    }

    IEnumerator ButtonView() {
        PlayBackward();
        while(!movingForward) {
            dolly.m_PathPosition -= viewportSpeed * Time.deltaTime;
            if (dolly.m_PathPosition <= 0) // 최소 WayPoint 도달
                movingForward = true;
            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
        buttonViewObject.SetActive(false);
        buttonUIObject.SetActive(true);

    }



}
