using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlanetManager;

public class SkyBoxSetting : MonoBehaviour
{
    [SerializeField]
    private Material skyboxMaterial;

    void Start()
    {
        PlanetManager.SetSelectedPlanet(PlanetType.Restaurant);
        RenderSettings.skybox = skyboxMaterial;
        // 만약 씬 전환 후 바로 반영 안 될 경우
        DynamicGI.UpdateEnvironment();
    }
}
