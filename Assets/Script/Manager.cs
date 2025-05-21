using System.Collections;
using System.Collections.Generic;
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
        if(PlanetManager.selectedPlanet == PlanetManager.PlanetType.Restaurant ||
           PlanetManager.selectedPlanet == PlanetManager.PlanetType.Store)
        {
            GoldUI.SetActive(true);
        }
        else
        {
            GoldUI.SetActive(false);
        }
    }
}

