using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMove : MonoBehaviour
{
    bool isOpened;
    bool isPlayerNearby;

    void Start()
    {
        isPlayerNearby = false;
        isOpened = false;
    }

    private void Update()
    {
        if (isPlayerNearby && InteractUIManger.isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            OpenUI();
        }
        if (isOpened && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUI();
        }
    }
    private void OpenUI()
    {
        isOpened = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InteractUIManger.isUseInteractObject = true;
    }
    private void CloseUI()
    {
        isOpened = false;
        Cursor.lockState -= CursorLockMode.Locked;
        Cursor.visible = false;
        InteractUIManger.isUseInteractObject = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지");
            isPlayerNearby = true;
            InteractUIManger.isPlayerNearby = true;
            InteractUIManger.currentInteractObject = this.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayerNearby)
        {
            InteractUIManger.isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 나감");
            isPlayerNearby = false;
            InteractUIManger.isPlayerNearby = false;
        }
    }
}
