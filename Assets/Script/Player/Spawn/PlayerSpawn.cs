using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            player.transform.position = transform.position; // 씬 이동 후 스폰 포인트로 이동
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

