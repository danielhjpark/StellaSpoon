using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : InteractObject
{
    private GameObject refrigeratorUI; //보물상자 UI
    private GameObject inventoryUI; //인벤토리 UI
    private GameObject inventoryBG;
    bool isPlayerNearby;
    bool isOpenedRefrigerator;

    void Start() 
    {
        isPlayerNearby = false;
        isOpenedRefrigerator = false;
        refrigeratorUI = GameObject.Find("PARENT_RefrigeratorBase(DeactivateThis)").transform.GetChild(0).gameObject;
        inventoryUI = GameObject.Find("PARENT_InventoryBase(DeactivateThis)").transform.GetChild(1).gameObject;
        inventoryBG = GameObject.Find("PARENT_InventoryBase(DeactivateThis)").transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (!refrigeratorUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        {
            OpenRefrigeratorUI();
        }
        if (refrigeratorUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) //UI가 열려있고 esc 눌렀을 때
        {
            CloseRefrigeratorUI();
        }
    }
    private void OpenRefrigeratorUI() //보물상자 UI출력
    {
        isOpenedRefrigerator = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayAudio();
        
        refrigeratorUI.SetActive(true);
        inventoryUI.SetActive(true);
        inventoryBG.SetActive(true);

        InteractUIManger.isUseInteractObject = true;
    }
    
    private void CloseRefrigeratorUI() //보물상자 UI 닫기
    {
        isOpenedRefrigerator = false;
        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;

        refrigeratorUI.SetActive(false);
        inventoryUI.SetActive(false);
        inventoryBG.SetActive(false);

        InteractUIManger.isUseInteractObject = false;        
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
        if (other.CompareTag("Player")&&!isOpenedRefrigerator)
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
