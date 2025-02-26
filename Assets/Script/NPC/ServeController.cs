using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServeController : MonoBehaviour
{
    [SerializeField] Transform playerTransfom;
    [SerializeField] Transform playerHand;
    [SerializeField] private float range = 2.0f; 
    [SerializeField] private TextMeshProUGUI actionText; 
    [SerializeField] private LayerMask NPCLayerMask;

    public Recipe currentMenu;
    private GameObject serveObject;
    bool isCanServeMenu;

    void Start()
    {
        isCanServeMenu = false;
        //PickUpMenu(currentMenu);
    }

    public void PickUpMenu(Recipe menu) {
        isCanServeMenu = true;
        currentMenu = menu;
        serveObject = Instantiate(currentMenu.menuPrefab, playerHand.position, Quaternion.identity);
        serveObject.transform.SetParent(playerHand);
    }

    // Update is called once per frame
    void Update()
    {
        if(isCanServeMenu) {
            CheckNPC();
        }
    }

    void ServeAnimation() {

    }

    /// -------------------------------------------------------//
    private void CheckNPC() {
        Vector3 rayOrigin = playerTransfom.position + Vector3.up * 0.5f; // 캐릭터 중심에서 약간 위로
        Vector3 rayDirection = playerTransfom.forward; // 캐릭터의 forward 방향

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, range, NPCLayerMask))
        {
            if(hitInfo.transform.gameObject.TryGetComponent<NPCBehavior>(out NPCBehavior behavior)) {
                actionText.gameObject.SetActive(true);
                actionText.text = "Press F";
                bool isSameMenu = behavior.CheckOrderMenu(currentMenu);
                if(isSameMenu && Input.GetKeyDown(KeyCode.V)) {
                    behavior.ReceiveNpc();
                    Destroy(serveObject);
                }
                else if(!isSameMenu && Input.GetKeyDown(KeyCode.V)){
                    StartCoroutine(behavior.DifferentMenu());
                }
            }
        }
        else actionText.gameObject.SetActive(false);
    }

    private void ThrowAwayMenu() {

    }
}
