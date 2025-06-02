using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceManager : MonoBehaviour
{
    public static bool isDeactived = true;

    public GameObject uiPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject equipmentPanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject planetPanel;
    [SerializeField] private GameObject recipePanel;
    [SerializeField] private GameObject recipeInfoPanel;
    [SerializeField] private GameObject showText;
    private bool wasShowTextActive;

    [Header("초기의 인벤토리 버튼")]
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Image inventoryButtonImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectedSprite;

    private ThirdPersonController playerController;
    private StarterAssetsInputs _input;

    private ItemNameData itemNameData;

    private void Awake()
    {
        playerController = FindObjectOfType<ThirdPersonController>();
        _input = FindObjectOfType<StarterAssetsInputs>();
    }

    private void Start()
    {
        itemNameData = FindObjectOfType<ItemNameData>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!TreasureChest.openingChest && playerController != null && playerController.Grounded && playerController.isDodge == false && _input.aiming == false && WeaponChanger.isDeactived
                && !StoreNPCManager.openingStoreUI && !InteractUIManger.isUseInteractObject)
            {
                ToggleUI();
            }
        }

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
        wasShowTextActive = showText.activeSelf;
        if (wasShowTextActive)
        {
            showText.SetActive(false);
        }

        uiPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inventoryPanel.SetActive(true);
        Inventory.inventoryActivated = true;
        equipmentPanel.SetActive(true);

        inventoryButtonImage.sprite = selectedSprite;

        isDeactived = false;
    }

    public void CloseUI()
    {
        uiPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inventoryPanel.SetActive(false);
        Inventory.inventoryActivated = false;
        equipmentPanel.SetActive(false);
        optionPanel.SetActive(false);
        planetPanel.SetActive(false);
        recipePanel.SetActive(false);
        recipeInfoPanel.SetActive(false);

        inventoryButtonImage.sprite = defaultSprite;

        isDeactived = true;

        Option.OptionActivated = false;
        Planet.planetActivated = false;
        DeviceRecipeUI.recipeActivated = false;

        if (wasShowTextActive)
        {
            showText.SetActive(true);
        }
        itemNameData.HideToolTip();
    }
}
