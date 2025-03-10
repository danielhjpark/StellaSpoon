using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTimeManager : MonoBehaviour
{
    public GameTimeManager gameTimeManager;
    public Material[] skyboxes;
    private int skyIdx = 0;
    private float blendValue = 0.0f;

    [SerializeField]
    private int currentHour = 0;

    private void Update()
    {
        float gameHour = gameTimeManager.gameHours + (gameTimeManager.gameMinutes / 60.0f);
        SunUpdate(gameHour);
    }

    void SunUpdate(float gameHour)
    {
        if (currentHour != (int)gameHour)
        {
            currentHour = (int)gameHour;
            switch (currentHour)
            {
                case 6:
                    SetBlendSkyTexture(skyboxes[0]);
                    skyIdx = 0;
                    break;
                case 12:
                    SetBlendSkyTexture(skyboxes[1]);
                    skyIdx = 1;
                    break;
                case 18:
                    SetBlendSkyTexture(skyboxes[2]);
                    skyIdx = 2;
                    break;
                case 24:
                    SetBlendSkyTexture(skyboxes[3]);
                    skyIdx = 3;
                    break;
            }
        }

        blendValue = Mathf.Clamp01((gameHour % 6) / 6.0f);
        RenderSettings.skybox.SetFloat("_Blend", blendValue);
    }

    void SetBlendSkyTexture(Material skybox)
    {
        string[] texParts = { "_FrontTex", "_BackTex", "_LeftTex", "_RightTex", "_UpTex", "_DownTex" };

        foreach (string texPart in texParts)
        {
            RenderSettings.skybox.SetTexture(texPart, skyboxes[skyIdx].GetTexture(texPart));
            RenderSettings.skybox.SetTexture(texPart + "2", skybox.GetTexture(texPart));
        }
    }
}
