using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetSelector : MonoBehaviour
{
    public static PlanetManager.PlanetType SelectedPlanet { get; private set; }

    [SerializeField]
    private GameObject mapPanel;
    public void SelectPlanet(string planetName) // 해당 함수를 이용해서 씬 이동
    {
        // enum 값으로 변환
        if (System.Enum.TryParse(planetName, out PlanetManager.PlanetType selectedPlanet))
        {
            PlanetManager.SetSelectedPlanet(selectedPlanet);
            StartGame(planetName); // StartGame 호출하여 씬 전환
        }
        else
        {
            Debug.LogWarning($"Invalid planet name: {planetName}");
        }
    }

    private void StartGame(string sceneName)
    {
        Inventory.inventoryActivated = false;
        UnityNote.SceneLoader.Instance.LoadScene(sceneName);
        mapPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InteractUIManger.isUseInteractObject = false;
    }
}
