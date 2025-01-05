using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField]
    private GameObject treasureChest; //보물상자 오브젝트
    private Animator animator;

    private bool isPlayerNearby = false; //플레이어 감지

    [SerializeField]
    private GameObject treasuteChestPanel; //보물상자 UI


    private void Start()
    {
        if(treasureChest != null)
        {
            animator = treasureChest.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("TreasureChest 오브젝트가 할당되지 않음.");
        }
    }

    private void Update()
    {
        if(!treasuteChestPanel.activeSelf && isPlayerNearby && Input.GetKeyDown(KeyCode.F)) //UI가 닫혀있고 주변 플레이어가 있고 F키 눌렀을 때
        {
            OpenChestUI();
        }
        if(treasuteChestPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) //UI가 열려있고 esc 눌렀을 때
        {
            CloseChestUI();
        }
    }
    private void OpenChestUI() //보물상자 UI출력
    {
        Debug.Log("보물상자 열기");
        treasuteChestPanel.SetActive(true);
        animator.SetTrigger("Open");
    }
    private void CloseChestUI() //보물상자 UI 닫기
    {
        Debug.Log("보물상자 닫기");
        treasuteChestPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어 나감");
            isPlayerNearby = false;
        }
    }
}
