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
        planetInformationText.text = $"유형: {planetInfo.type}\n"
            + $"위치: {planetInfo.location} \n"
            + $"설명: {planetInfo.description} \n\n"
            + $"<size=40>서식 생물\n</size>-{string.Join("\n- ", planetInfo.monsters)}\n"
            + $"자생 식물{string.Join("\n-",planetInfo.plant)}";
    }
}
