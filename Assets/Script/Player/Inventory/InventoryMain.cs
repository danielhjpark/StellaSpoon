using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �������� ���� ���� �⺻���� �κ��丮

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

    /// �κ��丮�� ����.
    public void OpenInventory()
    {
        mInventoryBase.SetActive(true);
        device.uiPanel.SetActive(false);
    }

    /// �κ��丮�� �ݴ´�.
    public void CloseInventory()
    {
        mInventoryBase.SetActive(false);
        device.uiPanel.SetActive(true);
    }
}
