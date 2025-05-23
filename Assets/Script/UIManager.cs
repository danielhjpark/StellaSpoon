using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static public UIManager instance { get; private set; }

    [SerializeField] Item[] items;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RefrigeratorAddIngredient();
    }

    void Update()
    {

    }


    void RefrigeratorAddIngredient()
    {
        foreach (var item in items)
        {
            RefrigeratorManager.instance.AddItem(item, 50);
        }
    }


}
