using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class TreasureChest : MonoBehaviour
{
    [Header("보물상자 Information")]
    [SerializeField]
    private GameObject treasureChest; //보물상자 오브젝트
    private Animator animator;

    private bool isPlayerNearby = false; //플레이어 감지

    private GameObject treasureChestPanel; //보물상자 UI
    private GameObject inventoryPanel; //인벤토리 UI
    private GameObject inventoryBackGround; //인벤토리 배경
    private GameObject slotsSetting; //슬롯 셋팅

    [SerializeField]
    private GameObject lightObject; //보물상자에 있는 빛 오브젝트

    [Header("보물상자에 나오는 아이템")]
    [SerializeField]
    private List<Item> possibleItems; //생성 가능한 아이템 리스트
    
    [Header("아이템 갯수")]
    [SerializeField]
    private List<int> minItemCount; //최초 생성 아이템 수 리스트
    [SerializeField]
    private List<int> maxItemCount; //최대 아이템 수 리스트

    [Header("아이템별 확률")]
    [SerializeField]
    private List<int> itemPrecent; //아이템 생성 확률
   

    private Slot[] chestSlots; //보물상자 슬롯 배열
    private bool isOpenChest = false; //보물상자 오픈 여부
    public static bool openingChest = false; //보물상자 오픈중인지 여부

    private void Start()
    {
        if (treasureChest != null)
        {
            animator = treasureChest.GetComponent<Animator>();
            FindUIObjects(); //UI 오브젝트 찾기
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
        FindUIObjects(); //씬이 변경될 때 UI 다시 찾기
        ResetChest(); //보물상자 초기화
    }

    private void FindUIObjects()
    {
        //보물상자 UI와 슬롯 재할당
        treasureChestPanel = GameObject.Find("Canvas/PARENT_TreasureChestBase(DeactivateThis)/TreasureChestBackGround");
        inventoryPanel = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)/InventoryBase");
        inventoryBackGround = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)/InventoryBackGround");
        slotsSetting = GameObject.Find("Canvas/PARENT_TreasureChestBase(DeactivateThis)/TreasureChestBackGround/TreasureChestBase/Slot Setting");

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
        isOpenChest = false; //보물상자 상태 초기화
        CreateItem(); //아이템 생성
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
        inventoryBackGround.SetActive(true);

        Inventory.inventoryActivated = true;

        animator.SetTrigger("Open");

        StartCoroutine(LightOn());
    }

    private IEnumerator LightOn()
    {
        yield return new WaitForSeconds(0.8f);

        lightObject.SetActive(true); //보물상자에 있는 빛 오브젝트 활성화
    }

    private void CloseChestUI()
    {
        openingChest = false;
        Debug.Log("보물상자 닫기");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        treasureChestPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        inventoryBackGround.SetActive(false);

        Inventory.inventoryActivated = false;
    }

    private void CreateItem()
    {
        if (chestSlots == null || chestSlots.Length == 0)
        {
            Debug.LogWarning("chestSlots가 설정되지 않았습니다. 아이템을 추가할 수 없습니다.");
            return;
        }

        int totalSlotCount = chestSlots.Length;

        for (int i = 0; i < possibleItems.Count; i++)
        {
            float rand = Random.Range(0, 100f);
            Debug.Log(rand);
            if(rand > itemPrecent[i])
            {
                continue; //생성 실패
            }

            int count = Random.Range(minItemCount[i], maxItemCount[i] + 1); //최소~최대 갯수 랜덤 생성

            Item currentItem = possibleItems[i];

            bool addedToExistingSlot = false;
            foreach (Slot slot in chestSlots)
            {
                if (slot.item != null && slot.item.itemName == currentItem.itemName)
                {
                    slot.SetSlotCount(slot.itemCount + count);
                    addedToExistingSlot = true;
                    break;
                }
            }
            // 4. 기존 슬롯에 없으면 새로운 슬롯에 추가
            if (!addedToExistingSlot)
            {
                for (int k = 0; k < totalSlotCount; k++)
                {
                    if (chestSlots[k].item == null)
                    {
                        chestSlots[k].AddItemWithoutWeight(currentItem, count);
                        break;
                    }
                    if (k == totalSlotCount - 1)
                    {
                        Debug.LogWarning("보물상자 슬롯이 부족합니다. 일부 아이템은 추가되지 않았습니다.");
                    }
                }
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
