using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PotSystem : MonoBehaviour
{
    [SerializeField] PlayableDirector lidTimeline;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CinemachineSmoothPath basicPath;
    [SerializeField] CinemachineSmoothPath buttonPath;
    [SerializeField] Transform basicViewTarget;
    [SerializeField] Transform buttonViewTarget;
    private CinemachineTrackedDolly dolly;
    private bool movingForward = true;
    public float speed = 2f;
    private bool isRewinding = false;
    private float rewindSpeed = 1f; 

   private bool isForward = true;

    // Start is called before the first frame update
    void Start()
    {
        dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        dolly.m_PathPosition = 0;
    }

    void Update()
    {
        if (isRewinding)
        {
            lidTimeline.time -= Time.deltaTime * rewindSpeed; // time 값을 직접 감소
            if (lidTimeline.time <= 0)
            {
                lidTimeline.time = 0;
                isRewinding = false;
            }
            lidTimeline.Evaluate(); // 즉시 적용
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            ViewControll();
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

    private void ViewControll() {
        if(movingForward) {
            StartCoroutine(TopView());
        }
        else {
            StartCoroutine(FrontView());
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
            dolly.m_PathPosition -= speed * Time.deltaTime;
            if (dolly.m_PathPosition <= 0) // 최소 WayPoint 도달
                movingForward = true;
            yield return null;
        }
    }

    public IEnumerator TopView() {
        PlayForward();
        while(movingForward) {
            dolly.m_PathPosition += speed * Time.deltaTime;
            if (dolly.m_PathPosition >= 2) // 최대 WayPoint 도달
                movingForward = false;
            yield return null;
        }
    }

    IEnumerator FrontView() {
        PlayBackward();
        while(!movingForward) {
            dolly.m_PathPosition -= speed * Time.deltaTime;
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
