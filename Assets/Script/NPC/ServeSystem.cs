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

    [SerializeField] private Animator playerAnimator;
    private string pickupAnimationName = "BringDish";

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        this.playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        //this.playerAnimator = playerAnimator;
    }

    public void PickUpMenu(GameObject menuObject)
    {
        playerAnimator.SetBool(pickupAnimationName, true);

        Destroy(menuObject.GetComponent<Collider>());
        serveObject = menuObject;
        serveObject.transform.SetParent(playerHand);
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = Quaternion.identity;
        playerHand.localPosition = new Vector3(-0.05f, 1.123f, 0.3f);
        menuObject.transform.localPosition = new Vector3(0, 0, 0);
        menuObject.transform.localRotation = Quaternion.identity;
    }

    public void ThrowOutMenu()
    {
        if (serveObject != null)
        {
            playerAnimator.SetBool(pickupAnimationName, false);
            Destroy(serveObject);
        }
    }

    public void ServeMenu(GameObject hitInfo)
    {
        if (hitInfo.transform.gameObject.TryGetComponent<NPCBehavior>(out NPCBehavior behavior))
        {
            playerAnimator.SetBool(pickupAnimationName, false);
            behavior.ReceiveNPC(serveObject);
        }
    }


}
