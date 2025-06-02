using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyMenuDevice : InteractObject
{
    //InteractUI
    [SerializeField] private AudioClip closeAudio;
    [SerializeField] private GameObject DailyMenuUI; //�κ��丮 UI

    bool isPlayerNearby;
    bool isOpenedDevice;

    void Start()
    {
        isPlayerNearby = false;
        isOpenedDevice = false;
    }

    private void Update()
    {
        if (!DailyMenuUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F)
        && !RestaurantOpenSystem.isRestaurantOpened && DeviceManager.isDeactived) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            OpenDailyMenuUI();
        }
    }
    private void OpenDailyMenuUI() //UI���
    {
        isOpenedDevice = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DailyMenuUI.SetActive(true);
        InteractUIManger.isUseInteractObject = true;
    }

    public void CloseDailyMenuUI() //UI �ݱ�
    {
        isOpenedDevice = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SoundManager.instance.PlaySound(SoundManager.DailyMenu.Daily_Menu_Complete_Button);
        DailyMenuUI.SetActive(false);
        InteractUIManger.isUseInteractObject = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !RestaurantOpenSystem.isRestaurantOpened)
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
