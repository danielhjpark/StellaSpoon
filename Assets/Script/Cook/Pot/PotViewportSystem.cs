using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.UI;
using cakeslice;

public class PotViewportSystem : MonoBehaviour
{
    private PotAudioSystem potAudioSystem;
    [Header("TimeLine")]
    [SerializeField] PlayableDirector lidTimeline;
    
    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    private CinemachineTrackedDolly dolly;

    //------------Button In Game View Objects----------------//
    [Header("Button Objects")]
    [SerializeField] public GameObject frontButton;
    [SerializeField] public GameObject bottomButton;
    [SerializeField] public GameObject buttonUI;
    [SerializeField] public GameObject lidButton;
    [SerializeField] public GameObject buttonViewObject; //In Game View Object Not UI;

    [Header("Off UI Panel")]
    [SerializeField] CanvasGroup ingredientInventoryPanel;

    [Header("Lid Object")]
    [SerializeField] GameObject lidObject;

    private bool movingForward = true;
    private bool isBottomView = false;
    private float viewportSpeed = 2f; 

    void Awake()
    {
        dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        potAudioSystem = GetComponent<PotAudioSystem>();
        dolly.m_PathPosition = 1;

        ingredientInventoryPanel.interactable = false;   // 버튼 등 이벤트 차단
        ingredientInventoryPanel.blocksRaycasts = false;
        bottomButton.SetActive(false);
        frontButton.SetActive(false);
    }

    public void ButtonAudio()
    {
        potAudioSystem.StartAudioSource(PotAudioSystem.AudioType.ViewButton);
    }

    // Button Change TopView /// Upper Button Active
    public void PutIngredient()
    {
        StartCoroutine(TopView());
    }

    // Button Change ButtonView  /// down Button Active
    public void BoilingPot() {
        bottomButton.SetActive(false);
        isBottomView = true;
    }

    public void FrontButton() {
        frontButton.SetActive(false);
        buttonUI.SetActive(false);
        buttonViewObject.SetActive(true);
        lidObject.AddComponent<cakeslice.Outline>();
        lidButton.SetActive(true);
        StartCoroutine(FrontView());
    }

    public IEnumerator OpenLid()
    {
        lidTimeline.Play();
        lidTimeline.time = 0; // 타임라인 시작점
        lidTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1); // 정방향 재생
        while (true) {
            if(lidTimeline.time >=  lidTimeline.duration) {
                break;
            }
            yield return null;
        }
    }

    public IEnumerator CloseLid()
    {
        lidTimeline.Pause();
        Destroy(lidObject.GetComponent<cakeslice.Outline>());
        while (true) {
            lidTimeline.time -= Time.deltaTime * viewportSpeed; // time 값을 직접 감소
            if (lidTimeline.time <= 0)
            {
                lidTimeline.time = 0;
                break;
            }
            lidTimeline.Evaluate(); // 즉시 적용
            yield return null;
        }
    }


    public IEnumerator FrontView() {
        //PlayBackward();
        while(movingForward) {
            dolly.m_PathPosition += viewportSpeed * Time.deltaTime;
            if (dolly.m_PathPosition >= 1) // 최대 WayPoint 도달
                movingForward = false;
            yield return null;
        }
    }

    IEnumerator TopView() {
       // PlayForward();
        buttonViewObject.SetActive(true);
        while(movingForward) {
            dolly.m_PathPosition += viewportSpeed * Time.deltaTime;
            if (dolly.m_PathPosition >= 2) // 최대 WayPoint 도달
                movingForward = false;
            yield return null;
        }
        ingredientInventoryPanel.interactable = true; 
        ingredientInventoryPanel.blocksRaycasts = true;

    }

    public IEnumerator ButtonView() {
        yield return new WaitUntil(() => isBottomView);

        while(!movingForward) {
            dolly.m_PathPosition -= viewportSpeed * Time.deltaTime;
            if (dolly.m_PathPosition <= 0) // 최소 WayPoint 도달
                movingForward = true;
            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
        buttonUI.SetActive(true);
        buttonViewObject.SetActive(false);

    }

}
