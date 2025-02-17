using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceManager : MonoBehaviour
{
    public static bool isDeactived = true; // �ʱ� ���´� ��Ȱ��ȭ

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI();
        }

        // Esc Ű�� UI ��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Escape) && !isDeactived)
        {
            CloseUI();
        }
    }

    private void ToggleUI()
    {
        if (isDeactived)
        {
            OpenUI();
        }
        else
        {
            CloseUI();
        }
    }

    private void OpenUI()
    {
        uiPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inventoryPanel.SetActive(true);
        equipmentPanel.SetActive(true);

        isDeactived = false;

        inventoryButtonImage.sprite = selectedSprite; // ��ư �̹��� ����
    }

    private void CloseUI()
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
