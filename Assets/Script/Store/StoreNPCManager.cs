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
                    switch (NPCTypes)
                    {
                        case NPCType.IngredientNPC:
                            ingredientBase.SetActive(true);//������ UI ���
                            isIngredient = true;
                            break;
                        case NPCType.KitchenNPC:
                            CookBase.SetActive(true);//�ֹ���� UI ���
                            break;
                        case NPCType.GunNPC:
                            GunBase.SetActive(true);//������� UI ���
                            break;
                    }
                    storeUIManager.CallChatUI(); //��ȭâ UI ���
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
    //UI ���� ��ũ��Ʈ �ۼ�
    //UI ��ũ��Ʈ ������ ��� ���� �� ������ �߰� ����

    //escŰ ������ �� �÷��̾� �̵� ���� ����, ī�޶� �⺻���� ���ư���
}
