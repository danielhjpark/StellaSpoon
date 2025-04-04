using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chatUI;

    public void CallChatUI() //대화창 ON
    {
        chatUI.SetActive(true);
    }
    public void CloseChatUI() //대화창 OFF
    {
        chatUI.SetActive(false);
    }
}
