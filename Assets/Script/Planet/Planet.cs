using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public static bool planetActivated = false; //�ɼ� ǥ�� ����

    public GameObject go_Planet; //�ɼ� ������Ʈ

    public void OpenPlanet()
    {
        go_Planet.SetActive(true);
        planetActivated = true;
    }

    public void ClosePlanet()
    {
        go_Planet.SetActive(false);
        planetActivated = false;
    }
}
