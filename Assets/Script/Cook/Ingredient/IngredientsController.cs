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
        if (Physics.Raycast(ray, out RaycastHit hitLayerMask, Mathf.Infinity, LayerMask.GetMask("ControllLayer")))
        {
            float H = Camera.main.transform.position.y;
            float h = controllObject.transform.position.y;
            Vector3 newPos = (hitLayerMask.point * (H - h) + Camera.main.transform.position * h) / H;
            controllObject.transform.position = hitLayerMask.point;
        }
        else {
            controllObject.transform.position = Input.mousePosition;
        }
    }

    public void ObjectDrop() {
        DropCheck();
        if(Input.GetMouseButtonUp(0)) {
            if(isCanDrop) {
                
                isControll = false;
                isCanDrop = false;
                CookManager.instance.DropObject(controllObject);
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

    void DropCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitLayerMask, Mathf.Infinity, LayerMask.GetMask("DropLayer")))
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

        controllObject = Instantiate(ingredientObject, Input.mousePosition, ingredientObject.transform.rotation);
        StartCoroutine(cookInventoryManager.HidePanel());
        isControll = true;
    }
}


