using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WokUI : MonoBehaviour
{
    [SerializeField] RectTransform sectionMark;
    [SerializeField] RectTransform[] roastSection;
    [SerializeField] int roastCount;
    [SerializeField] Slider powerSlider;
    [SerializeField] GameObject wokUIObject;
    [SerializeField] GameObject ingredientUIObejct;
    const int FullLength = 750;
    const int FixedWidth = 50;
    float power = 1;
    int[] sections = new int[3];
    public event Action<bool> OnWokSystem;
    private float currentPos;
    bool isEnd;

    void Start()
    {
        SetSections();
        wokUIObject.SetActive(false);
        
        //StartCoroutine(MoveMark());
    }
    void SetSections() {
        roastSection[0].sizeDelta = new Vector2(FixedWidth, 300);
        roastSection[1].sizeDelta = new Vector2(FixedWidth, 150);
        roastSection[2].sizeDelta = new Vector2(FixedWidth, 300);
        roastSection[1].anchoredPosition = new Vector2(0, -roastSection[0].sizeDelta.y);
        sections = new int[3]{300, 150, 300};
    }
   
    public void OnWokUI() {
        wokUIObject.SetActive(true);
        ingredientUIObejct.SetActive(false);
    }

    public IEnumerator MoveMark() {
        float startPos = 0;
        float endPos = FullLength;
        float Speed = 0.1f;
        isEnd = false;

        while(true) {
            startPos += Speed * power;
            sectionMark.anchoredPosition = new Vector2(sectionMark.anchoredPosition.x, startPos);
            currentPos = startPos;
            CheckSection();
            if(sectionMark.anchoredPosition.y >= endPos) {
                sectionMark.anchoredPosition = new Vector2(sectionMark.anchoredPosition.x, endPos);
                currentPos = endPos;
                isEnd = true;
                break;
            }
            yield return null;
        }
    }

    void RotateIngredient() {
        
    }

    public bool IsCheckEnd() {
        return isEnd;
    }

    void CheckSection() {
        if(currentPos <= sections[0]) {
            OnWokSystem?.Invoke(false);
        }
        else if(currentPos <= sections[0] + sections[1]) {
            OnWokSystem?.Invoke(true);
        }
        else {
            OnWokSystem?.Invoke(false);
        }
    }

    public void OnSliderValueChanged() {
        power = powerSlider.value;
    }
}
