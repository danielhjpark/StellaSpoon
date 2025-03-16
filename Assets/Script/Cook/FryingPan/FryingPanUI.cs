using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FryingPanUI : MonoBehaviour
{
    [SerializeField] RectTransform sectionMark;
    [SerializeField] Slider powerSlider;
    [SerializeField] RectTransform[] sections;
    [SerializeField] int roastCount;

    [SerializeField] GameObject fryingPanUIObject;
    [SerializeField] GameObject ingredientUIObejct;

    const int FullLength = 750;
    const int FixedWidth = 50;
    float power = 1;
    int[] sectionRange = new int[3];

    private float currentPos;
    private bool isEnd;


    void Start()
    {
        fryingPanUIObject.SetActive(false);
    }

    public void Initialize(FryingSetting fryingSetting) {
        sectionRange = fryingSetting.sectionRange;
        SetSections();
    }

    public void OnFryingPanUI() {
        fryingPanUIObject.SetActive(true);
        ingredientUIObejct.SetActive(false);
    }


    void SetSections() {
        sections[0].sizeDelta = new Vector2(FixedWidth, sectionRange[0]);
        sections[1].sizeDelta = new Vector2(FixedWidth, sectionRange[1]);
        sections[2].sizeDelta = new Vector2(FixedWidth, sectionRange[2]);

        sections[1].anchoredPosition = new Vector2(0, -sections[0].sizeDelta.y);
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
            if(sectionMark.anchoredPosition.y >= endPos) {
                sectionMark.anchoredPosition = new Vector2(sectionMark.anchoredPosition.x, endPos);
                currentPos = endPos;
                isEnd = true;
                break;
            }
            yield return null;
        }
    }

    public void GetCurrentSection() {
        if(currentPos <= sectionRange[0]) {
            Debug.Log("Rare");
        }
        else if(currentPos <= sectionRange[0] + sectionRange[1]) {
            Debug.Log("Medium");
        }
        else Debug.Log("Well Done");
    }
    
    public bool IsCheckEnd() {
        return isEnd;
    }

    public void OnSliderValueChanged() {
        power = powerSlider.value;
    }
}
