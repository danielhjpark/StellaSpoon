using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
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
        StartCoroutine(SetPlayerPosition());
    }

    private IEnumerator SetPlayerPosition()
    {
        yield return null; // 한 프레임 대기

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller)
            {
                controller.enabled = false; // 충돌 방지
                player.transform.position = transform.position;
                controller.enabled = true;
            }
            else
            {
                player.transform.position = transform.position;
            }
        }
    }
}


