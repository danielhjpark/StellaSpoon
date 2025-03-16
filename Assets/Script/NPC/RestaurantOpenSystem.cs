using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEditor.Rendering;

public class RestaurantOpenSystem : MonoBehaviour
{
    [SerializeField] private float range = 2.0f; 
    [SerializeField] private TextMeshProUGUI actionText; 
    [SerializeField] private Transform characterTransform; 
    [SerializeField] private LayerMask layerMask; 

    [SerializeField] PlayableDirector signTimeline;

    [SerializeField] Image pressGagueImage;

    private bool isOpened; 
    private bool isRewinding;
    private bool isCheckedSign;
    // private string earlyOpenText = "아직 식당 문을 열기엔 이르다. 오후 6시까지 기다려보자";
    // private string readyOpenText = "식당을 열 수 있는 시간이다(길게 눌러 식당을 열기)";
    private string earlyOpenText = "Ealry Open";
    private string readyOpenText = "Ready Open";

    void Start() {
        isOpened = false;
        isRewinding = false;
        isCheckedSign = false;
    }

    void Update()
    {
        CheckSign();
        // if (Input.GetKeyDown(KeyCode.R)&& isCheckedSign)
        // {
        //     if(!isRewinding) StartCoroutine(OpenSign());
        //     else StartCoroutine(CloseSign());
        // }
         // 항상 아이템이 사정 거리 안에 있는지 체크
    }

    public void FillGague() {
        pressGagueImage.fillAmount += Time.deltaTime * 0.5f;
    }

    public void ResetGague() {
        pressGagueImage.fillAmount = 0f;
    }


    public void PlayForward()
    {
        signTimeline.Play();
        signTimeline.time = 0; // 타임라인 시작점
        signTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1); // 정방향 재생
        
    }

    public void PlayBackward()
    {
        isRewinding = true;
        signTimeline.Pause();
    }

    //-------------- Hanging Sign Controll------------------//
    IEnumerator OpenSign() {
        PlayForward();
        while (true) {
            if(signTimeline.time >= signTimeline.duration) {
                isRewinding = true;
                break;
            }
            yield return null;
        }
    }

    IEnumerator CloseSign() {
        PlayBackward();
        while (isRewinding) {
            signTimeline.time -= Time.deltaTime; // time 값을 직접 감소
            if (signTimeline.time <= 0)
            {
                signTimeline.time = 0;
                isRewinding = false;
            }
            signTimeline.Evaluate(); // 즉시 적용
            yield return null;
        }
    }
    //------------------------------------------------------//
    private void CheckSign()
    {
        Vector3 rayOrigin = characterTransform.position + Vector3.up * 0.5f; // 캐릭터 중심에서 약간 위로
        Vector3 rayDirection = characterTransform.forward; // 캐릭터의 forward 방향

        if (Physics.Raycast(rayOrigin, rayDirection, range, layerMask))
        {
            isCheckedSign = true;
            if(isOpened) {
                ReadyOpenSign();
            }
            else {
                EarlyOpenSign();
            }

        }
        else {
            isCheckedSign = false;
            actionText.gameObject.SetActive(false);
        }
    }

    private void ReadyOpenSign()
    {
        //actionText.gameObject.SetActive(true);
        actionText.text = readyOpenText;
    }

    private void EarlyOpenSign()
    {
        //actionText.gameObject.SetActive(true);
        actionText.text = earlyOpenText;
    }
}
