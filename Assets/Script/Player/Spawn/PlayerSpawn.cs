using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerSpawn : MonoBehaviour
{
    public static bool useSavedPosition = false;  // ����� ��ġ�� ����� ��� true

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
        string[] ignoreScenes = { "WokMergeTest", "FryingPanMergeTest", "PotMergeTest", "CuttingBoardMergeTest" };
        foreach (string s in ignoreScenes)
            if (scene.name == s) return;

        if (useSavedPosition == false)
        {
            StartCoroutine(SetPlayerPosition());
        }
        else if(useSavedPosition == true)
        {
            Debug.Log("����� ��ġ�� �̵� �����̹Ƿ� �⺻ ��ġ�� ���� ����");
        }
    }

    private IEnumerator SetPlayerPosition()
    {
        GameObject player = null;
        float timeout = 3f;
        float elapsed = 0f;

        while (player == null && elapsed < timeout)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (player)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller)
            {
                controller.enabled = false;
                yield return null;
                player.transform.position = transform.position;
                controller.enabled = true;
            }
            else
            {
                yield return null;
                player.transform.position = transform.position;
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            useSavedPosition = false;
        }
        else
        {
            Debug.LogWarning("SetPlayerPosition: �ð� �ʰ��� �÷��̾ ã�� ����.");
        }
    }
}