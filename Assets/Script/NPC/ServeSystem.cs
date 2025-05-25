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
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioClip putDishAudio;

    private string pickupAnimationName = "BringDish";
    private GameObject serveObject;
    public void Start()
    {
        Initialize();
    }
    
    private void EndingCheck() { }

    public void Initialize()
    {
        this.playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        //this.playerAnimator = playerAnimator;
    }

    public void PickUpMenu(GameObject menuObject)
    {
        //PickUp Animate
        playerAnimator.SetBool(pickupAnimationName, true);

        //PickUp lock
        Destroy(menuObject.GetComponent<Collider>());

        //PickUp Menu transform position setting
        serveObject = menuObject;
        serveObject.transform.SetParent(playerHand);
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = Quaternion.identity;
        playerHand.localPosition = new Vector3(-0.05f, 1.123f, 0.3f);
        menuObject.transform.localPosition = new Vector3(0, 0, 0);
        menuObject.transform.localRotation = Quaternion.identity;

        //PickUp Check && Interact Setting 
        CookManager.instance.isPickUpMenu = true;
        CookType cooktype = menuObject.GetComponent<MenuData>().menu.cookType;
        if (cooktype == CookType.Frying || cooktype == CookType.Tossing) CookManager.instance.isCanUseMiddleTable = true;
        else if (cooktype == CookType.Tossing) CookManager.instance.isCanUseSideTable = true;
        else
        {
            CookManager.instance.isCanUseSideTable = true;
            CookManager.instance.isCanUseMiddleTable = true;
        }
    }

    public void ThrowOutMenu()
    {
        if (serveObject != null)
        {
            AudioSource.PlayClipAtPoint(putDishAudio, playerHand.position);
            playerAnimator.SetBool(pickupAnimationName, false);
            Destroy(serveObject);
            CookManager.instance.isPickUpMenu = false;
        }
    }

    public void ServeMenu(GameObject hitInfo)
    {
        if (hitInfo.transform.gameObject.TryGetComponent<NPCBehavior>(out NPCBehavior behavior))
        {
            if(!behavior.IsCanReceivedMenu()) return;
            AudioSource.PlayClipAtPoint(putDishAudio, playerHand.position);
            playerAnimator.SetBool(pickupAnimationName, false);
            behavior.ReceiveNPC(serveObject);
            CookManager.instance.isPickUpMenu = false;
        }
    }


}
