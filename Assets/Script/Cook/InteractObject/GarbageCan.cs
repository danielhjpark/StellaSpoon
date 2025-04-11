using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : MonoBehaviour
{
    [SerializeField] private ServeSystem serveSystem;
    bool isPlayerNearby;
    bool isOpenedDevice;
    bool isHaveMenu;

    void Start()
    {
        isPlayerNearby = false;
        isOpenedDevice = false;
        isHaveMenu = false;
        if (serveSystem == null) serveSystem = FindObjectOfType<ServeSystem>();
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            UseGarbageCan();
        }

    }
    private void UseGarbageCan() //UI���
    {
        isOpenedDevice = true;
        UIManager.instance.HideInteractUI();
        if (Input.GetKeyDown(KeyCode.F))
        {
            serveSystem.ThrowOutMenu();
            //Sound �߰� �ʿ� �������뿡 �ִ� �Ҹ�
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            UIManager.instance.VisibleInteractUI();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            UIManager.instance.HideInteractUI();
        }
    }
}
