using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThermometerSystem : MonoBehaviour
{
    [SerializeField] RectTransform sectionMark;
    [SerializeField] RectTransform[] roastSection;
    [SerializeField] int roastCount;
    public Slider powerSlider;

    const int FullLength = 750;
    const int FixedWidth = 50;
    float power = 1;
    int[] sections = new int[3];
    void Start()
    {
        SetSections();
        StartCoroutine(MoveMark(roastCount));
    }
    void SetSections() {
        roastSection[0].sizeDelta = new Vector2(FixedWidth, 300);
        roastSection[1].sizeDelta = new Vector2(FixedWidth, 150);
        roastSection[2].sizeDelta = new Vector2(FixedWidth, 300);
        roastSection[1].anchoredPosition = new Vector2(0, -roastSection[0].sizeDelta.y);
        sections = new int[3]{300, 150, 300};
    }
    
    void Update()
    {
        
    }

    IEnumerator MoveMark(int count) {
        float startPos = 0;
        float endPos = 750;
        float Speed = 0.1f;
        while(true) {
            startPos += Speed * power;
            sectionMark.anchoredPosition = new Vector2(sectionMark.anchoredPosition.x, startPos);
            if(Input.GetKeyDown(KeyCode.V)) {
                CheckSection(startPos);
                count--;
                break;
            }
            else if(sectionMark.anchoredPosition.y >= endPos) {
                sectionMark.anchoredPosition = new Vector2(sectionMark.anchoredPosition.x, endPos);
                CheckSection(endPos);
                count--;
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        if(count > 0) StartCoroutine(MoveMark(count));
    }

    void RotateIngredient() {
        
    }

    void CheckSection(float markPos) {
        if(markPos <= sections[0]) {
            Debug.Log("Rare");
        }
        else if(markPos <= sections[0] + sections[1]) {
            Debug.Log("Medium");
        }
        else Debug.Log("Well Done");
    }

    public void OnSliderValueChanged() {
        power = powerSlider.value;
    }
}
