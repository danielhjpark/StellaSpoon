using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetImageChange : MonoBehaviour
{
    [SerializeField]
    private Sprite defaltImage;
    [SerializeField]
    private Sprite scaliaImage;
    [SerializeField]
    private Sprite serenoxiaImage;

    [SerializeField]
    private GameObject planetImageObject;

    private void OnEnable()
    {
        if (planetImageObject == null) return;

        var image = planetImageObject.GetComponent<Image>();
        if (image == null) return;

        switch (PlanetManager.selectedPlanet)
        {
            case PlanetManager.PlanetType.aRedForest:
                image.sprite = scaliaImage;
                break;
            case PlanetManager.PlanetType.Serenoxia:
                image.sprite = serenoxiaImage;
                break;
            default:
                image.sprite = defaltImage;
                break;
        }
    }
}
