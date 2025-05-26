using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager Instance { get; private set; }
    public enum PlanetType
    {
        Moon, //��
        Earth, //����
        Mars, //ȭ��
        aRedForest, //1�༺
        Shop,
        RestaurantTest2, //������� �׽�Ʈ
        Restaurant,
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
            { PlanetType.Earth, planetInfos[0] },
            { PlanetType.Moon, planetInfos[1] },
            { PlanetType.Mars, planetInfos[2] },
            {PlanetType.aRedForest, planetInfos[3] },
            { PlanetType.Shop, planetInfos[4] },
            { PlanetType.RestaurantTest2, planetInfos[5] }
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
