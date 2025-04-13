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
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            OpenUI();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) //UI�� �����ְ� esc ������ ��
        {
            CloseUI();
        }
    }
    private void OpenUI() //�������� UI���
    {
        isOpened = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UIManager.instance.HideInteractUI();
        
        Inventory.inventoryActivated = true;
    }
    private void CloseUI() //�������� UI �ݱ�
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
            Debug.Log("�÷��̾� ����");
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
            Debug.Log("�÷��̾� ����");
            isPlayerNearby = false;
            UIManager.instance.HideInteractUI();
        }
    }
}
