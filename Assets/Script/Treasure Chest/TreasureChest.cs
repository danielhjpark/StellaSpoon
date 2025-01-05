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
    }

    private void Update()
    {
        if(!treasuteChestPanel.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            OpenChestUI();
        }
        if(treasuteChestPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) //UI�� �����ְ� esc ������ ��
        {
            CloseChestUI();
        }
    }
    private void OpenChestUI() //�������� UI���
    {
        Debug.Log("�������� ����");
        treasuteChestPanel.SetActive(true);
        animator.SetTrigger("Open");
    }
    private void CloseChestUI() //�������� UI �ݱ�
    {
        Debug.Log("�������� �ݱ�");
        treasuteChestPanel.SetActive(false);
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
