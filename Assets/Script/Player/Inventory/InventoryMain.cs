using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여러 아이템을 담을 가장 기본적인 인벤토리

public class InventoryMain : InventoryBase
{

    private DeviceManager device;

    new void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        device = GetComponent<DeviceManager>();
    }

    /// 인벤토리를 연다.
    public void OpenInventory()
    {
        mInventoryBase.SetActive(true);
        device.uiPanel.SetActive(false);
    }

    /// 인벤토리를 닫는다.
    public void CloseInventory()
    {
        mInventoryBase.SetActive(false);
        device.uiPanel.SetActive(true);
    }
}
