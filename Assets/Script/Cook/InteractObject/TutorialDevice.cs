using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDevice : MonoBehaviour
{
    //InteractUI
    [SerializeField] private GameObject tutorialUI; //�κ��丮 UI
    TutorialManger tutorialManger;
    bool isPlayerNearby;
    bool isOpenedDevice;

    void Start()
    {
        isPlayerNearby = false;
        isOpenedDevice = false;
        tutorialManger = tutorialUI.GetComponent<TutorialManger>();
    }

    private void Update()
    {
        if (!tutorialUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F)
        && DeviceManager.isDeactived) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            OpenTutorialUI();
        }
        else if (tutorialUI.activeSelf && Input.GetKeyDown(KeyCode.Escape) && tutorialManger.currentTutorial == null) //UI�� �����ְ� esc ������ ��
        {
            CloseTutorialUI();
        }
    }

    private void OpenTutorialUI() //UI���
    {
        isOpenedDevice = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        tutorialUI.SetActive(true);
        InteractUIManger.isUseInteractObject = true;
    }

    public void CloseTutorialUI() //UI �ݱ�
    {
        isOpenedDevice = false;
        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;

        tutorialUI.SetActive(false);
        InteractUIManger.isUseInteractObject = false;
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
