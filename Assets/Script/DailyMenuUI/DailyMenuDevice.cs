using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyMenuDevice : MonoBehaviour
{
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
        if (!DailyMenuUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            OpenDailyMenuUI();
        }
        // if (DailyMenuUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) //UI�� �����ְ� esc ������ ��
        // {
        //     CloseDailyMenuUI();
        // }
    }
    private void OpenDailyMenuUI() //UI���
    {
        isOpenedDevice = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DailyMenuUI.SetActive(true);
        UIManager.instance.HideInteractUI();
    }
    public void CloseDailyMenuUI() //UI �ݱ�
    {
        isOpenedDevice = false;
        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;

        DailyMenuUI.SetActive(false);
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            UIManager.instance.VisibleInteractUI();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")&&!isOpenedDevice)
        {
            UIManager.instance.VisibleInteractUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            UIManager.instance.HideInteractUI();
        }
    }
}
