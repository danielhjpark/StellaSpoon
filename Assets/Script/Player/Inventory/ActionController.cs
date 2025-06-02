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
    private bool TreeActivated = false; //트리 상호작용시 true

    private RaycastHit hitInfo; // 충돌체 정보 저장

    [SerializeField]
    private ObjectTree selectTree; // 충돌한 트리 저장

    [SerializeField]
    private LayerMask layerMask; // 특정 레이어를 가진 오브젝트에 대해서만 습득 가능

    [SerializeField]
    private Text actionText; // 행동을 보여 줄 텍스트

    [SerializeField]
    private Transform characterTransform; // 캐릭터 모델의 Transform

    [SerializeField]
    private Inventory theInventory; // 인벤토리 cs

    [SerializeField]
    private AudioClip Item_PickUp_SFX;

    [SerializeField]
    private GameObject go_InputWindow; // InputNumber 창의 go_Base 참조

    [SerializeField]
    private GameObject currentTarget = null; // 현재 바라보는 오브젝트
    void Update()
    {
        // InputNumber 창이 열려있으면 행동 검사 중단
        if (go_InputWindow != null && go_InputWindow.activeSelf) return;

        CheckItem(); // 항상 아이템이 사정 거리 안에 있는지 체크
        TryAction(); // F키 입력이 들어왔는지 검사
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //var itemPickUp = hitInfo.transform?.GetComponent<ItemPickUp>(); 최종 삭제 예정
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

            // 이전과 다른 오브젝트라면
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

            Debug.Log("Ray hit: " + hitInfo.transform.name); // 추가
            if (hitInfo.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
            else if (hitInfo.transform.CompareTag("ObjectTree"))
            {
                ObjectTree tree = hitInfo.transform.GetComponent<ObjectTree>(); // 충돌한 ObjectTree 인스턴스 가져오기
                if (tree != null && tree.canshake)
                    TreeObjectAppear();
            }
        }
        else
        {
            // 감지된 오브젝트가 없을 때 외곽선 제거
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
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 <color=white>(F)</color>";
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
        actionText.text = "나무 흔들기 <color=yellow>(F)</color>";
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
        if (TreeActivated && selectTree != null && selectTree.canshake) // 수정: ObjectTree.canshake를 selectTree.canshake로 변경
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
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다.");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                SoundManager.instance.PlaySound(SoundManager.Display.Item_Pick_Up);
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }

    //private void CheckRecipe()  최종 삭제 예정
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



