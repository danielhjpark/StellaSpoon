using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : InteractObject
{
    [SerializeField] private ServeSystem serveSystem;
    bool isPlayerNearby;

    void Start()
    {
        if (serveSystem == null) serveSystem = FindObjectOfType<ServeSystem>();
    }

    private void Update()
    {
        if (CookManager.instance.isPickUpMenu && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && DeviceManager.isDeactived) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
        {
            UseGarbageCan();
        }

    }
    private void UseGarbageCan() //UI���
    {
        serveSystem.ThrowOutMenu();
        //InteractUIManger.isUseInteractObject = true;
        //Sound �߰� �ʿ� �������뿡 �ִ� �Ҹ�
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            InteractUIManger.isPlayerNearby = true;
            InteractUIManger.currentInteractObject = this.gameObject;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            InteractUIManger.isPlayerNearby = false;
        }
    }
}
