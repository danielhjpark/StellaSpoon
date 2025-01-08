using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField]
    private GameObject treasureChest; //�������� ������Ʈ
    private Animator animator;

    private bool isPlayerNearby = false; //�÷��̾� ����
    private bool isChestOpened = false; //�������ڰ� �̹� ���ȴ��� Ȯ��

    [SerializeField]
    private GameObject treasuteChestBase; //�������� UI
    [SerializeField]
    private GameObject inventoryBase; //�κ��丮 UI

    private GameObject closeButton; //�κ��丮 �� �ݱ� ��ư UI

    [Header("��������")]
    [SerializeField]
    private GameObject gridSetting; //Slot���� �θ��� Grid Setting
    private Slot[] chestSlots; //�������� ���� �迭

    [Header("������ ����Ʈ")]
    [SerializeField]
    private List<Item> possibleItems; //�������ڿ� ���� ������ ���

    [Header("������ ����")]
    [SerializeField]
    private int minItems = 1; //�ּ� ������ ��
    [SerializeField]
    private int maxItems = 3; //�ִ� ������ ��

    private List<Item> chestItems = new List<Item>(); //�������ڿ� �� ������ ����Ʈ



    private void Start()
    {
        if(treasureChest != null)
        {
            animator = treasureChest.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("TreasureChest ������Ʈ�� �Ҵ���� ����.");
        }
        
        inventoryBase = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)/InventoryBase");
        if(inventoryBase == null)
        {
            Debug.Log("InventoryBase ������Ʈ�� �Ҵ���� ����.");
        }

        treasuteChestBase = GameObject.Find("Canvas/PARENT_TreasureChest(DeactivateThis)/TreasureChestBase");
        if(treasuteChestBase == null)
        {
            Debug.Log("TreasureChestBase ������Ʈ�� �Ҵ���� ����.");
        }

        closeButton = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)/InventoryBase/Close Inventory Button");
        if(closeButton == null)
        {
            Debug.Log("closeButton ������Ʈ�� �Ҵ���� ����.");
        }

        if(gridSetting != null)
        {
            chestSlots = gridSetting.GetComponentsInChildren<Slot>();
        }
    }

    private void Update()
    {
        if(!treasuteChestBase.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            OpenChestUI();
        }
        if(treasuteChestBase.activeSelf && Input.GetKeyDown(KeyCode.Escape)) //UI�� �����ְ� esc ������ ��
        {
            CloseChestUI();
        }
    }
    private void OpenChestUI() //�������� UI���
    {
        Debug.Log("�������� ����");

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
    private void CloseChestUI() //�������� UI �ݱ�
    {
        Debug.Log("�������� �ݱ�");

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
        int itemCount = Random.Range(minItems, maxItems + 1); // ������ ������ ��
        chestItems.Clear();

        for (int i = 0; i < itemCount; i++)
        {
            int randomIndex = Random.Range(0, possibleItems.Count);
            chestItems.Add(possibleItems[randomIndex]);
        }
    }

    // �������� UI ������Ʈ
    private void UpdateChestUI()
    {
        if (chestSlots == null)
        {
            Debug.LogError("ChestSlots �迭�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        for (int i = 0; i < chestSlots.Length; i++)
        {
            if (i < chestItems.Count)
            {
                chestSlots[i].AddItem(chestItems[i], 1); // �����۰� ������ ���Կ� �߰�
            }
            else
            {
                chestSlots[i].ClearSlot(); // ���� �ʱ�ȭ
            }
        }
    }  

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����");
            isPlayerNearby = false;
        }
    }
}
