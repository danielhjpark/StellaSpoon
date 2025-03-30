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
            //�÷��̾� �̵� ����
            //ī�޶� �̵�
            switch (NPCTypes)
            {
                case NPCType.IngredientNPC:
                    ingredientBase.SetActive(true);//������ UI ���
                    isIngredient = true;
                    break;
                case NPCType.KitchenNPC:
                    //�ֹ���� UI ���
                    break;
                case NPCType.GunNPC:
                    //������� UI ���
                    break;
            }
            storeUIManager.CallChatUI(); //��ȭâ UI ���
        }
    }

    private void Update()
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
                    //�ֹ���� UI ����
                    break;
                case NPCType.GunNPC:
                    //������� UI ����
                    break;
            }
            storeUIManager.CloseChatUI(); //��ȭâ UI �ݱ�
        }
    }
    //UI ���� ��ũ��Ʈ �ۼ�
    //UI ��ũ��Ʈ ������ ��� ���� �� ������ �߰� ����

    //escŰ ������ �� �÷��̾� �̵� ���� ����, ī�޶� �⺻���� ���ư���
}
