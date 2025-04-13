using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestauantSkyBoxSetting : MonoBehaviour
{
    public Material skyboxMaterial;

    void Start()
    {
        RenderSettings.skybox = skyboxMaterial;
        // ���� �� ��ȯ �� �ٷ� �ݿ� �� �� ���
        DynamicGI.UpdateEnvironment();
    }
}
