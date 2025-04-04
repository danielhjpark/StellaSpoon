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

    private bool openStoreUI = false; //���� UI ��������
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
            if(Input.GetKeyDown(KeyCode.F)) //FŰ ������ �� 
            {
                openStoreUI = true;
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
            if(openStoreUI) //���� UI�� �������� �� 
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //�÷��̾� �̵� ���� ����
                    //ī�޶� �⺻���� ���ư���
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
                    openStoreUI = false;
                }
            }
        }
        
    }
    //UI ���� ��ũ��Ʈ �ۼ�
    //UI ��ũ��Ʈ ������ ��� ���� �� ������ �߰� ����

    //escŰ ������ �� �÷��̾� �̵� ���� ����, ī�޶� �⺻���� ���ư���
}
