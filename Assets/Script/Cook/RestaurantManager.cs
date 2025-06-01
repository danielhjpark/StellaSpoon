using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager instance;
    [Header("�������")]
    public Item[] mainIngredients;
    [Header("�������")]
    public Item[] subIngredients;
    [Header("����")]
    public Inventory chest1Inventory;
    public Inventory chest2Inventory;
    public bool isInitialize;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            isInitialize = true;
        }

    }

    private void Start()
    {
        AddIngredient();
    }

    void AddIngredient()
    {
        if (isInitialize) isInitialize = false;
        else return;
        foreach (var item in mainIngredients)
        {
            chest1Inventory.AcquireItem(item, 1);
            chest2Inventory.AcquireItem(item, 1);
            RefrigeratorManager.instance.AddItem(item, 1);
        }
        foreach (var item in subIngredients)
        {
            RefrigeratorManager.instance.AddItem(item, 10);
            
        }
    }

}
