using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManger : MonoBehaviour
{
    public static TutorialManger instance;
    [Header("Parent UI")]
    [SerializeField] GameObject tutorialUI;

    [Header("SelectUI")]
    [SerializeField] GameObject selectUI;

    [Header("Tutorial Types")]
    [SerializeField] GameObject[] tutorialCanvas;

    [Header("Button UI")]
    [SerializeField] GameObject buttonUI;
    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;

    [Header("Return UI")]
    [SerializeField] GameObject returnButton;
    [SerializeField] GameObject escUI;

    [NonSerialized] public GameObject currentTutorial;

    private int currentPage;
    private int maxPage;
    public bool isTutorialOpen = false;
    public bool isAreaTutorial;

    public event Action OnTutorialClose;
    //Tutorial Check PlayerPrefs name

    public enum TutorialType
    {
        MOVEMAP,
        RESTAURENT,
        DAILYMENU,
        CUTTINGBOARD,
        PLANET
    }

    void Awake()
    {
        instance = this;
        CutSceneManager.OnCutSceneStart += ResetAllTutorials;
    }

    public static void MakeTutorial(TutorialType type)
    {
        string key = $"Tutorial_{type}";
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }

    public static void ResetTutorial(TutorialType type)
    {
        string key = $"Tutorial_{type}";
        PlayerPrefs.DeleteKey(key);
    }

    public static void ResetAllTutorials()
    {
        foreach (TutorialType type in Enum.GetValues(typeof(TutorialType)))
        {
            ResetTutorial(type);
        }
    }

    public static bool ShouldShowTutorial(TutorialType type)
    {
        string key = $"Tutorial_{type}";
        return PlayerPrefs.GetInt(key, 0) == 0;
    }

    private void Update()
    {
        UpdatePageUI();
        UpdateButtonUI();
        if (isTutorialOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) CloseTutorialUI();
    }

    void UpdatePageUI()
    {
        if (currentTutorial == null) return;

        for (int i = 0; i <= maxPage; i++)
        {
            if (i == currentPage) currentTutorial.transform.GetChild(i).gameObject.SetActive(true);
            else currentTutorial.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void UpdateButtonUI()
    {
        if (currentTutorial == null) return;

        if (isAreaTutorial) returnButton.SetActive(false);
        else returnButton.SetActive(true);

        if (currentPage == 0) leftButton.SetActive(false);
        else leftButton.SetActive(true);

        if (currentPage == maxPage)
        {
            if (isAreaTutorial)
            {
                escUI.SetActive(true);

            }
            else escUI.SetActive(false);
            rightButton.SetActive(false);
        }
        else
        {
            rightButton.SetActive(true);
            escUI.SetActive(false);
        }
    }

    public void CloseTutorialUI() //UI ´Ý±â
    {
        if (!isTutorialOpen) return;
        if (isAreaTutorial && currentPage != maxPage) return;

        tutorialUI.SetActive(false);
        isTutorialOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InteractUIManger.isUseInteractObject = false;
        DeviceManager.isDeactived = true;
        
        OnTutorialClose?.Invoke();
    }

    public void OpenTutorialUI()
    {
        tutorialUI.SetActive(true);
        isTutorialOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InteractUIManger.isUseInteractObject = true;
        DeviceManager.isDeactived = false;
    }

    //Select Button use this
    public void SelectTutorial(int tutorialNum)
    {
        isAreaTutorial = false;
        if (currentTutorial != null)
        {
            currentTutorial.SetActive(false);
            currentTutorial = null;
        }
        currentTutorial = tutorialCanvas[tutorialNum];
        currentTutorial.SetActive(true);
        buttonUI.SetActive(true);
        selectUI.SetActive(false);

        currentPage = 0;
        maxPage = currentTutorial.transform.childCount - 1;
    }

    public void SelectTutorial(TutorialType tutorialType)
    {
        int tutorialNum = (int)tutorialType + 3;
        isAreaTutorial = true;
        MakeTutorial(tutorialType);
        if (currentTutorial != null)
        {
            currentTutorial.SetActive(false);
            currentTutorial = null;
        }
        currentTutorial = tutorialCanvas[tutorialNum];
        currentTutorial.SetActive(true);
        buttonUI.SetActive(true);
        selectUI.SetActive(false);

        currentPage = 0;
        maxPage = currentTutorial.transform.childCount - 1;
    }

    // Tutorial Device
    public void OpenSelectTutorial()
    {
        if (currentTutorial != null)
        {
            currentTutorial.SetActive(false);
            currentTutorial = null;
        }
        buttonUI.SetActive(false);
        selectUI.SetActive(true);
        isAreaTutorial = false;
    }

    // Right button to next page
    public void NextPage()
    {
        if (currentPage < maxPage) currentPage++;
    }

    //left button to previous page
    public void PreviousPage()
    {
        if (currentPage > 0) currentPage--;
    }

}
