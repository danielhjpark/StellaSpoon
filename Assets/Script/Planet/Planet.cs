using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public static bool planetActivated = false; //옵션 표출 여부

    public GameObject go_Planet; //옵션 오브젝트

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
