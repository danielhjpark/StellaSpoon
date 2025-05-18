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
    public static bool openingStoreUI = false; //���� UI ��������
    [SerializeField]
    private bool iscollPlayer = false; //�÷��̾�� �浹�ߴ���

    [Header("Chat")]
    [SerializeField]
    private TypingEffect talkText; //��ȭâ �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI nameText; //���� �̸� ��� �ؽ�Ʈ
    [SerializeField]
    private string[] ingredientChatText; //������ ��ȭ ����
    [SerializeField]
    private string[] cookChatText; //�����ⱸ ���� ��ȭ ����
    [SerializeField]
    private string[] gunChatText; //�ѱ� ���� ��ȭ ����

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
        if(iscollPlayer)
        {
            if(!openingStoreUI)
            {
                if (Input.GetKeyDown(KeyCode.F)) //FŰ ������ �� 
                {
                    openingStoreUI = true;
                    //�÷��̾� �̵� ����
                    //ī�޶� �̵� ����
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    storeUIManager.CallChatUI();
                    switch (NPCTypes)
                    {
                        case NPCType.IngredientNPC:
                            ingredientBase.SetActive(true);//������ UI ���
                            ChangeChat(NPCType.IngredientNPC); //������ ��ȭ ���� ���
                            isIngredient = true;
                            break;
                        case NPCType.KitchenNPC:
                            CookBase.SetActive(true);//�ֹ���� UI ���
                            ChangeChat(NPCType.KitchenNPC); //�����ⱸ ���� ��ȭ ���� ���
                            break;
                        case NPCType.GunNPC:
                            GunBase.SetActive(true);//������� UI ���
                            ChangeChat(NPCType.GunNPC); //�ѱ� ���� ��ȭ ���� ���
                            break;
                    }
                    //��ȭâ UI ���
                    //�÷��̾� �̵� ����
                    //ī�޶� �̵�
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //�÷��̾� �̵� ���� ����
                    //ī�޶� �̵� ���� ����
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    switch (NPCTypes)
                    {
                        case NPCType.IngredientNPC:
                            ingredientBase.SetActive(false);//������ UI ����
                            isIngredient = false;
                            break;
                        case NPCType.KitchenNPC:
                            CookBase.SetActive(false);//�ֹ���� UI ����
                            break;
                        case NPCType.GunNPC:
                            GunBase.SetActive(false);//������� UI ����
                            break;
                    }
                    storeUIManager.CloseChatUI(); //��ȭâ UI �ݱ�
                    openingStoreUI = false;
                }
            }

        }        
    }

    public void ChangeChat(NPCType npcType)//�ش� NPC�� ��ȭ�ϱ�
    {
        switch (npcType)
        {
            case NPCType.IngredientNPC:
                talkText.StartTyping(ingredientChatText[Random.Range(0, ingredientChatText.Length)]);
                nameText.text = "��� ����";
                break;
            case NPCType.KitchenNPC:
                talkText.StartTyping(cookChatText[Random.Range(0, cookChatText.Length)]);
                nameText.text = "�ֹ��� ����";
                break;
            case NPCType.GunNPC:
                talkText.StartTyping(gunChatText[Random.Range(0, gunChatText.Length)]);
                nameText.text = "�� ����";
                break;
        }
    }
}
