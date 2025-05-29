using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private GameObject CookBase;
    [SerializeField]
    private GameObject GunBase;

    [SerializeField]
    private StoreUIManager storeUIManager;

    static public bool isIngredient = false;

    [SerializeField]
    public static bool openingStoreUI = false; //상점 UI 열었는지
    [SerializeField]
    private bool iscollPlayer = false; //플레이어와 충돌했는지

    [Header("Chat")]
    [SerializeField]
    private TypingEffect talkText; //대화창 텍스트
    [SerializeField]
    private TextMeshProUGUI nameText; //상인 이름 출력 텍스트
    [SerializeField]
    private string[] ingredientChatText; //재료상인 대화 내용
    [SerializeField]
    private string[] cookChatText; //조리기구 상인 대화 내용
    [SerializeField]
    private string[] gunChatText; //총기 상인 대화 내용

    private InteractUI interactUI; //상호작용 패널

    private void Awake()
    {
        // Canvas의 자식 오브젝트 중 WeaponChanger 찾기
        Transform canvasTransform = GameObject.Find("Canvas")?.transform; // Canvas를 찾기
        interactUI = canvasTransform.Find("InteractPanel")?.GetComponent<InteractUI>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            iscollPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            iscollPlayer = false;
        }
    }

    private void Update()
    {
        if (iscollPlayer)
        {
            if (!openingStoreUI)
            {
                interactUI.UseInteractUI(this.gameObject, Vector2.down * 0.5f);
                if (Input.GetKeyDown(KeyCode.F)) //F키 눌렀을 때 
                {
                    openingStoreUI = true;
                    //플레이어 이동 제한
                    //카메라 이동 제한
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    storeUIManager.CallChatUI();
                    switch (NPCTypes)
                    {
                        case NPCType.IngredientNPC:
                            ingredientBase.SetActive(true);//재료상점 UI 출력
                            ChangeChat(NPCType.IngredientNPC); //재료상인 대화 내용 출력
                            isIngredient = true;
                            break;
                        case NPCType.KitchenNPC:
                            CookBase.SetActive(true);//주방상점 UI 출력
                            ChangeChat(NPCType.KitchenNPC); //조리기구 상인 대화 내용 출력
                            break;
                        case NPCType.GunNPC:
                            GunBase.SetActive(true);//무기상점 UI 출력
                            ChangeChat(NPCType.GunNPC); //총기 상인 대화 내용 출력
                            break;
                    }
                    //대화창 UI 출력
                    //플레이어 이동 제한
                    //카메라 이동
                }
            }
            else
            {
                interactUI.DisableInteractUI(this.gameObject);
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //플레이어 이동 제한 해제
                    //카메라 이동 제한 해제
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    switch (NPCTypes)
                    {
                        case NPCType.IngredientNPC:
                            ingredientBase.SetActive(false);//재료상점 UI 끄기
                            isIngredient = false;
                            break;
                        case NPCType.KitchenNPC:
                            CookBase.SetActive(false);//주방상점 UI 끄기
                            break;
                        case NPCType.GunNPC:
                            GunBase.SetActive(false);//무기상점 UI 끄기
                            break;
                    }
                    storeUIManager.CloseChatUI(); //대화창 UI 닫기
                    openingStoreUI = false;
                }
            }

        }
        else interactUI.DisableInteractUI(this.gameObject);   
    }

    public void ChangeChat(NPCType npcType)//해당 NPC와 대화하기
    {
        switch (npcType)
        {
            case NPCType.IngredientNPC:
                talkText.StartTyping(ingredientChatText[Random.Range(0, ingredientChatText.Length)]);
                nameText.text = "재료 상인";
                break;
            case NPCType.KitchenNPC:
                talkText.StartTyping(cookChatText[Random.Range(0, cookChatText.Length)]);
                nameText.text = "주방기기 상인";
                break;
            case NPCType.GunNPC:
                talkText.StartTyping(gunChatText[Random.Range(0, gunChatText.Length)]);
                nameText.text = "총 장인";
                break;
        }
    }
}
