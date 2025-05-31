using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager instance;
    public Item[] items;
    public Inventory chest1Inventory;
    public Inventory chest2Inventory;
    public Inventory refrigeratorInventory;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        RefrigeratorAddIngredient();
        
    }

    void RefrigeratorAddIngredient()
    {
        foreach (var item in items)
        {
            chest1Inventory.AcquireItem(item, 10);
            chest2Inventory.AcquireItem(item, 10);
            RefrigeratorManager.instance.AddItem(item, 10);
        }
    }

}
