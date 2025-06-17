using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxSetting : MonoBehaviour
{
    [SerializeField]
    private Material skyboxMaterial;

    void Start()
    {
        RenderSettings.skybox = skyboxMaterial;
        // 만약 씬 전환 후 바로 반영 안 될 경우
        DynamicGI.UpdateEnvironment();
    }
}
