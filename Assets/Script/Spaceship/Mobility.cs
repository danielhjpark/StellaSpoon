using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mobility : MonoBehaviour
{
    [SerializeField]
    private GameObject mapUI; //���� UI

    private bool collPlayer = false; //�÷��̾� �浹üũ
    private bool openMap = false; //�� Ȱ��ȭ üũ

    private void Update()
    {
        if(collPlayer)
        {
            if (!openMap && Input.GetKeyDown(KeyCode.F))
            {
                ToggleMapUI();
                openMap = true;
            }
            if (openMap && Input.GetKeyDown(KeyCode.Escape))
            {
                CloseMapUI();
                openMap = false;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            collPlayer = false;
        }
    }

    private void ToggleMapUI()
    {
        mapUI.SetActive(!mapUI.activeSelf);
    }

    private void CloseMapUI()
    {
        mapUI.SetActive(false);
    }

    public void OnClick(string planetSceneName)
    {
        SceneManager.LoadScene(planetSceneName);
    }
}
