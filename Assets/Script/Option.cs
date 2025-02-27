using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public static bool OptionyActivated = false; //�ɼ� ǥ�� ����

    public GameObject go_Option; //�ɼ� ������Ʈ

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
