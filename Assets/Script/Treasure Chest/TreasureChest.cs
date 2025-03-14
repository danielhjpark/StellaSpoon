using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureChest : MonoBehaviour
{
    [Header("보물상자 Information")]
    [SerializeField]
    private GameObject treasureChest; //보물상자 오브젝트
    private Animator animator;

    private bool isPlayerNearby = false; //플레이어 감지

    private GameObject treasuteChestPanel; //보물상자 UI
    private GameObject inventorypanel; //인벤토리 UI
    private GameObject slotsSetting; //슬롯 셋팅

    [Header("보물상자에 나오는 아이템")]
    [SerializeField]
    private List<Item> possibleItems; //생성 가능한 아이템 리스트
    [Header("아이템 갯수")]
    [SerializeField]
    private int minItemCount = 1; //최소 아이템 수
    [SerializeField]
    private int maxItemCount = 3; //최대 아이템 수

    private Slot[] chestSlots; //보물상자 슬롯 배열
    private bool isOpenChest = false; //보물상자 오픈 여부
    public static bool openingChest = false; //오픈중인지 여부



    private void Start()
    {
        if (treasureChest != null)
        {
            animator = treasureChest.GetComponent<Animator>();
            treasuteChestPanel = GameObject.Find("Canvas/PARENT_TreasureChestBase(DeactivateThis)/TreasureChestBase");
            inventorypanel = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)/InventoryBase");
            slotsSetting = GameObject.Find("Canvas/PARENT_TreasureChestBase(DeactivateThis)/TreasureChestBase/Slot Setting");

        }
        else
        {
            Debug.Log("TreasureChest 오브젝트가 할당되지 않음.");
        }
        chestSlots = slotsSetting.GetComponentsInChildren<Slot>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetChest();
    }
    private void Update()
    {
        if (!treasuteChestPanel.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        {
            OpenChestUI();
        }
        if (treasuteChestPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) //UI가 열려있고 esc 눌렀을 때
        {
            CloseChestUI();
        }
    }
    private void ResetChest()
    {
        isOpenChest = false; // 보물상자 상태 초기화
        CreateItem(); // 아이템 생성
    }
    private void OpenChestUI() //보물상자 UI출력
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

        treasuteChestPanel.SetActive(true);
        inventorypanel.SetActive(true);

        Inventory.inventoryActivated = true;

        animator.SetTrigger("Open");
    }
    private void CloseChestUI() //보물상자 UI 닫기
    {
        openingChest = false;
        Debug.Log("보물상자 닫기");

        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;

        treasuteChestPanel.SetActive(false);
        inventorypanel.SetActive(false);

        Inventory.inventoryActivated = false;
    }

    private void CreateItem()
    {
        int itemCount = Random.Range(minItemCount, maxItemCount + 1);

        for (int i = 0; i < itemCount; i++)
        {
            Item randomItem = possibleItems[Random.Range(0, possibleItems.Count)];
            chestSlots[i].AddItemWithoutWeight(randomItem, 1);
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