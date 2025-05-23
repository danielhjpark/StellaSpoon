using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public static bool stage_01_clear = false; //�������� 1 Ŭ���� ����
    public static bool stage_02_clear = false; //�������� 2 Ŭ���� ����

    public static int gold = 300000; //�÷��̾� ���� ���

    [SerializeField]
    private GameObject GoldUI; //��� UI ������Ʈ

    [SerializeField]
    private TextMeshProUGUI InvenGoldText;

    private const string lastSavedTimeKey = "LastSavedTime"; // PlayerPrefs Ű
    private const string KillCountKey = "KillTime"; // PlayerPrefs Ű

    public static int KillMonsterCount = 0; //óġ�� ���� ��


    private void Update()
    {
        InvenGoldText.text = gold.ToString();

        if (GoldUI.activeSelf)
        {
            GoldUI.GetComponent<TextMeshProUGUI>().text = gold.ToString();
        }
    }

    private void Start()
    {
        LoadGameKillCount();
    }
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            Destroy(gameObject);
            return;
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� "Lobby"�� ����Ǹ� ����
        if (scene.name == "Lobby")
        {
            Destroy(gameObject);
        }

        //���� "��������̳� �������� ����Ǹ� ��� UI Ȱ��ȭ
        if(PlanetManager.selectedPlanet == PlanetManager.PlanetType.RestaurantTest2 ||
           PlanetManager.selectedPlanet == PlanetManager.PlanetType.Shop)
        {
            GoldUI.SetActive(true);
        }
        else
        {
            GoldUI.SetActive(false);
        }
    }

    private void OnApplicationQuit()
    {
        // ���� ���� �� �ð� ����
        SaveKillCount();
    }
    private void SaveKillCount()
    {
        // ���� �ð� ���
        PlayerPrefs.SetString(lastSavedTimeKey, DateTime.Now.ToString());
        PlayerPrefs.SetInt(KillCountKey, KillMonsterCount);
        PlayerPrefs.Save();
    }
    private void LoadGameKillCount()
    {
        // ������ ����� �ð� ��������
        if (PlayerPrefs.HasKey(lastSavedTimeKey) && PlayerPrefs.HasKey(KillCountKey))
        {
            KillMonsterCount = PlayerPrefs.GetInt(KillCountKey);
        }
        else
        {
            // ����� �����Ͱ� ������ �ʱ�ȭ
            KillMonsterCount = 0;
        }
    }
}

