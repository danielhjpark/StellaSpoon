using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStore : MonoBehaviour
{
    [SerializeField]
    private GameObject buyBase;
    [SerializeField]
    private GameObject sellBase;
    [SerializeField]
    private GameObject buyButton;
    [SerializeField]
    private GameObject sellButton;
    public void ClickBuy()
    {
        sellBase.SetActive(false);
        buyBase.SetActive(true);

        buyButton.SetActive(true);
        sellButton.SetActive(false);

    }
    public void ClickSell()
    {
        buyBase.SetActive(false);
        sellBase.SetActive(true);

        buyButton.SetActive(false);
        sellButton.SetActive(true);
    }
}
