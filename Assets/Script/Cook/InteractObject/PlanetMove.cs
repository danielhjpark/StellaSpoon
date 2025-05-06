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
        if (InteractUIManger.isPlayerNearby && Input.GetKeyDown(KeyCode.F)) 
        {
            OpenUI();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            CloseUI();
        }
    }
    private void OpenUI() 
    {
        isOpened = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InteractUIManger.isUseInteractObject = true;
        Inventory.inventoryActivated = true;
    }
    private void CloseUI() 
    {
        isOpened = false;
        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;
        InteractUIManger.isUseInteractObject = false;
        Inventory.inventoryActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지");
            isPlayerNearby = true;
            InteractUIManger.isPlayerNearby = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")&&!isPlayerNearby)
        {
            InteractUIManger.isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 나감");
            isPlayerNearby = false;
            InteractUIManger.isPlayerNearby = false;
        }
    }
}
