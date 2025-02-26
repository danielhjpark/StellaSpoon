using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    [SerializeField] private GameObject refrigeratorUI; //보물상자 UI
    [SerializeField] private GameObject inventoryUI; //인벤토리 UI
    [SerializeField] private GameObject CloseButton;
    bool isPlayerNearby;
    // Start is called before the first frame update
    void Start()
    {
        isPlayerNearby = false;
    }

    private void Update()
    {
        if (!refrigeratorUI.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        refrigeratorUI.SetActive(true);
        inventoryUI.SetActive(true);
        CloseButton.SetActive(false);

        Inventory.inventoryActivated = true;
    }
    private void CloseRefrigeratorUI() //보물상자 UI 닫기
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
            Debug.Log("플레이어 감지");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 나감");
            isPlayerNearby = false;
        }
    }
}
