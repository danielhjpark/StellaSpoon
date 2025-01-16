using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetUIManager : MonoBehaviour
{
    public static PlanetUIManager instance;
    public PlanetManager planetManager;

    public TextMeshProUGUI planetNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI weatherText;
    public TextMeshProUGUI gravityText;
    public TextMeshProUGUI monstersText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정

        // Text 요소들을 씬에서 찾습니다.
        planetNameText = GameObject.Find("Canvas/PARENT_PlanetBase(DeactivateThis)/PlanetBase/PlanetNameText").GetComponent<TextMeshProUGUI>();
        descriptionText = GameObject.Find("Canvas/PARENT_PlanetBase(DeactivateThis)/PlanetBase/DescriptionText").GetComponent<TextMeshProUGUI>();
        weatherText = GameObject.Find("Canvas/PARENT_PlanetBase(DeactivateThis)/PlanetBase/WeatherText").GetComponent<TextMeshProUGUI>();
        gravityText = GameObject.Find("Canvas/PARENT_PlanetBase(DeactivateThis)/PlanetBase/GravityText").GetComponent<TextMeshProUGUI>();
        monstersText = GameObject.Find("Canvas/PARENT_PlanetBase(DeactivateThis)/PlanetBase/MonstersText").GetComponent<TextMeshProUGUI>();

        if (planetNameText == null || descriptionText == null || weatherText == null || gravityText == null || monstersText == null)
        {
            Debug.LogError("UI Text elements are not correctly assigned or found in the scene.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("씬 로드됨");
        UpdatePlanetUI();
    }

    public void UpdatePlanetUI()
    {
        // PlanetManager에서 선택된 행성 정보 가져오기
        var selectedPlanet = planetManager.GetSelectedPlanet();
        var planetInfo = planetManager.GetPlanetInfo(selectedPlanet);

        if (planetInfo != null)
        {
            Debug.Log("UI 업데이트");
            // UI 업데이트
            UpdateUI(planetInfo);
        }
        else
        {
            Debug.LogWarning("PlanetInfo is null. UI cannot be updated.");
        }
    }
    public void UpdateUI(PlanetInfo planetInfo)
    {
        if (planetInfo == null)
        {
            Debug.LogWarning("PlanetInfo is null. UI cannot be updated.");
            return;
        }

        planetNameText.text = $"Name: {planetInfo.planetName}";
        descriptionText.text = $"Description: {planetInfo.description}";
        weatherText.text = $"Weather: {planetInfo.weather}";
        gravityText.text = $"Gravity: {planetInfo.gravity}";
        monstersText.text = $"Monsters: {string.Join(", ", planetInfo.monsters)}";
    }
}
