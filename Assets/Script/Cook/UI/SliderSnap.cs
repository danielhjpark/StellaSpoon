using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SliderSnap : MonoBehaviour, IPointerUpHandler
{
    public Slider slider;
    public int currentPower = 0;
    public Action<int> OnSliderEvent;

    void Start()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float value = slider.value;
        float snappedValue = GetNearestSnapValue(value);
        slider.value = snappedValue;
        currentPower = (int)snappedValue;

        OnSliderEvent?.Invoke(currentPower);
    }

    float GetNearestSnapValue(float value)
    {
        float[] snapPoints = { 1f, 2f, 3f };
        float nearest = snapPoints[0];
        float minDistance = Mathf.Abs(value - nearest);

        for (int i = 1; i < snapPoints.Length; i++)
        {
            float distance = Mathf.Abs(value - snapPoints[i]);
            if (distance < minDistance)
            {
                nearest = snapPoints[i];
                minDistance = distance;
            }
        }

        return nearest;
    }
}