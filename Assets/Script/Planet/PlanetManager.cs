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
        aRedForest, //1�༺
        Serenoxia
        //���ο� �༺ �߰�
    }

    [SerializeField]
    private List<PlanetInfo> planetInfos; //�༺ ���� ���� ����Ʈ

    private Dictionary<PlanetType, PlanetInfo> planetInfoMap;
    public static PlanetType selectedPlanet; //���� �༺

    private void Awake()
    {
        Instance = this;

        planetInfoMap = new Dictionary<PlanetType, PlanetInfo>
        {
            { PlanetType.aRedForest, planetInfos[0] },
            { PlanetType.Serenoxia, planetInfos[1] },
            { PlanetType.Restaurant, planetInfos[2] },
            { PlanetType.Shop, planetInfos[3] }
            //���ο� �༺ �߰�
        };
    }

    public PlanetInfo GetPlanetInfo(PlanetType planet)
    {
        planetInfoMap.TryGetValue(planet, out var info);
        return info;
    }

    //�༺ ����
    public static void SetSelectedPlanet(PlanetType planet)
    {
        selectedPlanet = planet;
    }

    public PlanetType GetSelectedPlanet()
    {
        return selectedPlanet;
    }
}
