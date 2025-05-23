using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ���� ������ ���� �߰�

public class PlayerChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject normalCharacter;
    [SerializeField]
    private GameObject specialCharacter;

    private string specialSceneName = "Restaurant"; // �ƹ�Ÿ�� ����� �� �̸�

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // �� ���� �̺�Ʈ ���
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // �̺�Ʈ ����
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
