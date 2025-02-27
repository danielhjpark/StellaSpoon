using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public static bool OptionyActivated = false; //옵션 표출 여부

    public GameObject go_Option; //옵션 오브젝트

    public void OpenOption()
    {
        if (OptionyActivated) return;

        go_Option.SetActive(true);
        OptionyActivated = true;
    }

    public void CloseOption()
    {
        go_Option.SetActive(false);
        OptionyActivated = false;
    }
}
