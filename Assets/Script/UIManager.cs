using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static public UIManager instance { get; private set; }
    [Header("UI")]
    [SerializeField] GameObject InteractUI;

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Inventory.inventoryActivated = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Inventory.inventoryActivated = false;
        }
    }


    void RefrigeratorAddIngredient()
    {
        foreach (var item in items)
        {
            RefrigeratorManager.instance.AddItem(item, 10);
        }
    }


    public void VisibleInteractUI()
    {
        InteractUI.SetActive(true);
    }

    public void HideInteractUI()
    {
        InteractUI.SetActive(false);
    }

}
