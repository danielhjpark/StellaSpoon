using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldTimeManager : MonoBehaviour
{
    public GameTimeManager gameTimeManager;

    private Material lerpSkybox;

    public Material aRedForestDaySkybox;
    public Material aRedForestNightSkybox;
    public Material SerenoxiaDaySkybox;
    public Material SerenoxiaNightSkybox;

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
        float currentHour = gameTimeManager.gameHours + gameTimeManager.gameMinutes / 60f;
        float t = 0f;

        // 현재 선택된 행성에 따라 스카이박스 선택
        Material daySkybox = null;
        Material nightSkybox = null;

        switch (PlanetManager.selectedPlanet)
        {
            case PlanetManager.PlanetType.aRedForest:
                daySkybox = aRedForestDaySkybox;
                nightSkybox = aRedForestNightSkybox;
                break;
            case PlanetManager.PlanetType.Serenoxia:
                daySkybox = SerenoxiaDaySkybox;
                nightSkybox = SerenoxiaNightSkybox;
                break;
            default:
                Debug.LogWarning("Unknown planet selected.");
                return;
        }

        // 1. 밤→낮 전환 (6~8시)
        if (currentHour >= 6f && currentHour < 8f)
        {
            t = (currentHour - 6f) / 2f;
            ApplyLerpSkybox(nightSkybox, daySkybox, t);
        }
        // 2. 낮→밤 전환 (18~20시)
        else if (currentHour >= 18f && currentHour < 20f)
        {
            t = 1f - (currentHour - 18f) / 2f;
            ApplyLerpSkybox(nightSkybox, daySkybox, t);
        }
        // 3. 완전 낮 (8~18시)
        else if (currentHour >= 8f && currentHour < 18f)
        {
            if (RenderSettings.skybox != daySkybox)
            { 
                RenderSettings.skybox = daySkybox;
            }
            lerpSkybox = null; // Lerp 머티리얼 해제
        }
        // 4. 완전 밤 (20~6시)
        else
        {
            if (RenderSettings.skybox != nightSkybox)
            {
                RenderSettings.skybox = nightSkybox;
            }
            lerpSkybox = null; // Lerp 머티리얼 해제
        }

        // --- Sun Light 밝기/색상 조절 (기존 코드 유지) ---
        float intensity = 0f;
        Color sunColor = Color.white;

        if (currentHour >= 6f && currentHour < 18f)
        {
            intensity = Mathf.Lerp(1.0f, 1.2f, Mathf.Sin((currentHour - 6f) / 12f * Mathf.PI));
            sunColor = Color.Lerp(new Color(1f, 0.95f, 0.8f), Color.white, (currentHour - 6f) / 12f);
        }
        else if (currentHour >= 5f && currentHour < 6f)
        {
            intensity = Mathf.Lerp(0.1f, 1.0f, currentHour - 5f);
            sunColor = Color.Lerp(new Color(1f, 0.7f, 0.4f), new Color(1f, 0.95f, 0.8f), currentHour - 5f);
        }
        else if (currentHour >= 18f && currentHour < 19f)
        {
            intensity = Mathf.Lerp(1.0f, 0.1f, currentHour - 18f);
            sunColor = Color.Lerp(Color.white, new Color(1f, 0.7f, 0.4f), currentHour - 18f);
        }
        else
        {
            intensity = 0.05f;
            sunColor = new Color(0.2f, 0.2f, 0.4f);
        }

        if (sunLight != null)
        {
            sunLight.intensity = intensity;
            sunLight.color = sunColor;
        }
    }

    // 전환 구간에서만 호출
    private void ApplyLerpSkybox(Material nightSkybox, Material daySkybox, float t)
    {
        if (lerpSkybox == null || lerpSkybox.shader != daySkybox.shader)
            lerpSkybox = new Material(daySkybox);

        // Skybox/Procedural 기준
        if (daySkybox.HasProperty("_SkyTint") && nightSkybox.HasProperty("_SkyTint"))
        {
            Color dayTint = daySkybox.GetColor("_SkyTint");
            Color nightTint = nightSkybox.GetColor("_SkyTint");
            lerpSkybox.SetColor("_SkyTint", Color.Lerp(nightTint, dayTint, t));
        }
        if (daySkybox.HasProperty("_GroundColor") && nightSkybox.HasProperty("_GroundColor"))
        {
            Color dayGround = daySkybox.GetColor("_GroundColor");
            Color nightGround = nightSkybox.GetColor("_GroundColor");
            lerpSkybox.SetColor("_GroundColor", Color.Lerp(nightGround, dayGround, t));
        }
        if (daySkybox.HasProperty("_Exposure") && nightSkybox.HasProperty("_Exposure"))
        {
            float dayExp = daySkybox.GetFloat("_Exposure");
            float nightExp = nightSkybox.GetFloat("_Exposure");
            lerpSkybox.SetFloat("_Exposure", Mathf.Lerp(nightExp, dayExp, t));
        }
        // 필요한 속성 추가로 Lerp

        RenderSettings.skybox = lerpSkybox;
    }
}
