using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    [SerializeField] private GameObject refrigeratorUI; //�������� UI
    [SerializeField] private GameObject inventoryUI; //�κ��丮 UI
    [SerializeField] private GameObject CloseButton;
    bool isPlayerNearby;
    bool isOpenedRefrigerator;

    void Start() 
    {
        isPlayerNearby = false;
        isOpenedRefrigerator = false;
        refrigeratorUI = GameObject.Find("PARENT_RefrigeratorBase(DeactivateThis)").transform.GetChild(0).gameObject;
        inventoryUI = GameObject.Find("PARENT_InventoryBase(DeactivateThis)").transform.GetChild(1).gameObject;
        
    }

    private void Update()
    {
        //if (!refrigeratorUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated)
        if (!refrigeratorUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            OpenRefrigeratorUI();
        }
        if (refrigeratorUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) //UI�� �����ְ� esc ������ ��
        {
            CloseRefrigeratorUI();
        }
    }
    private void OpenRefrigeratorUI() //�������� UI���
    {
        isOpenedRefrigerator = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        refrigeratorUI.SetActive(true);
        inventoryUI.SetActive(true);
        CloseButton.SetActive(false);
        UIManager.instance.HideInteractUI();
        
        Inventory.inventoryActivated = true;
    }
    private void CloseRefrigeratorUI() //�������� UI �ݱ�
    {
        isOpenedRefrigerator = false;
        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;

        refrigeratorUI.SetActive(false);
        inventoryUI.SetActive(false);
        CloseButton.SetActive(true);
        
        Inventory.inventoryActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����");
            isPlayerNearby = true;
            UIManager.instance.VisibleInteractUI();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")&&!isOpenedRefrigerator)
        {
            UIManager.instance.VisibleInteractUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����");
            isPlayerNearby = false;
            UIManager.instance.HideInteractUI();
        }
    }
}
