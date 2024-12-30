using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : InventoryBase
{
    //[Header("���� ���� ��ġ�� ǥ���� �ؽ�Ʈ")] ���߿� �߰�
    
    new private void Awake()
    {
        base.Awake();
    }

    public void OpenEquipment()
    {
        mInventoryBase.SetActive(true);
    }

    public void CloseEquipment()
    {
        mInventoryBase.SetActive(false);
    }
}
