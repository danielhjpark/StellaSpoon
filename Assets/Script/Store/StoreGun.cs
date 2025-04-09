using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreGun : MonoBehaviour
{
    public static bool BuyTempestFang = false; //총 구매 체크 변수
    public static bool BuyInfernoLance = false; //총 구매 체크 변수

    private int BuyCostTempestFang = 1500; //템페스트 총 제작 비용
    private int BuyCostInfernoLance = 2700; //인페르노 총 제작 비용

    [SerializeField]
    private Button buyButton; //제작 버튼

    private Dictionary<string, int> TempestFangRequirements = new Dictionary<string, int>()
    {
        {"Barcrose_Tusks", 3},
        {"Red_Hornavia_Blade", 5},
        {"AdenBear_Leather", 5},
        {"Veridion", 10},
        {"Screw", 7 }
    };

    private Dictionary<string, int> InfernoLanceRequirements = new Dictionary<string, int>()
    {
        { "Red_Barcrose_Leather", 5 },
        { "Black_Fermos_Fur", 3 },
        { "Nova_Wolf_Claw", 7 },
        { "Bypin_Mucus", 3 },
        { "Inferium", 5 },
        { "Tractanium", 10 },
        { "Screw", 10 }
    };


    //슬롯에 해당 아이템을 넣고
    //해당 아이템의 갯수가 달성되면
    //제작버튼 활성화
    //제작버튼 클릭시 아이템 제작
    //제작 재료들 소모
    //제작 비용 소모

    //TempestFang는 총 5개의 아이템이 필요하고
    //각각 3개, 5개, 5개, 10개, 7개가 필요하다.

    //InfernoLance는 총 7개의 아이템이 필요하고
    //각각 5개, 3개, 7개, 3개, 5개, 10개, 10개가 필요하다.
}
