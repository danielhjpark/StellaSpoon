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
                    break;
                case NPCType.KitchenNPC:
                    //�ֹ���� UI ���
                    break;
                case NPCType.GunNPC:
                    //������� UI ���
                    break;
            }
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
                    break;
                case NPCType.KitchenNPC:
                    //�ֹ���� UI ����
                    break;
                case NPCType.GunNPC:
                    //������� UI ����
                    break;
            }
        }
    }
    //UI ���� ��ũ��Ʈ �ۼ�
    //UI ��ũ��Ʈ ������ ��� ���� �� ������ �߰� ����

    //escŰ ������ �� �÷��̾� �̵� ���� ����, ī�޶� �⺻���� ���ư���
}
