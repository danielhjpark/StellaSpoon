using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantdChest : InteractObject
{
    GameObject chestInventory;
    private GameObject inventoryUI; //인벤토리 UI
    private GameObject inventoryBG;
    bool isPlayerNearby;
    bool isOpenedChest;
    public int chestNum;

    void Awake()
    {
        isPlayerNearby = false;
        isOpenedChest = false;
        chestInventory = GameObject.Find("PARENT_RestaurantChestBase_" + chestNum).transform.GetChild(0).gameObject;
        inventoryBG = GameObject.Find("PARENT_InventoryBase(DeactivateThis)").transform.GetChild(0).gameObject;
        inventoryUI = GameObject.Find("PARENT_InventoryBase(DeactivateThis)").transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (!chestInventory.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && DeviceManager.isDeactived)
        {
            OpenChestUI();
        }
        else if (chestInventory.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseChestUI();
        }
    }

    private void OpenChestUI()
    {
        isOpenedChest = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayAudio();

        chestInventory.SetActive(true);
        inventoryUI.SetActive(true);
        inventoryBG.SetActive(true);

        InteractUIManger.isUseInteractObject = true;
        InteractUIManger.currentInteractObject = this.gameObject;
    }

    private void CloseChestUI()
    {
        isOpenedChest = false;
        Debug.Log("보물상자 닫기");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        chestInventory.SetActive(false);
        inventoryUI.SetActive(false);
        inventoryBG.SetActive(false);

        InteractUIManger.isUseInteractObject = false;
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
