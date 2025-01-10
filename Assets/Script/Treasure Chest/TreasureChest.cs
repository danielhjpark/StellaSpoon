using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField]
    private GameObject treasureChest; //�������� ������Ʈ
    private Animator animator;

    private bool isPlayerNearby = false; //�÷��̾� ����

    [SerializeField]
    private GameObject treasuteChestPanel; //�������� UI
    [SerializeField]
    private GameObject inventorypanel; //�κ��丮 UI

    [SerializeField]
    private GameObject CloseButton; //�κ��丮 �� �ݱ��ư UI

    [SerializeField]
    private GameObject slotsSetting; //���� ����
    [SerializeField]
    private List<Item> possibleItems; //���� ������ ������ ����Ʈ

    [SerializeField]
    private int minItemCount = 1; //�ּ� ������ ��
    [SerializeField]
    private int maxItemCount = 3; //�ִ� ������ ��

    [SerializeField]
    private Slot[] chestSlots; //�������� ���� �迭
    private bool isOpenChest = false; //�������� ���� ����



    private void Start()
    {
        if (treasureChest != null)
        {
            animator = treasureChest.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("TreasureChest ������Ʈ�� �Ҵ���� ����.");
        }
        chestSlots = slotsSetting.GetComponentsInChildren<Slot>();
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
    private void OpenChestUI() //�������� UI���
    {
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
        CloseButton.SetActive(false);

        Inventory.inventoryActivated = true;

        animator.SetTrigger("Open");
    }
    private void CloseChestUI() //�������� UI �ݱ�
    {
        Debug.Log("�������� �ݱ�");

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