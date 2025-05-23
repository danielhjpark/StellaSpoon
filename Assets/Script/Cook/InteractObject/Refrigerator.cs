using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    [SerializeField] private GameObject refrigeratorUI; //�������� UI
    [SerializeField] private GameObject inventoryUI; //�κ��丮 UI
    [SerializeField] private GameObject inventoryBG;
    //[SerializeField] private GameObject CloseButton;
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
        inventoryBG.SetActive(true);

        InteractUIManger.isUseInteractObject = true;
    }
    
    private void CloseRefrigeratorUI() //�������� UI �ݱ�
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
            Debug.Log("�÷��̾� ����");
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
            Debug.Log("�÷��̾� ����");
            isPlayerNearby = false;
            InteractUIManger.isPlayerNearby = false;
        }
    }
}
