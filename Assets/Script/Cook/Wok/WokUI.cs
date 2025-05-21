using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class WokUI : MonoBehaviour
{
    [Header("Section")]
    [SerializeField] GameObject sectionObject;
    [SerializeField] RectTransform sectionMark;
    [SerializeField] RectTransform[] roastSection;

    [Header("Slider")]
    [SerializeField] GameObject powerSliderObject;
    [SerializeField] SliderSnap powerSliderSnap;

    [Header("UI")]
    [SerializeField] GameObject wokUIObject;
    [SerializeField] GameObject ingredientUIObejct;

    public event Action<bool> OnWokSystem;
    private const int FullLength = 750;
    private float[] sections;
    private float currentPos;
    private bool isEnd;
    private bool isUnlockStep;
    private int unlockStep; //Store Unlock Upgrade;

    [NonSerialized] public int fireStep = 1; // fireSlider 

    void Start()
    {
        SetSections();
        wokUIObject.SetActive(false);
        powerSliderSnap.OnSliderEvent += OnSliderValueChanged;
    }

    public void Initialize(int successFireStep)
    {
        this.unlockStep = RestaurantManager.instance.currentWorLevel;

        if (RestaurantManager.instance.currentWorLevel >= 2)
        {
            AutoFireTemperature();
            isUnlockStep = true;
            fireStep = successFireStep;
            powerSliderObject.SetActive(false);
        }
        else isUnlockStep = false;

        SetSections();
    }

    public void OnFireControlUI()
    {
        wokUIObject.SetActive(true);
        powerSliderObject.SetActive(true);
        sectionObject.SetActive(false);
    }

    public void OffFireControlUI()
    {
        wokUIObject.SetActive(false);
        powerSliderObject.SetActive(false);
        sectionObject.SetActive(true);
    }

    void AutoFireTemperature()
    {
        fireStep = 2;
        powerSliderObject.SetActive(false);
    }

    float GetSuccessSection()
    {
        float sectionValue;
        switch (unlockStep)
        {
            case 0:
                sectionValue = 150;
                break;
            case 1:
                sectionValue = 187.5f;
                break;
            case 2:
                sectionValue = 187.5f;
                break;
            case 3:
                sectionValue = 225;
                break;
            default:
                sectionValue = 150;
                break;
        }
        return sectionValue;
    }

    void SetSections()
    {
        float successsSection = GetSuccessSection();
        float failSection = (FullLength - successsSection) / 2;

        roastSection[0].sizeDelta = new Vector2(roastSection[0].sizeDelta.x, failSection);
        roastSection[1].sizeDelta = new Vector2(roastSection[1].sizeDelta.x, successsSection);
        roastSection[2].sizeDelta = new Vector2(roastSection[2].sizeDelta.x, failSection);
        roastSection[1].anchoredPosition = new Vector2(0, -roastSection[0].sizeDelta.y);

        sections = new float[3] { failSection, successsSection, failSection };
    }

    public void OnWokUI()
    {
        wokUIObject.SetActive(true);
        ingredientUIObejct.SetActive(false);
    }

    public void OnFridgeUI()
    {
        wokUIObject.SetActive(false);
        ingredientUIObejct.SetActive(true);
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
            CheckSection();
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

    public bool IsCheckEnd()
    {
        return isEnd;
    }

    void CheckSection()
    {
        if (currentPos <= sections[0])
        {
            OnWokSystem?.Invoke(false);
        }
        else if (currentPos <= sections[0] + sections[1])
        {
            OnWokSystem?.Invoke(true);
        }
        else
        {
            OnWokSystem?.Invoke(false);
        }
    }

    public void OnSliderValueChanged(int powerValue)
    {
        fireStep = powerValue;
    }

    public bool CheckFireStep(int fireStep)
    {
        if (unlockStep >= 2) return true;
        else if (this.fireStep == fireStep) return true;
        else return false;
    }
}
