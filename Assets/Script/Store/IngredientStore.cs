using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStore : MonoBehaviour
{
    [SerializeField]
    private GameObject buyBase;
    [SerializeField]
    private GameObject sellBase;
    public void ClickBuy()
    {
        sellBase.SetActive(false);
        buyBase.SetActive(true);

    }
    public void ClickSell()
    {
        buyBase.SetActive(false);
        sellBase.SetActive(true);

    }
}
