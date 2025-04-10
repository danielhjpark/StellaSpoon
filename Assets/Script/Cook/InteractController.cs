using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    private ServeSystem serveSystem;
    [SerializeField] private RestaurantOpenSystem restaurantOpenSystem;

    private Transform playerTransfom;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private CinemachineVirtualCamera playerFollowCamera;

    [Header("Interact Layer")]
    [SerializeField] private LayerMask utensilLayer;
    [SerializeField] private LayerMask menuLayer;
    [SerializeField] private LayerMask NPCLayerMask;
    [SerializeField] private LayerMask GarbageCanLayerMask;

    [Header("UI Object")]
    [SerializeField] private GameObject InteractPanel;

    private bool isCanInteract;
    private float range = 1f;

    void Start()
    {
        serveSystem = GetComponent<ServeSystem>();
        isCanInteract = true;
        InteractPanel.SetActive(false);

        playerTransfom = GameObject.FindGameObjectWithTag("Player").transform;
        this.transform.SetParent(playerTransfom);
        playerFollowCamera = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsCheckInteract()) return;
        CheckLayer();
    }

    bool IsCheckInteract()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();

        if (brain != null && brain.ActiveVirtualCamera != null)
        {
            actionText.gameObject.SetActive(false);
            //InteractPanel.SetActive(false);
            return brain.ActiveVirtualCamera.VirtualCameraGameObject == playerFollowCamera.gameObject;
        }

        return false;

    }

    private void CheckLayer()
    {
        Vector3 rayOrigin = playerTransfom.position + Vector3.up * 0.5f; // 캐릭터 중심에서 약간 위로
        Vector3 rayDirection = playerTransfom.forward; // 캐릭터의 forward 방향
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, utensilLayer))
        {
            ChangeActionText("Utensil");
            actionText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                CookManager.instance.InteractObject(hitInfo.transform.name);
            }
        }
        else if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, menuLayer))
        {
            ChangeActionText("Menu");
            actionText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                serveSystem.PickUpMenu(hitInfo.transform.gameObject);
            }

        }
        else if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, GarbageCanLayerMask))
        {
            ChangeActionText("GarbageCan");
            actionText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                serveSystem.ThrowOutMenu();
                //Sound 추가 필요 쓰레기통에 넣는 소리
            }
        }
        else if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, NPCLayerMask))
        {
            ChangeActionText("NPC");
            actionText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                serveSystem.ServeMenu(hitInfo.transform.gameObject);
            }
        }
        else
        {
            actionText.gameObject.SetActive(false);
        }

    }

    LayerMask interactLayer;
    private void CheckLayerTest()
    {
        Vector3 rayOrigin = playerTransfom.position + Vector3.up * 0.5f; // 캐릭터 중심에서 약간 위로
        Vector3 rayDirection = playerTransfom.forward; // 캐릭터의 forward 방향
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, interactLayer))
        {
            string objectName = hitInfo.transform.gameObject.name;
            switch (objectName)
            {
                case "Menu":
                    ChangeActionText("Menu");
                    actionText.gameObject.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        serveSystem.PickUpMenu(hitInfo.transform.gameObject);
                        Destroy(hitInfo.transform.gameObject);
                    }
                    break;
            }
        }

    }

    private void ChangeActionText(string textType)
    {
        string defaultText = "Press F";
        string utensilText = "Use Utensil Press F";
        string menuText = "Pick Up Menu Press F";
        string garbageCanText = "Throw out Press F";
        string npcText = "Serve To Menu Press F";
        switch (textType)
        {
            case "Menu":
                actionText.text = menuText;
                break;
            case "Utensil":
                actionText.text = utensilText;
                break;
            case "GarbageCan":
                actionText.text = garbageCanText;
                break;
            case "NPC":
                actionText.text = npcText;
                break;
            default:
                actionText.text = defaultText;
                break;
        }
    }

}
