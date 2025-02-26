using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CookSceneManager : MonoBehaviour
{
    static public CookSceneManager instance;

    private const string potSceneName = "PotMergeTest";
    private const string cuttingSceneName = "CuttingBoardMergeTest";
    private const string wokSceneName = "WokMergeTest";
    private const string panSceneName = "FryingPanMergeTest";

    public bool isSceneLoaded = false; // 씬 로드 상태 체크
    public string currentSceneName;
    public GameObject[] SpawnPoint;

    void Awake() {
        instance = this;
    }

    public void LoadScene(string objName) {
        if (!isSceneLoaded)
        {
            switch(objName) {
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
            SceneManager.LoadScene(currentSceneName, LoadSceneMode.Additive);
            isSceneLoaded = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UnloadScene() {
        if (isSceneLoaded) {
            SceneManager.UnloadSceneAsync(currentSceneName);
            isSceneLoaded = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    public void UnloadScene(string sceneName, Recipe menu)
    {
        if (isSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            isSceneLoaded = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            switch(sceneName) {
                case cuttingSceneName:
                    Instantiate(menu.menuPrefab, SpawnPoint[3].transform.position, Quaternion.identity);
                    break;
                case potSceneName:
                    Instantiate(menu.menuPrefab, SpawnPoint[0].transform.position, Quaternion.identity);
                    break;
                case panSceneName:
                    Instantiate(menu.menuPrefab, SpawnPoint[1].transform.position, Quaternion.identity);
                    break;
                case wokSceneName:
                    Instantiate(menu.menuPrefab, SpawnPoint[2].transform.position, Quaternion.identity);
                    break;
                default:
                    break;
            }
        }
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
