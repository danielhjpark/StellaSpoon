using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManger : MonoBehaviour
{
    [SerializeField] GameObject selectUI;
    [SerializeField] GameObject[] tutorialCanvas;
    [Header("Button UI")]
    [SerializeField] GameObject buttonUI;
    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;
    [NonSerialized] public GameObject currentTutorial;

    private int currentPage;
    private int maxPage;

    void OnEnable()
    {
        ReturnTutorial();
    }

    private void Update()
    {
        OpenPage();
        ButtonView();
    }

    void ButtonView()
    {
        if (currentTutorial == null) return;

        if (currentPage == 0) leftButton.SetActive(false);
        else leftButton.SetActive(true);

        if (currentPage == maxPage) rightButton.SetActive(false);
        else rightButton.SetActive(true);
    }

    void OpenPage()
    {
        if (currentTutorial == null) return;

        for (int i = 0; i <= maxPage; i++)
        {
            if (i == currentPage) currentTutorial.transform.GetChild(i).gameObject.SetActive(true);
            else currentTutorial.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    //Select Button use this
    public void SelectTutorial(int tutorialNum)
    {
        currentTutorial = tutorialCanvas[tutorialNum];
        currentTutorial.SetActive(true);
        buttonUI.SetActive(true);
        selectUI.SetActive(false);

        currentPage = 0;
        maxPage = currentTutorial.transform.childCount - 1;
    }

    // Back Button use this
    public void ReturnTutorial()
    {
        if (currentTutorial != null)
        {
            currentTutorial.SetActive(false);
            currentTutorial = null;
        }
        buttonUI.SetActive(false);
        selectUI.SetActive(true);
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
