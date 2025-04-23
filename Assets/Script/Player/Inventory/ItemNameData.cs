using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNameData : MonoBehaviour
{
    [SerializeField]
    private SlotToolTip slotToolTip;

    [SerializeField]
    private GameObject inventory;
    [SerializeField]
    private GameObject refriger;
    [SerializeField]
    private GameObject chest;

    private void Update()
    {
        // �κ��丮, �����, ���ڰ� ���� ���� ������ ���� �����
        if (!inventory.activeSelf && !refriger.activeSelf && !chest.activeSelf)
        {
            if (slotToolTip != null && slotToolTip.IsVisible())
            {
                slotToolTip.HideToolTip();
            }
        }
    }   

    public void showToolTip(Item _item, Vector3 _pos)
    {
        slotToolTip.ShowToolTip(_item, _pos);
    }

    public void HideToolTip()
    {
        slotToolTip.HideToolTip();
    }
}
