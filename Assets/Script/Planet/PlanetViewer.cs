using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetViewer : MonoBehaviour
{
    public PlanetManager planetManager;

    private void Start()
    {
        var earthInfo = planetManager.GetPlanetInfo(PlanetManager.PlanetType.Restaurant);
        if (earthInfo != null)
        {
            Debug.Log($"Planet: {earthInfo.planetName}, Description: {earthInfo.description}");
        }
    }
}
