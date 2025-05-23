using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : MonoBehaviour
{
    [SerializeField] private ServeSystem serveSystem;
    void Start()
    {
        if (serveSystem == null) serveSystem = FindObjectOfType<ServeSystem>();
    }

    private void Update()
    {
        if (CookManager.instance.isPickUpMenu && InteractUIManger.isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI�� �����ְ� �ֺ� �÷��̾ �ְ� FŰ ������ ��
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
            InteractUIManger.isPlayerNearby = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractUIManger.isPlayerNearby = false;
        }
    }
}
