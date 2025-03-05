using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public static bool OptionActivated = false; //�ɼ� ǥ�� ����

    public GameObject go_Option; //�ɼ� ������Ʈ

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
