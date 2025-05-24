using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetSelector : MonoBehaviour
{
    public static PlanetManager.PlanetType SelectedPlanet { get; private set; }

    [SerializeField]
    private GameObject mapPanel;
    public void SelectPlanet(string planetName) // �ش� �Լ��� �̿��ؼ� �� �̵�
    {
        // enum ������ ��ȯ
        if (System.Enum.TryParse(planetName, out PlanetManager.PlanetType selectedPlanet))
        {
            PlanetManager.SetSelectedPlanet(selectedPlanet);
            StartGame(planetName); // StartGame ȣ���Ͽ� �� ��ȯ
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
