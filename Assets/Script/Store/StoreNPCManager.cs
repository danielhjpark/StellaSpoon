using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType
{
    IngredientNPC,
    KitchenNPC,
    GunNPC
}
public class StoreNPCManager : MonoBehaviour
{
    [SerializeField]
    private NPCType NPCTypes;

    [SerializeField]
    private GameObject ingredientBase;

    [SerializeField]
    private StoreUIManager storeUIManager;

    static public bool isIngredient = false;

    private void Awake()
    {
        storeUIManager = GetComponent<StoreUIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            //플레이어 이동 제한
            //카메라 이동
            switch (NPCTypes)
            {
                case NPCType.IngredientNPC:
                    ingredientBase.SetActive(true);//재료상점 UI 출력
                    isIngredient = true;
                    break;
                case NPCType.KitchenNPC:
                    //주방상점 UI 출력
                    break;
                case NPCType.GunNPC:
                    //무기상점 UI 출력
                    break;
            }
            storeUIManager.CallChatUI(); //대화창 UI 출력
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //플레이어 이동 제한 해제
            //카메라 기본으로 돌아가기
            switch (NPCTypes)
            {
                case NPCType.IngredientNPC:
                    ingredientBase.SetActive(false);//재료상점 UI 끄기
                    isIngredient = false;
                    break;
                case NPCType.KitchenNPC:
                    //주방상점 UI 끄기
                    break;
                case NPCType.GunNPC:
                    //무기상점 UI 끄기
                    break;
            }
            storeUIManager.CloseChatUI(); //대화창 UI 닫기
        }
    }
    //UI 별로 스크립트 작성
    //UI 스크립트 내에서 골드 감소 밑 아이템 추가 구현

    //esc키 눌렀을 때 플레이어 이동 제한 해제, 카메라 기본으로 돌아가기
}
