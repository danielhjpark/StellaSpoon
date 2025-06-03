using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mobility : MonoBehaviour
{
    [SerializeField]
    private GameObject mapUI; //���� UI
    private InteractUI interactUI;
    private bool collPlayer = false; //�÷��̾� �浹üũ

    public PlanetManager planetManager;
    public PlanetUIManager planetUIManager;

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
        // Canvas�� �ڽ� ������Ʈ �� MapPanel�� ã��
        Transform canvasTransform = GameObject.Find("Canvas")?.transform; // Canvas�� ã��
        mapUI = canvasTransform.Find("MapPanel")?.gameObject; // MapPanel�� ã��
        interactUI = canvasTransform.Find("InteractPanel")?.GetComponent<InteractUI>();
        if (mapUI != null)
        {
            Debug.Log("MapUI ã�� ����!");
        }
        else
        {
            Debug.LogError("MapUI�� ã�� �� �����ϴ�.");
        }

        // PlanetManager�� PlanetUIManager�� ������ ã��
        if (planetManager == null)
            planetManager = FindObjectOfType<PlanetManager>();

        if (planetUIManager == null)
            planetUIManager = FindObjectOfType<PlanetUIManager>();

        // ���õ� �༺ ��������
        var selectedPlanet = planetManager.GetSelectedPlanet();
        var planetInfo = planetManager.GetPlanetInfo(selectedPlanet);

        // UI ������Ʈ
        planetUIManager.UpdateUI(planetInfo);
    }

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
        if (interactUI != null)
        {
            if (DeviceManager.isDeactived && collPlayer && !mapUI.activeSelf) interactUI.UseInteractUI(this.gameObject, Vector2.up * 0.5f);
            else interactUI.DisableInteractUI(this.gameObject);
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
        SoundManager.instance.PlaySound(SoundManager.Interact.TurnOnMovePlanet);
        mapUI.SetActive(!mapUI.activeSelf);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseMapUI()
    {
        mapUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
