using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CookSceneManager : MonoBehaviour
{
    static public CookSceneManager instance;
    public Camera mainCamera;
    private const string potSceneName = "PotMergeTest";
    private const string cuttingSceneName = "CuttingBoardMergeTest";
    private const string wokSceneName = "WokMergeTest";
    private const string panSceneName = "FryingPanMergeTest";

    public bool isSceneLoaded = false; // 씬 로드 상태 체크

    public string currentSceneName;
    public GameObject[] SpawnPoint;

    void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (SpawnPoint[0].transform.childCount > 0)
            CookManager.instance.isCanUseSideTable = false;
        else 
            CookManager.instance.isCanUseSideTable = true;
        
        if (SpawnPoint[1].transform.childCount > 0)
            CookManager.instance.isCanUseMiddleTable = false;
        else
            CookManager.instance.isCanUseMiddleTable = true;
    }

    public void LoadScene(string objName)
    {
        if (!isSceneLoaded)
        {
            switch (objName)
            {
                case "CuttingBoard":
                    currentSceneName = cuttingSceneName;
                    break;
                case "Pot":
                    currentSceneName = potSceneName;
                    break;
                case "Pan":
                    currentSceneName = panSceneName;
                    break;
                case "Wok":
                    currentSceneName = wokSceneName;
                    break;
                default:
                    break;
            }
            mainCamera.transform.gameObject.SetActive(false);
            SceneManager.LoadScene(currentSceneName, LoadSceneMode.Additive);
            isSceneLoaded = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    public void SpawnMenu(string sceneName, Recipe menu)
    {
        GameObject meneInstance = Instantiate(menu.menuPrefab, Vector3.zero, Quaternion.identity);
        meneInstance.AddComponent<MenuData>();
        meneInstance.GetComponent<MenuData>().menu = menu;
        switch (sceneName)
        {
            case cuttingSceneName:
                meneInstance.transform.SetParent(SpawnPoint[3].transform);
                meneInstance.transform.position = SpawnPoint[3].transform.position;
                break;
            case potSceneName:
                meneInstance.transform.SetParent(SpawnPoint[0].transform);
                meneInstance.transform.position = SpawnPoint[0].transform.position;
                break;
            case panSceneName:
                meneInstance.transform.SetParent(SpawnPoint[1].transform);
                meneInstance.transform.position = SpawnPoint[1].transform.position;
                break;
            case wokSceneName:
                meneInstance.transform.SetParent(SpawnPoint[2].transform);
                meneInstance.transform.position = SpawnPoint[2].transform.position;
                break;
            default:
                break;
        }
    }

    public void UnloadScene()
    {
        mainCamera.transform.gameObject.SetActive(true);
        if (isSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(currentSceneName);
            isSceneLoaded = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void UnloadScene(string sceneName, Recipe menu)
    {
        if (isSceneLoaded) { }
        if (menu == CookManager.instance.failMenu)
        {
            InteractUIManger.instance.UsingText(InteractUIManger.TextType.FailMenu);
        }
        mainCamera.transform.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync(sceneName);
        isSceneLoaded = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SpawnMenu(sceneName, menu);
    }

    public void UnloadScene(string sceneName)
    {
        mainCamera.transform.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync(sceneName);
        isSceneLoaded = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public bool IsSceneLoaded(string sceneName)
    {
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                return true;
            }
        }
        return false;
    }


}
