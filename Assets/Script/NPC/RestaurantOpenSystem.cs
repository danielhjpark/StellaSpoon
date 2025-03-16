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
    // private string earlyOpenText = "���� �Ĵ� ���� ���⿣ �̸���. ���� 6�ñ��� ��ٷ�����";
    // private string readyOpenText = "�Ĵ��� �� �� �ִ� �ð��̴�(��� ���� �Ĵ��� ����)";
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
         // �׻� �������� ���� �Ÿ� �ȿ� �ִ��� üũ
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
        signTimeline.time = 0; // Ÿ�Ӷ��� ������
        signTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1); // ������ ���
        
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
            signTimeline.time -= Time.deltaTime; // time ���� ���� ����
            if (signTimeline.time <= 0)
            {
                signTimeline.time = 0;
                isRewinding = false;
            }
            signTimeline.Evaluate(); // ��� ����
            yield return null;
        }
    }
    //------------------------------------------------------//
    private void CheckSign()
    {
        Vector3 rayOrigin = characterTransform.position + Vector3.up * 0.5f; // ĳ���� �߽ɿ��� �ణ ����
        Vector3 rayDirection = characterTransform.forward; // ĳ������ forward ����

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
