using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyMenuDevice : MonoBehaviour
{
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
        if (!DailyMenuUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
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
        UIManager.instance.HideInteractUI();
    }
    public void CloseDailyMenuUI() //UI 닫기
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
