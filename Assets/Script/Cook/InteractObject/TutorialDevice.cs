using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDevice : MonoBehaviour
{
    //InteractUI
    bool isPlayerNearby;
    bool isOpenedDevice;

    void Start()
    {
        isPlayerNearby = false;
        isOpenedDevice = false;
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F)
        && DeviceManager.isDeactived) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        {
            OpenTutorialUI();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) //UI가 열려있고 esc 눌렀을 때
        {
            CloseTutorialUI();
        }
    }

    private void OpenTutorialUI() //UI출력
    {
        isOpenedDevice = true;
        TutorialManger.instance.OpenTutorialUI();
        TutorialManger.instance.OpenSelectTutorial();
    }

    public void CloseTutorialUI() //UI 닫기
    {
        isOpenedDevice = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            InteractUIManger.isPlayerNearby = true;
            InteractUIManger.currentInteractObject = this.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isOpenedDevice)
        {
            InteractUIManger.isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            InteractUIManger.isPlayerNearby = false;
        }
    }
}
