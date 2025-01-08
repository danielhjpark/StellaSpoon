using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField]
    private GameObject treasureChest; //보물상자 오브젝트
    private Animator animator;

    private bool isPlayerNearby = false; //플레이어 감지
    private bool isChestOpened = false; //보물상자가 이미 열렸는지 확인

    [SerializeField]
    private GameObject treasuteChestBase; //보물상자 UI
    [SerializeField]
    private GameObject inventoryBase; //인벤토리 UI

    private GameObject closeButton; //인벤토리 내 닫기 버튼 UI

    [Header("보물상자")]
    [SerializeField]
    private GameObject gridSetting; //Slot들의 부모인 Grid Setting
    private Slot[] chestSlots; //보물상자 슬롯 배열

    [Header("아이템 리스트")]
    [SerializeField]
    private List<Item> possibleItems; //보물상자에 들어가는 아이템 목록

    [Header("아이템 갯수")]
    [SerializeField]
    private int minItems = 1; //최소 아이템 수
    [SerializeField]
    private int maxItems = 3; //최대 아이템 수

    private List<Item> chestItems = new List<Item>(); //보물상자에 들어갈 아이템 리스트



    private void Start()
    {
        if(treasureChest != null)
        {
            animator = treasureChest.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("TreasureChest 오브젝트가 할당되지 않음.");
        }
        
        inventoryBase = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)/InventoryBase");
        if(inventoryBase == null)
        {
            Debug.Log("InventoryBase 오브젝트가 할당되지 않음.");
        }

        treasuteChestBase = GameObject.Find("Canvas/PARENT_TreasureChest(DeactivateThis)/TreasureChestBase");
        if(treasuteChestBase == null)
        {
            Debug.Log("TreasureChestBase 오브젝트가 할당되지 않음.");
        }

        closeButton = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)/InventoryBase/Close Inventory Button");
        if(closeButton == null)
        {
            Debug.Log("closeButton 오브젝트가 할당되지 않음.");
        }

        if(gridSetting != null)
        {
            chestSlots = gridSetting.GetComponentsInChildren<Slot>();
        }
    }

    private void Update()
    {
        if(!treasuteChestBase.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        {
            OpenChestUI();
        }
        if(treasuteChestBase.activeSelf && Input.GetKeyDown(KeyCode.Escape)) //UI가 열려있고 esc 눌렀을 때
        {
            CloseChestUI();
        }
    }
    private void OpenChestUI() //보물상자 UI출력
    {
        Debug.Log("보물상자 열기");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        treasuteChestBase.SetActive(true);
        inventoryBase.SetActive(true);
        closeButton.SetActive(false);
        animator.SetTrigger("Open");

        if(!isChestOpened)
        {
            GenerateRandomItems();
            UpdateChestUI();
            isChestOpened = true;
        }
    }
    private void CloseChestUI() //보물상자 UI 닫기
    {
        Debug.Log("보물상자 닫기");

        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;

        //SaveChestState();

        treasuteChestBase.SetActive(false);
        inventoryBase.SetActive(false);
    }

    private void GenerateRandomItems()
    {
        if(isChestOpened)
        {
            return;
        }
        int itemCount = Random.Range(minItems, maxItems + 1); // 생성할 아이템 수
        chestItems.Clear();

        for (int i = 0; i < itemCount; i++)
        {
            int randomIndex = Random.Range(0, possibleItems.Count);
            chestItems.Add(possibleItems[randomIndex]);
        }
    }

    // 보물상자 UI 업데이트
    private void UpdateChestUI()
    {
        if (chestSlots == null)
        {
            Debug.LogError("ChestSlots 배열이 초기화되지 않았습니다.");
            return;
        }

        for (int i = 0; i < chestSlots.Length; i++)
        {
            if (i < chestItems.Count)
            {
                chestSlots[i].AddItem(chestItems[i], 1); // 아이템과 수량을 슬롯에 추가
            }
            else
            {
                chestSlots[i].ClearSlot(); // 슬롯 초기화
            }
        }
    }  

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어 나감");
            isPlayerNearby = false;
        }
    }
}
