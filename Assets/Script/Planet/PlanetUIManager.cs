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
        Debug.Log("�� �ε��");
        UpdatePlanetUI();
    }

    public void UpdatePlanetUI()
    {
        // PlanetManager���� ���õ� �༺ ���� ��������
        var selectedPlanet = planetManager.GetSelectedPlanet();
        var planetInfo = planetManager.GetPlanetInfo(selectedPlanet);

        if (planetInfo != null)
        {
            Debug.Log("UI ������Ʈ");
            // UI ������Ʈ
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
