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
        planetNameText.text = planetInfo.planetName;
        planetInformationText.text = $"Description: {planetInfo.description}\n"
            + $"Weather: {planetInfo.weather} \n"
            + $"Gravity: {planetInfo.gravity} \n"
            + $"Monsters: {string.Join(", ", planetInfo.monsters)}";
    }
}
