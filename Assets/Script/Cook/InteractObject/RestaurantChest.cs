using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantChest : MonoBehaviour
{
    [SerializeField] GameObject chestInventory;

    bool isPlayerNearby;
    bool isOpenedChest;

    void Start()
    {
        isPlayerNearby = false;
        isOpenedChest = false;
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated)
        {
            OpenChestUI();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseChestUI();
        }
    }

    private void OpenChestUI()
    {
        isOpenedChest = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        chestInventory.SetActive(true);

        Inventory.inventoryActivated = true;

    }

    private void CloseChestUI()
    {
        isOpenedChest = false;
        Debug.Log("보물상자 닫기");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        chestInventory.SetActive(false);
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
        if (other.CompareTag("Player") && !isOpenedChest)
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
