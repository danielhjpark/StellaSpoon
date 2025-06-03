using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mobility : MonoBehaviour
{
    [SerializeField]
    private GameObject mapUI; //지도 UI
    private InteractUI interactUI;
    private bool collPlayer = false; //플레이어 충돌체크

    public PlanetManager planetManager;
    public PlanetUIManager planetUIManager;

    private void OnEnable()
    {
        // 씬이 로드된 후 콜백 함수 설정
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 씬 로드 후 콜백 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Canvas의 자식 오브젝트 중 MapPanel을 찾기
        Transform canvasTransform = GameObject.Find("Canvas")?.transform; // Canvas를 찾기
        mapUI = canvasTransform.Find("MapPanel")?.gameObject; // MapPanel을 찾기
        interactUI = canvasTransform.Find("InteractPanel")?.GetComponent<InteractUI>();
        if (mapUI != null)
        {
            Debug.Log("MapUI 찾기 성공!");
        }
        else
        {
            Debug.LogError("MapUI를 찾을 수 없습니다.");
        }

        // PlanetManager와 PlanetUIManager를 씬에서 찾기
        if (planetManager == null)
            planetManager = FindObjectOfType<PlanetManager>();

        if (planetUIManager == null)
            planetUIManager = FindObjectOfType<PlanetUIManager>();

        // 선택된 행성 가져오기
        var selectedPlanet = planetManager.GetSelectedPlanet();
        var planetInfo = planetManager.GetPlanetInfo(selectedPlanet);

        // UI 업데이트
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
