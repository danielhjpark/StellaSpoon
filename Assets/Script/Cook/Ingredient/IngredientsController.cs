using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientsController : MonoBehaviour , IPointerDownHandler
{
    CookUIManager cookInventoryManager;
    IngredientSlot ingredientSlot;

    GameObject ingredientObject;
    GameObject controllObject;

    bool isControll;
    bool isCanDrop;

    void Start()
    {
        cookInventoryManager = GetComponentInParent<CookUIManager>();
        ingredientSlot = GetComponent<IngredientSlot>();
        isControll = false;
        isCanDrop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(controllObject != null && isControll) {
            OnUnitMove();
        }
        if(isControll) {
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
        else {
            //controllObject.transform.position = Input.mousePosition;
        }
    }

    public void ObjectDrop() {
        if(Input.GetMouseButtonUp(0)) {
            DropCheck();
            if(isCanDrop) {
                isControll = false;
                isCanDrop = false;
                CookManager.instance.DropObject(controllObject, ingredientSlot.currentIngredient);
                SlotUpdate();
                controllObject = null;  

            }
            else {
                Destroy(controllObject);
                isControll = false;
                isCanDrop = false;
                StartCoroutine(cookInventoryManager.VisiblePanel());
            }
            
        }

    }

    void SlotUpdate() {
        ingredientSlot.itemCount --;
        IngredientManager.IngredientAmount[ingredientSlot.currentIngredient] --;
        ingredientSlot.refrigeratorInventory.UseIngredient(ingredientSlot.currentIngredient, 1);
        //ingredientSlot.SetSlotCount(ingredientSlot.itemCount);
        if(ingredientSlot.itemCount <= 0) {
            ingredientSlot.SlotClear();
            ingredientSlot.gameObject.SetActive(false);

        }
        
        if(CookManager.instance.cookMode == CookManager.CookMode.Make) {
            
        }
    }

    void DropCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitLayerMask, Mathf.Infinity, LayerMask.GetMask("DropArea")))
        {
            isCanDrop = true;
        }
        else
        {
            isCanDrop = false;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if(ingredientSlot.currentIngredient != null) {
            ingredientObject = ingredientSlot.currentIngredient.ingredientPrefab;
        }
        else return;
        controllObject = Instantiate(ingredientObject, CookManager.instance.spawnPoint.position, ingredientObject.transform.rotation);
        //controllObject.transform.SetParent(CookManager.instance.spawnPoint, false);
        StartCoroutine(cookInventoryManager.HidePanel());
        isControll = true;
    }
}


