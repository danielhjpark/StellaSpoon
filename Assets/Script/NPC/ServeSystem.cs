using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEditor;

public class ServeSystem : MonoBehaviour
{
    [SerializeField] private Transform playerHand;
    [NonSerialized] public Recipe currentMenu;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioClip putDishAudio;
    [SerializeField] private AudioClip trashAudio;
    [SerializeField] private WaitingTableSystem waitingTableSystem;
    private string pickupAnimationName = "BringDish";
    private GameObject serveObject;
    public void Start()
    {
        Initialize();
    }

    private bool EndingCheck(GameObject menuObject)
    {
        MenuData menuData = menuObject.GetComponent<MenuData>();
        if (menuData.menu == RecipeManager.instance.HiddenRecipe) return true;
        return false;
    }

    public void Initialize()
    {
        this.playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        //this.playerAnimator = playerAnimator;
    }

    public void PickUpMenu(GameObject menuObject)
    {
        if(EndingCheck(menuObject)) {
            EndingUI endingUI = FindObjectOfType<EndingUI>();
            endingUI.StartEndingScene();
            return;
        }
        //PickUp Animate
        playerAnimator.SetBool(pickupAnimationName, true);

        //PickUp lock
        //Destroy(menuObject.GetComponent<Collider>());
        menuObject.GetComponent<Collider>().enabled = false;

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
        //case 1 use table
        MenuData menuData = menuObject.GetComponent<MenuData>();
        if (menuData.useTable)
        {
            waitingTableSystem.CheckUseTable();
            return;
        }
        CookType cooktype = menuData.menu.cookType;
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
            AudioSource.PlayClipAtPoint(trashAudio, playerHand.position);
            playerAnimator.SetBool(pickupAnimationName, false);
            Destroy(serveObject);
            CookManager.instance.isPickUpMenu = false;
        }
    }

    public void ServeMenu(GameObject hitInfo)
    {
        if (serveObject != null)
        {
            if (hitInfo.transform.gameObject.TryGetComponent<NPCBehavior>(out NPCBehavior behavior))
            {
                if (!behavior.IsCanReceivedMenu()) return;
                if (!behavior.ReceiveNPC(serveObject)) return;
                AudioSource.PlayClipAtPoint(putDishAudio, playerHand.position);
                playerAnimator.SetBool(pickupAnimationName, false);
                CookManager.instance.isPickUpMenu = false;
            }
            else if (hitInfo.transform.gameObject.TryGetComponent<WaitingTableSystem>(out WaitingTableSystem tableSystem))
            {
                if (!tableSystem.IsCanUseTable()) return;
                tableSystem.UseWaitingTable(serveObject);
                playerAnimator.SetBool(pickupAnimationName, false);
                CookManager.instance.isPickUpMenu = false;
                serveObject.GetComponent<Collider>().enabled = true;
                serveObject.GetComponent<MenuData>().useTable = true;
                serveObject = null;
            }
        }

    }


}
