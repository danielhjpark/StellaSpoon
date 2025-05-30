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
        // ���� �� ��ȯ �� �ٷ� �ݿ� �� �� ���
        DynamicGI.UpdateEnvironment();
    }
}
