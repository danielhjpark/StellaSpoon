using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUIManger : MonoBehaviour
{
    [SerializeField] GameObject interactUI;
    public static bool isUseInteractObject;
    public static bool isPlayerNearby;

    void Start()
    {
        isPlayerNearby = false;
        isUseInteractObject = false;
    }

    void Update() {
        if(isUseInteractObject) HideInteractUI();
        else if(!isPlayerNearby) HideInteractUI();
        else if(isPlayerNearby) VisibleInteractUI();
    }

    public void VisibleInteractUI()
    {
        interactUI.SetActive(true);
    }

    public void HideInteractUI()
    {
        interactUI.SetActive(false);
    }

}
