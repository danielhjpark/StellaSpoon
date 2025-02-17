using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceManager : MonoBehaviour
{
    public static bool isDeactived = false;

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
        // Tab 키로 UI 활성화/비활성화
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            uiPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 처음 열었을 때 인벤토리 활성화
            inventoryPanel.SetActive(true);
            equipmentPanel.SetActive(true);

            isDeactived = false;

            inventoryButtonImage.sprite = selectedSprite; // 버튼 이미지 변경
        }

        // Esc 키로 UI 비활성화
        if (Input.GetKeyDown(KeyCode.Escape))
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
}
