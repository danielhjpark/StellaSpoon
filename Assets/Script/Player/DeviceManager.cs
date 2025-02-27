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
    [SerializeField]
    private GameObject optionPanel; //�ɼ�

    [Header("�ʱ��� �κ��丮 ��ư")]
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Image inventoryButtonImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectedSprite;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //todo �����ġ ���½� ī�޶�, �÷��̾� ������ ���� �ʿ�
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

        inventoryButtonImage.sprite = selectedSprite; // ��ư �̹��� ����

        isDeactived = false;
    }

    private void CloseUI()
    {
        uiPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //��� â false �ʿ�
        inventoryPanel.SetActive(false); //�κ��丮
        equipmentPanel.SetActive(false); //���
        optionPanel.SetActive(false); //�ɼ�

        inventoryButtonImage.sprite = defaultSprite; // ��ư �̹��� ����

        isDeactived = true;
    }
}
