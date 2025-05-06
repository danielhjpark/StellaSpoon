using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : MonoBehaviour
{
    [SerializeField] private ServeSystem serveSystem;
    bool isHaveMenu;

    void Start()
    {
        isHaveMenu = false;
        if (serveSystem == null) serveSystem = FindObjectOfType<ServeSystem>();
    }

    private void Update()
    {
        if (InteractUIManger.isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        {
            UseGarbageCan();
        }

    }
    private void UseGarbageCan() //UI출력
    {
        serveSystem.ThrowOutMenu();
        //Sound 추가 필요 쓰레기통에 넣는 소리
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
