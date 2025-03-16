using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class ServeSystem : MonoBehaviour
{
    [SerializeField] private Transform playerHand;
    [NonSerialized] public Recipe currentMenu;
    private GameObject serveObject;

    public void PickUpMenu(GameObject menuObject)
    {
        Recipe menu = menuObject.GetComponent<MenuData>().menu;
        currentMenu = menu;

        serveObject = Instantiate(currentMenu.menuPrefab, playerHand.position, Quaternion.identity);
        MenuData serveObjectData = serveObject.AddComponent<MenuData>();
        //serveObjectData.Initialize(currentMenu);
        serveObject.transform.SetParent(playerHand);
    }

    public void ThrowOutMenu()
    {
        if (serveObject != null) Destroy(serveObject);
    }

    public void ServeMenu(GameObject hitInfo)
    {
        if (hitInfo.transform.gameObject.TryGetComponent<NPCBehavior>(out NPCBehavior behavior))
        {
            behavior.ReceiveNPC(serveObject);
        }
    }


}
