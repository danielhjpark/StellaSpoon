using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderSnap : MonoBehaviour, IPointerUpHandler
{
    public Slider slider;

    void Start()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float value = slider.value;
        float snappedValue = GetNearestSnapValue(value);
        slider.value = snappedValue;
    }

    float GetNearestSnapValue(float value)
    {
        float[] snapPoints = { 1f, 3f, 5f };
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