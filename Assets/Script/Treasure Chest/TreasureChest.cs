using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureChest : MonoBehaviour
{
    [Header("�������� Information")]
    [SerializeField]
    private GameObject treasureChest; //�������� ������Ʈ
    private Animator animator;

    private bool isPlayerNearby = false; //�÷��̾� ����

    private GameObject treasuteChestPanel; //�������� UI
    private GameObject inventorypanel; //�κ��丮 UI
    private GameObject slotsSetting; //���� ����

    [Header("�������ڿ� ������ ������")]
    [SerializeField]
    private List<Item> possibleItems; //���� ������ ������ ����Ʈ
    [Header("������ ����")]
    [SerializeField]
    private int minItemCount = 1; //�ּ� ������ ��
    [SerializeField]
    private int maxItemCount = 3; //�ִ� ������ ��

    private Slot[] chestSlots; //�������� ���� �迭
    private bool isOpenChest = false; //�������� ���� ����
    public static bool openingChest = false; //���������� ����



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
            Debug.Log("TreasureChest ������Ʈ�� �Ҵ���� ����.");
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
        if (!treasuteChestPanel.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && !Inventory.inventoryActivated) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            OpenChestUI();
        }
        if (treasuteChestPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) //UI�� �����ְ� esc ������ ��
        {
            CloseChestUI();
        }
    }
    private void ResetChest()
    {
        isOpenChest = false; // �������� ���� �ʱ�ȭ
        CreateItem(); // ������ ����
    }
    private void OpenChestUI() //�������� UI���
    {
        openingChest = true;
        if (!isOpenChest)
        {
            CreateItem();
            isOpenChest = true;
        }
        Debug.Log("�������� ����");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        treasuteChestPanel.SetActive(true);
        inventorypanel.SetActive(true);

        Inventory.inventoryActivated = true;

        animator.SetTrigger("Open");
    }
    private void CloseChestUI() //�������� UI �ݱ�
    {
        openingChest = false;
        Debug.Log("�������� �ݱ�");

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
            Debug.Log("�÷��̾� ����");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����");
            isPlayerNearby = false;
        }
    }
}