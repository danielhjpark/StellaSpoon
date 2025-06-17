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
        if (CookManager.instance.isPickUpMenu && isPlayerNearby && Input.GetKeyDown(KeyCode.F) && DeviceManager.isDeactived) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        {
            UseGarbageCan();
        }

    }
    private void UseGarbageCan() //UI출력
    {
        serveSystem.ThrowOutMenu();
        //InteractUIManger.isUseInteractObject = true;
        //Sound 추가 필요 쓰레기통에 넣는 소리
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
