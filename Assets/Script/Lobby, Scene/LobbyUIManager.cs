using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;

    private void Awake()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => UnityNote.SceneLoader.Instance.OnClick_NewGame());

        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(() => UnityNote.SceneLoader.Instance.OnClick_ContinueGame());
    }
}
