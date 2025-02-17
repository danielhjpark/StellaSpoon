using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour
{
    public GameObject uiPanel; // UI �г�
    [SerializeField]
    private GameObject inventoryPanel; // �κ��丮 �г�
    [SerializeField]
    private GameObject equipmentPanel; // ��� �г�

    void Update()
    {
        // Tab Ű�� UI Ȱ��ȭ/��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            uiPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // ó�� ������ �� �κ��丮 Ȱ��ȭ
            inventoryPanel.SetActive(true);
            equipmentPanel.SetActive(true);
        }

        // Esc Ű�� UI ��Ȱ��ȭ
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
