using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingSystem : MonoBehaviour
{
    [SerializeField] RectTransform fullArea;
    [SerializeField] RectTransform timingArea;
    const int areaSizeW = 50;
    const int areaSizeH = 100;
    const int areaPosX = 0;

    float rangePos;

    private void Awake() {
        rangePos = fullArea.sizeDelta.y - 25;
    }

    void Start()
    {
        SetArea();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) {
            SetArea();
        }
    }

    void SetArea() {
        float rand = Random.Range(-rangePos, rangePos);
        timingArea.sizeDelta = new Vector2(areaSizeW, areaSizeH);
        timingArea.anchoredPosition = new Vector2(0, rand/2);
    }
}
