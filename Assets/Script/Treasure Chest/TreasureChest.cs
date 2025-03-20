using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureChest : MonoBehaviour
{
    [Header("보물상자 Information")]
    [SerializeField]
    private GameObject treasureChest; // 보물상자 오브젝트
    private Animator animator;

    private bool isPlayerNearby = false; // 플레이어 감지

    private GameObject treasureChestPanel; // 보물상자 UI
    private GameObject inventoryPanel; // 인벤토리 UI
    private GameObject slotsSetting; // 슬롯 셋팅

    [Header("보물상자에 나오는 아이템")]
    [SerializeField]
    private List<Item> possibleItems; // 생성 가능한 아이템 리스트
    [Header("아이템 갯수")]
    [SerializeField]
    private int minItemCount = 1; // 최소 아이템 수
    [SerializeField]
    private int maxItemCount = 3; // 최대 아이템 수

    private Slot[] chestSlots; // 보물상자 슬롯 배열
    private bool isOpenChest = false; // 보물상자 오픈 여부
    public static bool openingChest = false; // 오픈중인지 여부

    private void Start()
    {
        if (treasureChest != null)
        {
            animator = treasureChest.GetComponent<Animator>();
            FindUIObjects(); // UI 오브젝트 찾기
        }
        else
        {
            Debug.LogWarning("TreasureChest 오브젝트가 할당되지 않음.");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIObjects(); // 씬이 변경될 때 UI 다시 찾기
        ResetChest(); // 보물상자 초기화
    }

    private void FindUIObjects()
    {
        // 보물상자 UI와 슬롯 재할당
        treasureChestPanel = GameObject.Find("Canvas/PARENT_TreasureChestBase(DeactivateThis)/TreasureChestBase");
        inventoryPanel = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)/InventoryBase");
        slotsSetting = GameObject.Find("Canvas/PARENT_TreasureChestBase(DeactivateThis)/TreasureChestBase/Slot Setting");

        if (slotsSetting != null)
        {
            chestSlots = slotsSetting.GetComponentsInChildren<Slot>();
        }
        else
        {
            Debug.LogWarning("slotsSetting을 찾을 수 없습니다. 슬롯을 초기화할 수 없음.");
        }
    }

    private void Update()
    {
        if (!treasureChestPanel.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated)
        {
            OpenChestUI();
        }
        if (treasureChestPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseChestUI();
        }
    }

    private void ResetChest()
    {
        isOpenChest = false; // 보물상자 상태 초기화
        CreateItem(); // 아이템 생성
    }

    private void OpenChestUI()
    {
        openingChest = true;
        if (!isOpenChest)
        {
            CreateItem();
            isOpenChest = true;
        }
        Debug.Log("보물상자 열기");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        treasureChestPanel.SetActive(true);
        inventoryPanel.SetActive(true);

        Inventory.inventoryActivated = true;

        animator.SetTrigger("Open");
    }

    private void CloseChestUI()
    {
        openingChest = false;
        Debug.Log("보물상자 닫기");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        treasureChestPanel.SetActive(false);
        inventoryPanel.SetActive(false);

        Inventory.inventoryActivated = false;
    }

    private void CreateItem()
    {
        if (chestSlots == null || chestSlots.Length == 0)
        {
            Debug.LogWarning("chestSlots가 설정되지 않았습니다. 아이템을 추가할 수 없습니다.");
            return;
        }

        int itemCount = Random.Range(minItemCount, maxItemCount + 1);

        for (int i = 0; i < itemCount; i++)
        {
            Item randomItem = possibleItems[Random.Range(0, possibleItems.Count)];
            if (i < chestSlots.Length)
            {
                chestSlots[i].AddItemWithoutWeight(randomItem, 1);
            }
            else
            {
                Debug.LogWarning("슬롯 인덱스 초과. 더 이상 아이템을 추가할 슬롯이 없습니다.");
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 나감");
            isPlayerNearby = false;
        }
    }
}
