using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour
{
    public GameObject uiPanel; // UI �г�

    void Update()
    {
        // Tab Ű�� UI Ȱ��ȭ/��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            uiPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Esc Ű�� UI ��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
