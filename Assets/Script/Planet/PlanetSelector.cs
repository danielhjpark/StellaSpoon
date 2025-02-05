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
        // enum ������ ��ȯ
        if (System.Enum.TryParse(planetName, out PlanetManager.PlanetType selectedPlanet))
        {
            PlanetManager.SetSelectedPlanet(selectedPlanet);

            mapPanel.SetActive(false);
            // �� ��ȯ
            SceneManager.LoadScene(planetName); // ��ȯ�� �� �̸�
        }
        else
        {
            Debug.LogWarning($"Invalid planet name: {planetName}");
        }
    }
}
