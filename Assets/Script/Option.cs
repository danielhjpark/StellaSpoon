using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public static bool OptionActivated = false; //옵션 표출 여부

    public GameObject go_Option; //옵션 오브젝트

    public void OpenOption()
    {
        if (OptionActivated) return;

        go_Option.SetActive(true);
        OptionActivated = true;
    }

    public void CloseOption()
    {
        go_Option.SetActive(false);
        OptionActivated = false;
    }
}
