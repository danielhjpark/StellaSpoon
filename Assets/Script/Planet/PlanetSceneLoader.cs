using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSceneLoader : MonoBehaviour
{
    public PlanetManager planetManager;
    public PlanetUIManager planetUIManager;

    private void Start()
    {
        // PlanetManager�� PlanetUIManager�� ������ ã��
        if (planetManager == null)
            planetManager = FindObjectOfType<PlanetManager>();

        if (planetUIManager == null)
            planetUIManager = FindObjectOfType<PlanetUIManager>();
        // ���õ� �༺ ��������
        var selectedPlanet = planetManager.GetSelectedPlanet();
        var planetInfo = planetManager.GetPlanetInfo(selectedPlanet);

        // UI ������Ʈ
        planetUIManager.UpdateUI(planetInfo);
    }
}
