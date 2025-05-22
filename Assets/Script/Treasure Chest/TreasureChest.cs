using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class TreasureChest : MonoBehaviour
{
    [Header("�������� Information")]
    [SerializeField]
    private GameObject treasureChest; //�������� ������Ʈ
    private Animator animator;

    private bool isPlayerNearby = false; //�÷��̾� ����

    private GameObject treasureChestPanel; //�������� UI
    private GameObject inventoryPanel; //�κ��丮 UI
    private GameObject inventoryBackGround; //�κ��丮 ���
    private GameObject slotsSetting; //���� ����

    [SerializeField]
    private GameObject lightObject; //�������ڿ� �ִ� �� ������Ʈ

    [Header("�������ڿ� ������ ������")]
    [SerializeField]
    private List<Item> possibleItems; //���� ������ ������ ����Ʈ
    
    [Header("������ ����")]
    [SerializeField]
    private List<int> minItemCount; //���� ���� ������ �� ����Ʈ
    [SerializeField]
    private List<int> maxItemCount; //�ִ� ������ �� ����Ʈ

    [Header("�����ۺ� Ȯ��")]
    [SerializeField]
    private List<int> itemPrecent; //������ ���� Ȯ��
   

    private Slot[] chestSlots; //�������� ���� �迭
    private bool isOpenChest = false; //�������� ���� ����
    public static bool openingChest = false; //�������� ���������� ����

    private void Start()
    {
        if (treasureChest != null)
        {
            animator = treasureChest.GetComponent<Animator>();
            FindUIObjects(); //UI ������Ʈ ã��
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
        FindUIObjects(); //���� ����� �� UI �ٽ� ã��
        ResetChest(); //�������� �ʱ�ȭ
    }

    private void FindUIObjects()
    {
        //�������� UI�� ���� ���Ҵ�
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
        isOpenChest = false; //�������� ���� �ʱ�ȭ
        CreateItem(); //������ ����
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
        inventoryBackGround.SetActive(true);

        Inventory.inventoryActivated = true;

        animator.SetTrigger("Open");

        StartCoroutine(LightOn());
    }

    private IEnumerator LightOn()
    {
        yield return new WaitForSeconds(0.8f);

        lightObject.SetActive(true); //�������ڿ� �ִ� �� ������Ʈ Ȱ��ȭ
    }

    private void CloseChestUI()
    {
        openingChest = false;
        Debug.Log("�������� �ݱ�");

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
            Debug.LogWarning("chestSlots�� �������� �ʾҽ��ϴ�. �������� �߰��� �� �����ϴ�.");
            return;
        }

        int totalSlotCount = chestSlots.Length;

        for (int i = 0; i < possibleItems.Count; i++)
        {
            float rand = Random.Range(0, 100f);
            Debug.Log(rand);
            if(rand > itemPrecent[i])
            {
                continue; //���� ����
            }

            int count = Random.Range(minItemCount[i], maxItemCount[i] + 1); //�ּ�~�ִ� ���� ���� ����

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
            // 4. ���� ���Կ� ������ ���ο� ���Կ� �߰�
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
                        Debug.LogWarning("�������� ������ �����մϴ�. �Ϻ� �������� �߰����� �ʾҽ��ϴ�.");
                    }
                }
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
