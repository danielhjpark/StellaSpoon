using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetSelector : MonoBehaviour
{
    public static PlanetManager.PlanetType SelectedPlanet { get; private set; }

    [SerializeField]
    private GameObject mapPanel;
    public void SelectPlanet(string planetName)
    {
        // enum 값으로 변환
        if (System.Enum.TryParse(planetName, out PlanetManager.PlanetType selectedPlanet))
        {
            PlanetManager.SetSelectedPlanet(selectedPlanet);

            mapPanel.SetActive(false);
            // 씬 전환
            SceneManager.LoadScene(planetName); // 전환할 씬 이름
        }
        else
        {
            Debug.LogWarning($"Invalid planet name: {planetName}");
        }
    }
}
