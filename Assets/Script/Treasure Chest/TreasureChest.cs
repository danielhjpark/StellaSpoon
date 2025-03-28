using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureChest : MonoBehaviour
{
    [Header("�������� Information")]
    [SerializeField]
    private GameObject treasureChest; // �������� ������Ʈ
    private Animator animator;

    private bool isPlayerNearby = false; // �÷��̾� ����

    private GameObject treasureChestPanel; // �������� UI
    private GameObject inventoryPanel; // �κ��丮 UI
    private GameObject slotsSetting; // ���� ����

    [Header("�������ڿ� ������ ������")]
    [SerializeField]
    private List<Item> possibleItems; // ���� ������ ������ ����Ʈ
    [Header("������ ����")]
    [SerializeField]
    private int minItemCount = 1; // �ּ� ������ ��
    [SerializeField]
    private int maxItemCount = 3; // �ִ� ������ ��

    private Slot[] chestSlots; // �������� ���� �迭
    private bool isOpenChest = false; // �������� ���� ����
    public static bool openingChest = false; // ���������� ����

    private void Start()
    {
        if (treasureChest != null)
        {
            animator = treasureChest.GetComponent<Animator>();
            FindUIObjects(); // UI ������Ʈ ã��
        }
        else
        {
            Debug.LogWarning("TreasureChest ������Ʈ�� �Ҵ���� ����.");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIObjects(); // ���� ����� �� UI �ٽ� ã��
        ResetChest(); // �������� �ʱ�ȭ
    }

    private void FindUIObjects()
    {
        // �������� UI�� ���� ���Ҵ�
        treasureChestPanel = GameObject.Find("Canvas/PARENT_TreasureChestBase(DeactivateThis)/TreasureChestBase");
        inventoryPanel = GameObject.Find("Canvas/PARENT_InventoryBase(DeactivateThis)/InventoryBase");
        slotsSetting = GameObject.Find("Canvas/PARENT_TreasureChestBase(DeactivateThis)/TreasureChestBase/Slot Setting");

        if (slotsSetting != null)
        {
            chestSlots = slotsSetting.GetComponentsInChildren<Slot>();
        }
        else
        {
            Debug.LogWarning("slotsSetting�� ã�� �� �����ϴ�. ������ �ʱ�ȭ�� �� ����.");
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
        isOpenChest = false; // �������� ���� �ʱ�ȭ
        CreateItem(); // ������ ����
    }

    private void OpenChestUI()
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

        treasureChestPanel.SetActive(true);
        inventoryPanel.SetActive(true);

        Inventory.inventoryActivated = true;

        animator.SetTrigger("Open");
    }

    private void CloseChestUI()
    {
        openingChest = false;
        Debug.Log("�������� �ݱ�");

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
            Debug.LogWarning("chestSlots�� �������� �ʾҽ��ϴ�. �������� �߰��� �� �����ϴ�.");
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
                Debug.LogWarning("���� �ε��� �ʰ�. �� �̻� �������� �߰��� ������ �����ϴ�.");
                break;
            }
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
