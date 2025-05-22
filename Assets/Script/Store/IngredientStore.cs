using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngredientStore : MonoBehaviour
{

    [SerializeField]
    private StoreUIManager storeUIManager;
    [SerializeField]
    private GameObject buyBase;
    [SerializeField]
    private GameObject sellBase;
    [SerializeField]
    private GameObject buyButton;
    [SerializeField]
    private GameObject sellButton;


    private void Start()
    {
        storeUIManager = GameObject.Find("StoreUIManager").GetComponent<StoreUIManager>();
    }
    public void ClickBuy()
    {
        sellBase.SetActive(false);
        buyBase.SetActive(true);

        buyButton.SetActive(true);
        sellButton.SetActive(false);

        storeUIManager.countText.text = "0";
        storeUIManager.ingredientNeedGold.text = "0";
        storeUIManager.currentState = StoreUIManager.CurrentState.Buy;

    }
    public void ClickSell()
    {
        buyBase.SetActive(false);
        sellBase.SetActive(true);

        buyButton.SetActive(false);
        sellButton.SetActive(true);

        storeUIManager.countText.text = "0";
        storeUIManager.ingredientNeedGold.text = "0";

        storeUIManager.currentState = StoreUIManager.CurrentState.Sell;
    }
}
