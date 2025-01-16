using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSceneLoader : MonoBehaviour
{
    public PlanetManager planetManager;
    public PlanetUIManager planetUIManager;

    private void Start()
    {
        // PlanetManager와 PlanetUIManager를 씬에서 찾기
        if (planetManager == null)
            planetManager = FindObjectOfType<PlanetManager>();

        if (planetUIManager == null)
            planetUIManager = FindObjectOfType<PlanetUIManager>();
        // 선택된 행성 가져오기
        var selectedPlanet = planetManager.GetSelectedPlanet();
        var planetInfo = planetManager.GetPlanetInfo(selectedPlanet);

        // UI 업데이트
        planetUIManager.UpdateUI(planetInfo);
    }
}
