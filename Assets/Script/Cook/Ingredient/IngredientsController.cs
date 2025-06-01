using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientsController : MonoBehaviour, IPointerDownHandler
{
    CookUIManager cookUIManager;
    IngredientSlot ingredientSlot;

    GameObject ingredientObject;
    GameObject controllObject;

    bool isControll;

    void Start()
    {
        cookUIManager = GetComponentInParent<CookUIManager>();
        ingredientSlot = GetComponent<IngredientSlot>();
        isControll = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CookManager.instance.isCanIngredientControll)
        {
            CancleIngredientControll();
            return;
        }
        if (controllObject != null && isControll)
        {
            OnUnitMove();
        }
        if (isControll)
        {
            ObjectDrop();
        }
    }

    public void OnUnitMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitLayerMask, Mathf.Infinity, LayerMask.GetMask("ControllArea")))
        {
            float H = Camera.main.transform.position.y;
            float h = controllObject.transform.position.y;
            Vector3 newPos = (hitLayerMask.point * (H - h) + Camera.main.transform.position * h) / H;
            controllObject.transform.position = hitLayerMask.point;
        }
        else
        {
            //controllObject.transform.position = Input.mousePosition;
        }
    }

    public void ObjectDrop()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (DropCheck())
            {
                CookManager.instance.DropObject(controllObject, ingredientSlot.currentIngredient);
                controllObject = null;
                SlotUpdate();
            }
            else
            {
                Destroy(controllObject);
                StartCoroutine(cookUIManager.VisiblePanel());
            }
            isControll = false;
        }
    }

    void SlotUpdate()
    {
        int useCount = ingredientSlot.currentIngredient.ingredientUseCount;

        ingredientSlot.itemCount -= useCount;

        if (CookManager.instance.cookMode == CookManager.CookMode.Make)
        {
            if (ingredientSlot.currentIngredient.ingredientType == IngredientType.Sub)
            {
                IngredientManager.IngredientAmount[ingredientSlot.currentIngredient] -= useCount;
                ingredientSlot.refrigeratorInventory.UseIngredient(ingredientSlot.currentIngredient, useCount);
            }
            else
            {
                IngredientManager.IngredientAmount[ingredientSlot.currentIngredient]--;
                ingredientSlot.refrigeratorInventory.UseIngredient(ingredientSlot.currentIngredient, 1);
            }
        }
        if (ingredientSlot.itemCount < useCount)
        {
            ingredientSlot.SlotClear();
            ingredientSlot.gameObject.SetActive(false);
            return;
        }
    }

    bool DropCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitLayerMask, Mathf.Infinity, LayerMask.GetMask("DropArea")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (ingredientSlot.currentIngredient != null)
        {
            ingredientObject = ingredientSlot.currentIngredient.ingredientPrefab;
        }
        else return;
        //controllObject = Instantiate(ingredientObject, CookManager.instance.spawnPoint.position, ingredientObject.transform.rotation);
        controllObject = Instantiate(ingredientObject, CookManager.instance.spawnPoint.position, ingredientObject.transform.rotation);
        //controllObject.transform.SetParent(CookManager.instance.spawnPoint, false);
        StartCoroutine(cookUIManager.HidePanel());
        isControll = true;
    }

    public void CancleIngredientControll()
    {
        Destroy(controllObject);
        StartCoroutine(cookUIManager.HidePanel());
    }
}


