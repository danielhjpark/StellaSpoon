using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public Transform ReSpawnPoint;

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
}

