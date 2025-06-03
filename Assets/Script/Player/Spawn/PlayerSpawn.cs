using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerSpawn : MonoBehaviour
{
    public static bool useSavedPosition = false;  // 저장된 위치를 사용할 경우 true

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

        if (!useSavedPosition)
        {
            StartCoroutine(SetPlayerPosition());
        }
        else
        {
            Debug.Log("저장된 위치로 이동 예정이므로 기본 위치로 스폰 생략");
        }
    }

    private IEnumerator SetPlayerPosition()
    {
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            //CharacterController controller = player.GetComponent<CharacterController>();
            //if (controller)
            //{
            //    controller.enabled = false;
            //    player.transform.position = transform.position;
            //    controller.enabled = true;
            //}
            //else
            //{
            //    player.transform.position = transform.position;
            //}
            player.transform.position = transform.position;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}