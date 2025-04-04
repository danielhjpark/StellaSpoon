using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chatUI;

    public void CallChatUI() //��ȭâ ON
    {
        chatUI.SetActive(true);
    }
    public void CloseChatUI() //��ȭâ OFF
    {
        chatUI.SetActive(false);
    }
}
