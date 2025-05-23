using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FryingPanUI : MonoBehaviour
{
    [Header("Section")]
    [SerializeField] GameObject sectionObject;
    [SerializeField] RectTransform sectionMark;
    [SerializeField] RectTransform[] sections;

    [Header("Slider")]
    [SerializeField] GameObject powerSliderObject;
    [SerializeField] SliderSnap powerSliderSnap;

    [Header("UI")]
    [SerializeField] GameObject fryingPanUIObject;
    [SerializeField] GameObject ingredientUIObejct;

    private const int FullLength = 750;
    private int[] sectionRange = new int[3] { 250, 250, 250 };
    private float currentPos;
    private bool isEnd;
    private bool isUnlockStep;
    [NonSerialized] public int fireStep = 1;

    void Start()
    {
        fryingPanUIObject.SetActive(false);
        powerSliderSnap.OnSliderEvent += OnSliderValueChanged;

    }

    public void Initialize(int successFireStep)
    {
        if (RestaurantManager.instance.currentPanLevel >= 2)
        {
            this.isUnlockStep = true;
            fireStep = successFireStep;
            powerSliderObject.SetActive(false);
        }
        else isUnlockStep = false;

        SetSections();
    }

    public void OnFireControlUI()
    {
        fryingPanUIObject.SetActive(true);
        powerSliderObject.SetActive(true);
        sectionObject.SetActive(false);
    }

    public void OffFireControlUI()
    {
        fryingPanUIObject.SetActive(false);
        powerSliderObject.SetActive(false);
        sectionObject.SetActive(true);
    }

    public void OnFryingPanUI()
    {
        fryingPanUIObject.SetActive(true);
        ingredientUIObejct.SetActive(false);
    }

    public void OnIngredientUI()
    {
        fryingPanUIObject.SetActive(false);
        ingredientUIObejct.SetActive(true);
    }

    void SetSections()
    {
        sections[0].sizeDelta = new Vector2(sections[0].sizeDelta.x, sectionRange[0]);
        sections[1].sizeDelta = new Vector2(sections[1].sizeDelta.x, sectionRange[1]);
        sections[2].sizeDelta = new Vector2(sections[2].sizeDelta.x, sectionRange[2]);

        sections[1].anchoredPosition = new Vector2(0, -sections[0].sizeDelta.y);
    }

    public IEnumerator MoveMark()
    {
        float startPos = 0;
        float endPos = FullLength;
        float Speed = 0.1f * CookManager.instance.SlideAcceleration;
        isEnd = false;

        while (true)
        {
            startPos += Speed * fireStep;
            sectionMark.anchoredPosition = new Vector2(sectionMark.anchoredPosition.x, startPos);
            currentPos = startPos;
            if (sectionMark.anchoredPosition.y >= endPos)
            {
                sectionMark.anchoredPosition = new Vector2(sectionMark.anchoredPosition.x, endPos);
                currentPos = endPos;
                isEnd = true;
                break;
            }
            yield return null;
        }
    }

    public FryingStep GetCurrentSection()
    {
        if (currentPos <= sectionRange[0])
        {
            Debug.Log("Rare");
            return FryingStep.Rare;
        }
        else if (currentPos <= sectionRange[0] + sectionRange[1])
        {
            Debug.Log("Medium");
            return FryingStep.Medium;
        }
        else
        {
            Debug.Log("Well Done");
            return FryingStep.WellDone;
        }
    }

    public bool IsCheckEnd()
    {
        return isEnd;
    }

    public void OnSliderValueChanged(int powerValue)
    {
        fireStep = powerValue;
    }

    public bool CheckFireStep(int fireStep)
    {
        if (isUnlockStep) return true;
        else if (this.fireStep == fireStep) return true;
        else return false;
    }
}
