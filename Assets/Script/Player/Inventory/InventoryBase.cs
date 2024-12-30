using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �κ��丮�� ���̽��� �κ��丮 ���Ե��� ��Ͻ�Ű�� ����� �غ� �ϴ� ��ũ��Ʈ
// �߻� Ŭ������ �ۼ��Ͽ� �κ��丮 ���̽� ��ü������ �ν��Ͻ� �Ұ��ϰ� ��.
abstract public class InventoryBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject mInventoryBase; // Inventory �ֻ��� �θ�(Ȱ�� / ��Ȱ�� ����)
    [SerializeField]
    protected GameObject mInventorySlotsParent; // Slot���� ���� �θ� ���ӿ�����Ʈ

    protected InventorySlot[] mSlots; // �κ��丮 ���� �迭
    // �κ��丮 ���̽��� �ʱ�ȭ
    protected void Awake()
    {
        if(mInventoryBase.activeSelf)
        {
            mInventoryBase.SetActive(false);
        }

        mSlots = mInventorySlotsParent.GetComponentsInChildren<InventorySlot>();
    }
}
