using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnSceneButton : MonoBehaviour
{
    private Button button;

    void OnEnable()
    {
        if (CookSceneManager.instance != null)
        {
            button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            //button.onClick.AddListener(UnloadScene);
            button.onClick.AddListener(CookSceneManager.instance.UnloadScene);
            int eventCount = button.onClick.GetPersistentEventCount();

        }
        else {
            Debug.Log("Error");
        }
    }

    //TEMP FUNC
    void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(CookSceneManager.instance.currentSceneName);
    }

}
