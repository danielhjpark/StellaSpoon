using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ServeController : MonoBehaviour
{
    [SerializeField] Transform playerTransfom;
    [SerializeField] Transform playerHand;
    [SerializeField] private float range = 2.0f; 
    [SerializeField] private TextMeshProUGUI actionText; 
    [SerializeField] private LayerMask NPCLayerMask;
    [SerializeField] private LayerMask GarbageCanLayerMask;

    public Recipe currentMenu;
    private GameObject serveObject;
    bool isCanServeMenu;

    void Start()
    {
        isCanServeMenu = false;
        //PickUpMenu(currentMenu);
    }

    public void PickUpMenu(GameObject menuObject) {
        Recipe menu = menuObject.GetComponent<MenuData>().menu;
        if(menu.menuName == "Garbage") {
        }
        isCanServeMenu = true;
        currentMenu = menu;
        serveObject = Instantiate(currentMenu.menuPrefab, playerHand.position, Quaternion.identity);
        serveObject.transform.SetParent(playerHand);
    }

    // Update is called once per frame
    void Update()
    {
        CheckNPC();
    }

    void ServeAnimation() {

    }

    /// -------------NPC & Garbage can--------------------------//
    private void CheckNPC() {
        if(serveObject == null) {
            isCanServeMenu = false;
            actionText.gameObject.SetActive(false);
            return;
        }
        else if(!isCanServeMenu) {
            actionText.gameObject.SetActive(false);
            return;
        }

        Vector3 rayOrigin = playerTransfom.position + Vector3.up * 0.5f; // 캐릭터 중심에서 약간 위로
        Vector3 rayDirection = playerTransfom.forward; // 캐릭터의 forward 방향
        RaycastHit hitInfo = new RaycastHit();

        if(Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, GarbageCanLayerMask)) {
            actionText.gameObject.SetActive(true);
            actionText.text = "Throw out Press F";
            if(Input.GetKeyDown(KeyCode.F)) {
                Destroy(serveObject);
                //Sound 추가 필요 쓰레기통에 넣는 소리
            }
        }
        else if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, NPCLayerMask))
        {
            if(hitInfo.transform.gameObject.TryGetComponent<NPCBehavior>(out NPCBehavior behavior)) {
                actionText.gameObject.SetActive(true);
                actionText.text = "Press F";
                if(Input.GetKeyDown(KeyCode.F)) {
                    behavior.ReceiveNPC(serveObject);
                }
            }
        }
        else actionText.gameObject.SetActive(false);
    }

}
