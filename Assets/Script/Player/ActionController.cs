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

    private RaycastHit hitInfo; // �浹ü ���� ����

    [SerializeField]
    private LayerMask layerMask; // Ư�� ���̾ ���� ������Ʈ�� ���ؼ��� ���� ����

    [SerializeField]
    private Text actionText; // �ൿ�� ���� �� �ؽ�Ʈ

    [SerializeField]
    private Transform characterTransform; // ĳ���� ���� Transform

    void Update()
    {
        CheckItem(); // �׻� �������� ���� �Ÿ� �ȿ� �ִ��� üũ
        TryAction(); // FŰ �Է��� ���Դ��� �˻�
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CheckItem()
    {
        // ĳ������ forward �������� ����ĳ��Ʈ �߻�
        Vector3 rayOrigin = characterTransform.position + Vector3.up * 0.1f; // ĳ���� �߽ɿ��� �ణ ����
        Vector3 rayDirection = characterTransform.forward; // ĳ������ forward ����

        // ����ĳ��Ʈ �߻�
        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
        }
        else
        {
            ItemInfoDisappear();
        }

        // ���� �ð�ȭ
        Debug.DrawRay(rayOrigin, rayDirection * range, Color.red);
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� " + "<color=yellow>(F)</color>";
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ���߽��ϴ�."); // �κ��丮�� �߰�
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }
}


