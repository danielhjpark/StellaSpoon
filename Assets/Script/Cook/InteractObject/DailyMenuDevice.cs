using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyMenuDevice : MonoBehaviour
{
    //InteractUI
    [SerializeField] private GameObject DailyMenuUI; //인벤토리 UI
    bool isPlayerNearby;
    bool isOpenedDevice;

    void Start()
    {
        isPlayerNearby = false;
        isOpenedDevice = false;
    }

    private void Update()
    {
        //if (!DailyMenuUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated)
        if (!DailyMenuUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        {
            OpenDailyMenuUI();
        }
        // if (DailyMenuUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) //UI가 열려있고 esc 눌렀을 때
        // {
        //     CloseDailyMenuUI();
        // }
    }
    private void OpenDailyMenuUI() //UI출력
    {
        isOpenedDevice = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DailyMenuUI.SetActive(true);
        InteractUIManger.isUseInteractObject = true;
        Inventory.inventoryActivated = true;
    }

    public void CloseDailyMenuUI() //UI 닫기
    {
        isOpenedDevice = false;
        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;

        DailyMenuUI.SetActive(false);
        InteractUIManger.isUseInteractObject = false;
        Inventory.inventoryActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            InteractUIManger.isPlayerNearby = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")&&!isOpenedDevice)
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
