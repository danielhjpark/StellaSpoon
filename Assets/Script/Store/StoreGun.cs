using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreGun : MonoBehaviour
{
    public static bool BuyTempestFang = false; //�� ���� üũ ����
    public static bool BuyInfernoLance = false; //�� ���� üũ ����

    private int BuyCostTempestFang = 1500; //���佺Ʈ �� ���� ���
    private int BuyCostInfernoLance = 2700; //���丣�� �� ���� ���

    [SerializeField]
    private Button buyButton; //���� ��ư

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


    //���Կ� �ش� �������� �ְ�
    //�ش� �������� ������ �޼��Ǹ�
    //���۹�ư Ȱ��ȭ
    //���۹�ư Ŭ���� ������ ����
    //���� ���� �Ҹ�
    //���� ��� �Ҹ�

    //TempestFang�� �� 5���� �������� �ʿ��ϰ�
    //���� 3��, 5��, 5��, 10��, 7���� �ʿ��ϴ�.

    //InfernoLance�� �� 7���� �������� �ʿ��ϰ�
    //���� 5��, 3��, 7��, 3��, 5��, 10��, 10���� �ʿ��ϴ�.
}
