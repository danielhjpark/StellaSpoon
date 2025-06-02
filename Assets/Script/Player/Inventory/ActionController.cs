using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range = 5.0f; // ������ ������ ������ �ִ� �Ÿ�

    private bool pickupActivated = false; // ������ ���� �����ҽ� true
    private bool TreeActivated = false; //Ʈ�� ��ȣ�ۿ�� true

    private RaycastHit hitInfo; // �浹ü ���� ����

    [SerializeField]
    private ObjectTree selectTree; // �浹�� Ʈ�� ����

    [SerializeField]
    private LayerMask layerMask; // Ư�� ���̾ ���� ������Ʈ�� ���ؼ��� ���� ����

    [SerializeField]
    private Text actionText; // �ൿ�� ���� �� �ؽ�Ʈ

    [SerializeField]
    private Transform characterTransform; // ĳ���� ���� Transform

    [SerializeField]
    private Inventory theInventory; // �κ��丮 cs

    [SerializeField]
    private AudioClip Item_PickUp_SFX;

    [SerializeField]
    private GameObject go_InputWindow; // InputNumber â�� go_Base ����

    [SerializeField]
    private GameObject currentTarget = null; // ���� �ٶ󺸴� ������Ʈ
    void Update()
    {
        // InputNumber â�� ���������� �ൿ �˻� �ߴ�
        if (go_InputWindow != null && go_InputWindow.activeSelf) return;

        CheckItem(); // �׻� �������� ���� �Ÿ� �ȿ� �ִ��� üũ
        TryAction(); // FŰ �Է��� ���Դ��� �˻�
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //var itemPickUp = hitInfo.transform?.GetComponent<ItemPickUp>(); ���� ���� ����
            //if (itemPickUp != null && itemPickUp.item != null && itemPickUp.item.itemType == Item.ItemType.Recipe)
            //{
            //    CheckRecipe();
            //}

            CanPickUp();
            TryShakeTree();
        }
    }

    private void CheckItem()
    {
        Vector3 rayOrigin = characterTransform.position + Vector3.up * 0.3f;
        Vector3 rayDirection = characterTransform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, layerMask))
        {
            GameObject hitObject = hitInfo.transform.gameObject;

            // ������ �ٸ� ������Ʈ���
            if (hitObject != currentTarget)
            {
                if (currentTarget != null)
                {
                    var outline = currentTarget.GetComponent<OutlineEffect>();
                    if (outline != null) outline.DisableOutline();
                }

                currentTarget = hitObject;
                var newOutline = currentTarget.GetComponent<OutlineEffect>();
                if (newOutline != null) newOutline.EnableOutline();
            }

            Debug.Log("Ray hit: " + hitInfo.transform.name); // �߰�
            if (hitInfo.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
            else if (hitInfo.transform.CompareTag("ObjectTree"))
            {
                ObjectTree tree = hitInfo.transform.GetComponent<ObjectTree>(); // �浹�� ObjectTree �ν��Ͻ� ��������
                if (tree != null && tree.canshake)
                    TreeObjectAppear();
            }
        }
        else
        {
            // ������ ������Ʈ�� ���� �� �ܰ��� ����
            if (currentTarget != null)
            {
                var outline = currentTarget.GetComponent<OutlineEffect>();
                if (outline != null) outline.DisableOutline();
                currentTarget = null;
            }

            ItemInfoDisappear();
            TreeObjectDIsappear();
        }

        Debug.DrawRay(rayOrigin, rayDirection * range, Color.red);
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� <color=white>(F)</color>";
    }

    private void ItemInfoDisappear()
    {
        if (IsInputWindowActive()) return;
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void TreeObjectAppear()
    {
        TreeActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "���� ���� <color=yellow>(F)</color>";
        selectTree = hitInfo.transform.gameObject.GetComponent<ObjectTree>();
    }

    private void TreeObjectDIsappear()
    {
        if (IsInputWindowActive()) return;
        TreeActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void TryShakeTree()
    {
        if (TreeActivated && selectTree != null && selectTree.canshake) // ����: ObjectTree.canshake�� selectTree.canshake�� ����
        {
            selectTree.DropAliverry();
            selectTree = null;
            TreeObjectDIsappear();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (Slot.isFull) return;

            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ���߽��ϴ�.");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                SoundManager.instance.PlaySound(SoundManager.Display.Item_Pick_Up);
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }

    //private void CheckRecipe()  ���� ���� ����
    //{
    //    if (hitInfo.transform != null)
    //    {
    //        var itemPickUp = hitInfo.transform.GetComponent<ItemPickUp>();
    //        if (itemPickUp.item.itemType == Item.ItemType.Recipe)
    //        {
    //            Recipe unLockTarget = RecipeManager.instance.FindRecipe(itemPickUp.item.itemName);
    //            RecipeManager.instance.RecipeUnLock(unLockTarget);
    //            AudioSource.PlayClipAtPoint(Item_PickUp_SFX, characterTransform.position, 0.5f);
    //            Destroy(hitInfo.transform.gameObject);
    //        }
    //    }
    //}

    private bool IsInputWindowActive()
    {
        return go_InputWindow != null && go_InputWindow.activeSelf;
    }
}



