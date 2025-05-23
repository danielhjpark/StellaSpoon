using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetUIManager : MonoBehaviour
{
    public static PlanetUIManager instance;
    public PlanetManager planetManager;

    public TextMeshProUGUI planetInformationText;
    public TextMeshProUGUI planetNameText;

    private void Awake()
    {
        planetManager = GameObject.Find("GameManager/PlanetManager").GetComponent<PlanetManager>();
        if (planetInformationText == null)
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
        planetNameText.text = planetInfo.planetName;
        planetInformationText.text = $"Description: {planetInfo.description}\n"
            + $"Weather: {planetInfo.weather} \n"
            + $"Gravity: {planetInfo.gravity} \n"
            + $"Monsters: {string.Join(", ", planetInfo.monsters)}";
    }
}
