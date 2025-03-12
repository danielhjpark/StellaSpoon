using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager Instance { get; private set; }
    public enum PlanetType
    {
        Moon, //달
        Earth, //지구
        Mars, //화성
        aRedForest // 민근이가 만든거
    }

    [SerializeField]
    private List<PlanetInfo> planetInfos; //행성 정보 저장 리스트

    private Dictionary<PlanetType, PlanetInfo> planetInfoMap;
    private static PlanetType selectedPlanet; //현재 행성

    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정

        planetInfoMap = new Dictionary<PlanetType, PlanetInfo>
        {
            { PlanetType.Earth, planetInfos[0] },
            { PlanetType.Moon, planetInfos[1] },
            { PlanetType.Mars, planetInfos[2] }
        };
    }

    public PlanetInfo GetPlanetInfo(PlanetType planet)
    {
        planetInfoMap.TryGetValue(planet, out var info);
        return info;
    }

    //행성 설정
    public static void SetSelectedPlanet(PlanetType planet)
    {
        selectedPlanet = planet;
    }

    public PlanetType GetSelectedPlanet()
    {
        return selectedPlanet;
    }
}
