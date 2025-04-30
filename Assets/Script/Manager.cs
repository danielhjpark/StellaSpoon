using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public static bool stage_01_clear = false; //스테이지 1 클리어 여부
    public static bool stage_02_clear = false; //스테이지 2 클리어 여부

    public static int gold = 300000; //플레이어 보유 골드


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
    }
}

