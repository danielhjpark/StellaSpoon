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

    public static bool stage_01_clear = false; //스테이지 1 클리어 여부
    public static bool stage_02_clear = false; //스테이지 2 클리어 여부

    public static int gold = 300000; //플레이어 보유 골드

    [SerializeField]
    private GameObject GoldUI; //골드 UI 오브젝트

    [SerializeField]
    private TextMeshProUGUI InvenGoldText;

    private const string lastSavedTimeKey = "LastSavedTime"; // PlayerPrefs 키
    private const string KillCountKey = "KillTime"; // PlayerPrefs 키

    public static int KillMonsterCount = 0; //처치한 몬스터 수


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
        // 씬이 "Lobby"로 변경되면 삭제
        if (scene.name == "Lobby")
        {
            Destroy(gameObject);
        }

        //씬이 "레스토랑이나 상점으로 변경되면 골드 UI 활성화
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
        // 게임 종료 시 시간 저장
        SaveKillCount();
    }
    private void SaveKillCount()
    {
        // 현재 시간 기록
        PlayerPrefs.SetString(lastSavedTimeKey, DateTime.Now.ToString());
        PlayerPrefs.SetInt(KillCountKey, KillMonsterCount);
        PlayerPrefs.Save();
    }
    private void LoadGameKillCount()
    {
        // 마지막 저장된 시간 가져오기
        if (PlayerPrefs.HasKey(lastSavedTimeKey) && PlayerPrefs.HasKey(KillCountKey))
        {
            KillMonsterCount = PlayerPrefs.GetInt(KillCountKey);
        }
        else
        {
            // 저장된 데이터가 없으면 초기화
            KillMonsterCount = 0;
        }
    }
}

