using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponChanger : MonoBehaviour
{

    [SerializeField]
    private GameObject weaponChanger;
    private InteractUI interactUI;
    private bool collPlayer = false;
    public static bool isDeactived = true;

    private void OnEnable()
    {
        // ���� �ε�� �� �ݹ� �Լ� ����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // �� �ε� �� �ݹ� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Canvas�� �ڽ� ������Ʈ �� WeaponChanger ã��
        Transform canvasTransform = GameObject.Find("Canvas")?.transform; // Canvas�� ã��
        weaponChanger = canvasTransform.Find("WeaponChanger")?.gameObject; // MapPanel�� ã��
        interactUI = canvasTransform.Find("InteractPanel")?.GetComponent<InteractUI>();
        if (weaponChanger != null)
        {
            Debug.Log("MapUI ã�� ����!");
        }
        else
        {
            Debug.LogError("MapUI�� ã�� �� �����ϴ�.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!weaponChanger.activeSelf && collPlayer && Input.GetKeyDown(KeyCode.F))
        {
            ToggleMapUI();
        }
        if (weaponChanger.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMapUI();
        }
        if (!weaponChanger.activeSelf && collPlayer && DeviceManager.isDeactived) interactUI.UseInteractUI(this.gameObject, Vector2.up * 0.5f);
        else interactUI.DisableInteractUI(this.gameObject);
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
        weaponChanger.SetActive(!weaponChanger.activeSelf);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isDeactived = false;
    }

    private void CloseMapUI()
    {
        weaponChanger.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isDeactived = true;
    }
}
