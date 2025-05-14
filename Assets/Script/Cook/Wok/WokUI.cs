using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class WokUI : MonoBehaviour
{
    [Header("Section")]
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
    private int unlockStep = 3; //Store Unlock Upgrade;

    private float[] sections;
    private float power = 1;
    private float currentPos;
    private bool isEnd;

    void Start()
    {
        SetSections();
        wokUIObject.SetActive(false);
        powerSliderSnap.OnSliderEvent += OnSliderValueChanged;
    }

    public void Initialize(int unlockStep)
    {
        //this.unlockStep = unlockStep;
        this.unlockStep = 2;
        if(unlockStep >= 2) {
            AutoFireTemperature();
        }

        SetSections();
        wokUIObject.SetActive(false);
    }


    void AutoFireTemperature() {
        power = 3;
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
            startPos += Speed * power;
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
        power = powerValue;
    }
}
