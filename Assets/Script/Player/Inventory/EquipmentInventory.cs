using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : InventoryBase
{
    //[Header("현재 계산된 수치를 표현할 텍스트")] 나중에 추가
    
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
