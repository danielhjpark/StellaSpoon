using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public Transform ReSpawnPoint;
    public Transform BossPoint;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindRespawnPoint(); // 씬이 바뀌면 새로운 RespawnPoint 찾기
        FindBossPoint();
    }

    private void FindRespawnPoint()
    {
        GameObject respawnObj = GameObject.FindGameObjectWithTag("RespawnPoint");
        if (respawnObj != null)
        {
            ReSpawnPoint = respawnObj.transform;
            Debug.Log("새로운 RespawnPoint를 찾았습니다: " + ReSpawnPoint.position);
        }
        else
        {
            Debug.LogWarning("RespawnPoint 태그를 가진 오브젝트가 없습니다!");
            ReSpawnPoint = null; // 씬에 RespawnPoint가 없을 경우 null 처리
        }
    }

    private void FindBossPoint()
    {
        GameObject bossObj = GameObject.FindGameObjectWithTag("BossPoint");
        if(bossObj != null)
        {
            BossPoint = bossObj.transform;
        }
        else
        {
            BossPoint = null;
        }
    }
}

