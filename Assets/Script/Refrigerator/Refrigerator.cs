using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    [SerializeField] private GameObject refrigeratorUI; //�������� UI
    [SerializeField] private GameObject inventoryUI; //�κ��丮 UI
    [SerializeField] private GameObject CloseButton;
    bool isPlayerNearby;
    // Start is called before the first frame update
    void Start()
    {
        isPlayerNearby = false;
    }

    private void Update()
    {
        if (!refrigeratorUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        refrigeratorUI.SetActive(true);
        inventoryUI.SetActive(true);
        CloseButton.SetActive(false);

        Inventory.inventoryActivated = true;
    }
    private void CloseRefrigeratorUI() //�������� UI �ݱ�
    {
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����");
            isPlayerNearby = false;
        }
    }
}
