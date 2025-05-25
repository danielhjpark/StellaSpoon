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
        aRedForest, //1행성
        Shop,
        RestaurantTest2, //레스토랑 테스트
        Restaurant,
        Serenoxia
        //새로운 행성 추가
    }

    [SerializeField]
    private List<PlanetInfo> planetInfos; //행성 정보 저장 리스트

    private Dictionary<PlanetType, PlanetInfo> planetInfoMap;
    public static PlanetType selectedPlanet; //현재 행성

    private void Awake()
    {
        Instance = this;

        planetInfoMap = new Dictionary<PlanetType, PlanetInfo>
        {
            { PlanetType.Earth, planetInfos[0] },
            { PlanetType.Moon, planetInfos[1] },
            { PlanetType.Mars, planetInfos[2] },
            {PlanetType.aRedForest, planetInfos[3] },
            { PlanetType.Shop, planetInfos[4] },
            { PlanetType.RestaurantTest2, planetInfos[5] }
            //새로운 행성 추가
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
