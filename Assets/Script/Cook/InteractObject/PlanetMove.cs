using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMove : MonoBehaviour
{
    bool isOpened;
    bool isPlayerNearby;
    void Start() 
    {
        isPlayerNearby = false;
        isOpened = false;
        
    }

    private void Update()
    {
        //if (!refrigeratorUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated)
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        {
            OpenUI();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) //UI가 열려있고 esc 눌렀을 때
        {
            CloseUI();
        }
    }
    private void OpenUI() //보물상자 UI출력
    {
        isOpened = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UIManager.instance.HideInteractUI();
        
        Inventory.inventoryActivated = true;
    }
    private void CloseUI() //보물상자 UI 닫기
    {
        isOpened = false;
        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;

        Inventory.inventoryActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지");
            isPlayerNearby = true;
            UIManager.instance.VisibleInteractUI();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")&&!isPlayerNearby)
        {
            UIManager.instance.VisibleInteractUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 나감");
            isPlayerNearby = false;
            UIManager.instance.HideInteractUI();
        }
    }
}
