using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGroup : MonoBehaviour
{
    public static PlayerGroup instance;
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            Destroy(gameObject);
            return;
        }

        // ΩÃ±€≈Ê ∑Œ¡˜
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
        // æ¿¿Ã "Lobby"∑Œ ∫Ø∞Êµ«∏È ªË¡¶
        if (scene.name == "Lobby")
        {
            Destroy(gameObject);
        }
    }
}

