using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour
{
    public GameObject uiPanel; // UI 패널
    [SerializeField]
    private GameObject inventoryPanel; // 인벤토리 패널
    [SerializeField]
    private GameObject equipmentPanel; // 장비 패널

    void Update()
    {
        // Tab 키로 UI 활성화/비활성화
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            uiPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // 처음 열었을 때 인벤토리 활성화
            inventoryPanel.SetActive(true);
            equipmentPanel.SetActive(true);
        }

        // Esc 키로 UI 비활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryPanel.SetActive(false);
            equipmentPanel.SetActive(false);
        }
    }
}
