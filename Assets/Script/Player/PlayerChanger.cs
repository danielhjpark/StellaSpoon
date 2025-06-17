using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 변경 감지를 위해 추가

public class PlayerChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject normalCharacter;
    [SerializeField]
    private GameObject specialCharacter;

    private string specialSceneName = "Restaurant"; // 아바타가 변경될 씬 이름

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 변경 이벤트 등록
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 해제
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == specialSceneName)
        {
            normalCharacter.SetActive(false);
            specialCharacter.SetActive(true);
        }
        else
        {
            normalCharacter.SetActive(true);
            specialCharacter.SetActive(false);
        }
    }
}
