using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField]
    private GameObject treasureChest; //보물상자 오브젝트
    private Animator animator;

    private bool isPlayerNearby = false; //플레이어 감지

    [SerializeField]
    private GameObject treasuteChestPanel; //보물상자 UI
    [SerializeField]
    private GameObject inventorypanel; //인벤토리 UI

    [SerializeField]
    private GameObject CloseButton; //인벤토리 내 닫기버튼 UI

    [SerializeField]
    private GameObject slotsSetting; //슬롯 셋팅
    [SerializeField]
    private List<Item> possibleItems; //생성 가능한 아이템 리스트

    [SerializeField]
    private int minItemCount = 1; //최소 아이템 수
    [SerializeField]
    private int maxItemCount = 3; //최대 아이템 수

    [SerializeField]
    private Slot[] chestSlots; //보물상자 슬롯 배열
    private bool isOpenChest = false; //보물상자 오픈 여부



    private void Start()
    {
        if (treasureChest != null)
        {
            animator = treasureChest.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("TreasureChest 오브젝트가 할당되지 않음.");
        }
        chestSlots = slotsSetting.GetComponentsInChildren<Slot>();
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
    private void OpenChestUI() //보물상자 UI출력
    {
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
        CloseButton.SetActive(false);

        Inventory.inventoryActivated = true;

        animator.SetTrigger("Open");
    }
    private void CloseChestUI() //보물상자 UI 닫기
    {
        Debug.Log("보물상자 닫기");

        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;

        treasuteChestPanel.SetActive(false);
        inventorypanel.SetActive(false);
        CloseButton.SetActive(true);

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