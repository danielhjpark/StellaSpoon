using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneController : MonoBehaviour
{

    [SerializeField]
    private GameObject lobbyUI;

    public void GameStartEvent()
    {
        UnityNote.SceneLoader.Instance.LoadScene(SceneNames.RestaurantTest);
        lobbyUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GameExitEvent()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
