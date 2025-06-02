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
        FindRespawnPoint(); // ���� �ٲ�� ���ο� RespawnPoint ã��
        FindBossPoint();
    }

    private void FindRespawnPoint()
    {
        GameObject respawnObj = GameObject.FindGameObjectWithTag("RespawnPoint");
        if (respawnObj != null)
        {
            ReSpawnPoint = respawnObj.transform;
            Debug.Log("���ο� RespawnPoint�� ã�ҽ��ϴ�: " + ReSpawnPoint.position);
        }
        else
        {
            Debug.LogWarning("RespawnPoint �±׸� ���� ������Ʈ�� �����ϴ�!");
            ReSpawnPoint = null; // ���� RespawnPoint�� ���� ��� null ó��
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

