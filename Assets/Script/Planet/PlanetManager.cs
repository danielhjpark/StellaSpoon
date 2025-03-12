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
        aRedForest // �α��̰� �����
    }

    [SerializeField]
    private List<PlanetInfo> planetInfos; //�༺ ���� ���� ����Ʈ

    private Dictionary<PlanetType, PlanetInfo> planetInfoMap;
    private static PlanetType selectedPlanet; //���� �༺

    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����

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
