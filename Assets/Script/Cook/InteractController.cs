using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractController : MonoBehaviour
{
    private ServeSystem serveSystem;
    [SerializeField] private RestaurantOpenSystem restaurantOpenSystem;

    private Transform playerTransfom;

    [SerializeField] private CinemachineVirtualCamera playerFollowCamera;

    [Header("Interact Layer")]
    [SerializeField] private LayerMask utensilLayer;
    [SerializeField] private LayerMask menuLayer;
    [SerializeField] private LayerMask NPCLayerMask;

    [Header("UI Object")]
    [SerializeField] private InteractUI interactUI;

    private bool isCanInteract;
    private float range = 1f;

    void Start()
    {
        serveSystem = GetComponent<ServeSystem>();
        isCanInteract = true;
        playerTransfom = GameObject.FindGameObjectWithTag("Player").transform;
        this.transform.SetParent(playerTransfom);
        playerFollowCamera = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    private void OnSceneUnloaded(Scene current)
    {
        if (current.name == "Restaurant" || current.name == "RestaurantTest2")
            Destroy(gameObject);
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
            interactUI.DisableInteractUI();
            return brain.ActiveVirtualCamera.VirtualCameraGameObject == playerFollowCamera.gameObject;
        }

        return false;

    }

    private void CheckLayer()
    {
        Vector3 rayOrigin = playerTransfom.position + Vector3.up; // 캐릭터 중심에서 약간 위로
        Vector3 rayDirection = playerTransfom.forward; // 캐릭터의 forward 방향
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, utensilLayer))
        {
            interactUI.UseInteractUI(hitInfo.transform.gameObject);
            if (Input.GetKeyDown(KeyCode.F))
            {
                CookManager.instance.InteractObject(hitInfo.transform.name);
            }
        }
        else if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, menuLayer))
        {
            interactUI.UseInteractUI(hitInfo.transform.gameObject);
            if (Input.GetKeyDown(KeyCode.F))
            {
                serveSystem.PickUpMenu(hitInfo.transform.gameObject);
            }

        }
        else if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, range, NPCLayerMask))
        {
            interactUI.UseInteractUI(hitInfo.transform.gameObject);
            if (Input.GetKeyDown(KeyCode.F))
            {
                serveSystem.ServeMenu(hitInfo.transform.gameObject);
            }
        }
        else
        {
            interactUI.DisableInteractUI();
        }

    }


}
