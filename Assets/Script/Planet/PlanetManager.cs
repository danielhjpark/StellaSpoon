using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager Instance { get; private set; }
    public enum PlanetType
    {
        Shop,
        Restaurant,
        aRedForest, //1青己
        Serenoxia
        //货肺款 青己 眠啊
    }

    [SerializeField]
    private List<PlanetInfo> planetInfos; //青己 沥焊 历厘 府胶飘

    private Dictionary<PlanetType, PlanetInfo> planetInfoMap;
    public static PlanetType selectedPlanet; //泅犁 青己

    private void Awake()
    {
        Instance = this;

        planetInfoMap = new Dictionary<PlanetType, PlanetInfo>
        {
            { PlanetType.aRedForest, planetInfos[0] },
            { PlanetType.Serenoxia, planetInfos[1] },
            { PlanetType.Restaurant, planetInfos[2] },
            { PlanetType.Shop, planetInfos[3] }
            //货肺款 青己 眠啊
        };
    }

    public PlanetInfo GetPlanetInfo(PlanetType planet)
    {
        planetInfoMap.TryGetValue(planet, out var info);
        return info;
    }

    //青己 汲沥
    public static void SetSelectedPlanet(PlanetType planet)
    {
        selectedPlanet = planet;
    }

    public PlanetType GetSelectedPlanet()
    {
        return selectedPlanet;
    }
}
