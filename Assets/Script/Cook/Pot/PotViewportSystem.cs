using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PotViewportSystem : MonoBehaviour
{
    //---------------lid Controll TimeLine-------------------------//
    [SerializeField] PlayableDirector lidTimeline;

    //------------- Viewport Change Cinemachine -------------------//
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CinemachineSmoothPath basicPath;
    [SerializeField] CinemachineSmoothPath buttonPath;
    [SerializeField] Transform basicViewTarget;
    [SerializeField] Transform buttonViewTarget;
    private CinemachineTrackedDolly dolly;

    //------------Button In Game View Objects----------------//
    [SerializeField] GameObject buttonViewObject;
    [SerializeField] GameObject buttonUIObject;

    //------------ Off UISystem for change viewport ----------//
    [SerializeField] CanvasGroup ingredientInventoryPanel;


    private bool movingForward = true;
    private bool isRewinding = false;
    private bool isForward = true;
    private float viewportSpeed = 2f; 

    void Start()
    {
        dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        dolly.m_PathPosition = 0;

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

    //Button Path
    public void SwitchTrack()
    {
        isForward = !isForward; // 진행 방향 변경
        float currentPos = dolly.m_PathPosition; // 현재 경로 위치 저장

        if (isForward)
        {
            dolly.m_Path = basicPath;
            dolly.m_PathPosition = basicPath.PathLength - currentPos; // 역방향 -> 정방향 전환 시 보정
        }
        else
        {
            virtualCamera.LookAt = buttonViewTarget;
            dolly.m_Path = buttonPath;
            //dolly.m_PathPosition = buttonPath.PathLength - currentPos; // 정방향 -> 역방향 전환 시 보정
        }
    }

    // Button Change TopView
    public void PutIngredient() {
        StartCoroutine(TopView());
    }


    public void BoilingPot() {
        //StartCoroutine(FrontView());
        StartCoroutine(ButtonView());
    }

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


    IEnumerator ButtonView() {
        PlayBackward();
        SwitchTrack();
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

    public IEnumerator TopView() {
        PlayForward();
        while(movingForward) {
            dolly.m_PathPosition += viewportSpeed * Time.deltaTime;
            if (dolly.m_PathPosition >= 2) // 최대 WayPoint 도달
                movingForward = false;
            yield return null;
        }
        ingredientInventoryPanel.interactable = true; 
        ingredientInventoryPanel.blocksRaycasts = true;
    }

    IEnumerator FrontView() {
        PlayBackward();
        while(!movingForward) {
            dolly.m_PathPosition -= viewportSpeed * Time.deltaTime;
            if (dolly.m_PathPosition <= 0) // 최소 WayPoint 도달
                movingForward = true;
            yield return null;
        }
    }

    public void SetDirection(bool forward)
    {
        movingForward = forward;
    }


}
