using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldTimeManager : MonoBehaviour
{
    public GameTimeManager gameTimeManager;
    public Material[] skyboxes;
    private int skyIdx = 0;
    private float blendValue = 0.0f;

    [SerializeField]
    private int currentHour = 0;

    [SerializeField]
    private Light sunLight; // Sun Light


    private void Awake()
    {
        AssignSunLight();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignSunLight();
    }

    private void AssignSunLight()
    {
        var lightObj = GameObject.Find("Directional Light");
        if (lightObj != null)
            sunLight = lightObj.GetComponent<Light>();
        else
            sunLight = null;
    }
    private void Update()
    {
        if(PlanetManager.Instance.GetSelectedPlanet() == PlanetManager.PlanetType.RestaurantTest2 ||
           PlanetManager.Instance.GetSelectedPlanet() == PlanetManager.PlanetType.Shop) return; //레스토랑, 상점 씬에서는 스카이 박스 업데이트 안함
        float gameHour = gameTimeManager.gameHours + (gameTimeManager.gameMinutes / 60.0f);
        SunUpdate(gameHour);
    }

    void SunUpdate(float gameHour)
    {
        //Debug.Log("들어옴");
        if (currentHour != (int)gameHour)
        {
            currentHour = (int)gameHour;
            switch (currentHour)
            {
                case 0:
                    SetBlendSkyTexture(skyboxes[0]);
                    skyIdx = 3;
                    break;
                case 6:
                    SetBlendSkyTexture(skyboxes[1]);
                    skyIdx = 0;
                    break;
                case 12:
                    SetBlendSkyTexture(skyboxes[2]);
                    skyIdx = 1;
                    break;
                case 18:
                    SetBlendSkyTexture(skyboxes[3]);
                    skyIdx = 2;
                    break;
                
            }
        }

        blendValue = Mathf.Clamp01((gameHour % 6) / 6.0f);
        RenderSettings.skybox.SetFloat("_Blend", blendValue);

        // --- Sun Light 밝기/색상 조절 ---
        // 예시: 새벽/저녁은 약하게, 낮은 강하게, 밤은 거의 없음
        float intensity = 0f;
        Color sunColor = Color.white;

        if (gameHour >= 6f && gameHour < 18f)
        {
            // 낮: 1.0 ~ 1.2
            intensity = Mathf.Lerp(1.0f, 1.2f, Mathf.Sin((gameHour - 6f) / 12f * Mathf.PI));
            sunColor = Color.Lerp(new Color(1f, 0.95f, 0.8f), Color.white, (gameHour - 6f) / 12f);
        }
        else if (gameHour >= 5f && gameHour < 6f)
        {
            // 해 뜨기 전: 점점 밝아짐
            intensity = Mathf.Lerp(0.1f, 1.0f, gameHour - 5f);
            sunColor = Color.Lerp(new Color(1f, 0.7f, 0.4f), new Color(1f, 0.95f, 0.8f), gameHour - 5f);
        }
        else if (gameHour >= 18f && gameHour < 19f)
        {
            // 해 질 때: 점점 어두워짐
            intensity = Mathf.Lerp(1.0f, 0.1f, gameHour - 18f);
            sunColor = Color.Lerp(Color.white, new Color(1f, 0.7f, 0.4f), gameHour - 18f);
        }
        else
        {
            // 밤: 거의 없음
            intensity = 0.05f;
            sunColor = new Color(0.2f, 0.2f, 0.4f);
        }

        sunLight.intensity = intensity;
        sunLight.color = sunColor;
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
