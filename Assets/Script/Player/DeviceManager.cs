using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceManager : MonoBehaviour
{
    public static bool isDeactived = true; // 초기 상태는 비활성화

    public GameObject uiPanel; // UI 패널
    [SerializeField]
    private GameObject inventoryPanel; // 인벤토리 패널
    [SerializeField]
    private GameObject equipmentPanel; // 장비 패널

    // 처음 열었을 때 인벤토리 버튼 Sprite 활성화
    [Header("인벤토리 버튼 세팅")]
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

        // Esc 키로 UI 비활성화
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

        inventoryButtonImage.sprite = selectedSprite; // 버튼 이미지 변경
    }

    private void CloseUI()
    {
        uiPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inventoryPanel.SetActive(false);
        equipmentPanel.SetActive(false);

        isDeactived = true;

        inventoryButtonImage.sprite = defaultSprite; // 버튼 이미지 복귀
    }
}
