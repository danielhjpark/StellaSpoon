using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceManager : MonoBehaviour
{
    public static bool isDeactived = false;

    public GameObject uiPanel; // UI �г�
    [SerializeField]
    private GameObject inventoryPanel; // �κ��丮 �г�
    [SerializeField]
    private GameObject equipmentPanel; // ��� �г�


    // ó�� ������ �� �κ��丮 ��ư Sprite Ȱ��ȭ
    [Header("�κ��丮 ��ư ����")]
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Image inventoryButtonImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectedSprite;

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

            isDeactived = false;

            inventoryButtonImage.sprite = selectedSprite; // ��ư �̹��� ����
        }

        // Esc Ű�� UI ��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            inventoryPanel.SetActive(false);
            equipmentPanel.SetActive(false);

            isDeactived = true;

            inventoryButtonImage.sprite = defaultSprite; // ��ư �̹��� ����
        }
    }
}
