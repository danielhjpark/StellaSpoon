using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range = 5.0f; // 아이템 습득이 가능한 최대 거리

    private bool pickupActivated = false; // 아이템 습득 가능할시 true

    private RaycastHit hitInfo; // 충돌체 정보 저장

    [SerializeField]
    private LayerMask layerMask; // 특정 레이어를 가진 오브젝트에 대해서만 습득 가능

    [SerializeField]
    private Text actionText; // 행동을 보여 줄 텍스트

    [SerializeField]
    private Transform characterTransform; // 캐릭터 모델의 Transform

    void Update()
    {
        CheckItem(); // 항상 아이템이 사정 거리 안에 있는지 체크
        TryAction(); // F키 입력이 들어왔는지 검사
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
        // 캐릭터의 forward 방향으로 레이캐스트 발사
        Vector3 rayOrigin = characterTransform.position + Vector3.up * 0.1f; // 캐릭터 중심에서 약간 위로
        Vector3 rayDirection = characterTransform.forward; // 캐릭터의 forward 방향

        // 레이캐스트 발사
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

        // 레이 시각화
        Debug.DrawRay(rayOrigin, rayDirection * range, Color.red);
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>(F)</color>";
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
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다."); // 인벤토리에 추가
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }
}


