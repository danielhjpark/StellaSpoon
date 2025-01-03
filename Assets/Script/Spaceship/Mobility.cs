using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mobility : MonoBehaviour
{
    [SerializeField]
    private GameObject mapUI; //���� UI

    private bool collPlayer = false; //�÷��̾� �浹üũ

    private void Update()
    {
        if (!mapUI.activeSelf && collPlayer && Input.GetKeyDown(KeyCode.F))
        {
            ToggleMapUI();
        }
        if (mapUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMapUI();
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
        if (other.CompareTag("Player"))
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
