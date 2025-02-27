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
    [SerializeField]
    private GameObject optionPanel; //옵션

    [Header("초기의 인벤토리 버튼")]
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Image inventoryButtonImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectedSprite;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //todo 기계장치 오픈시 카메라, 플레이어 움직임 제한 필요
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

        inventoryButtonImage.sprite = selectedSprite; // 버튼 이미지 변경

        isDeactived = false;
    }

    private void CloseUI()
    {
        uiPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //모든 창 false 필요
        inventoryPanel.SetActive(false); //인벤토리
        equipmentPanel.SetActive(false); //장비
        optionPanel.SetActive(false); //옵션

        inventoryButtonImage.sprite = defaultSprite; // 버튼 이미지 복귀

        isDeactived = true;
    }
}
