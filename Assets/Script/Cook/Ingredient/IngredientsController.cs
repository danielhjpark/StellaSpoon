using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientsController : MonoBehaviour , IPointerDownHandler
{
    CookUIManager cookInventoryManager;
    [SerializeField] GameObject ingredientsObject;
    GameObject controllObject;

    [SerializeField] GameObject wokObject;
    bool isControll;
    bool isCanDrop;
    Vector3 previousPos;
    void Start()
    {
        cookInventoryManager = GetComponentInParent<CookUIManager>();
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
            previousPos =hitLayerMask.point;
        }
        else {
            controllObject.transform.position = Input.mousePosition;//previousPos;
        }
    }

    public void ObjectDrop() {
        CheckOverlap3D();
        if(Input.GetMouseButtonUp(0)) {
            if(isCanDrop) {
                controllObject.transform.SetParent(wokObject.transform);
                controllObject.GetComponent<Rigidbody>().useGravity = true;
                isControll = false;
                isCanDrop = false;
            }
            else {
                Destroy(controllObject);
                isControll = false;
                isCanDrop = false;
            }
            StartCoroutine(cookInventoryManager.VisiblePanel());
        }

    }

    void CheckOverlap3D()
    {
        Collider[] colliders = Physics.OverlapSphere(controllObject.transform.position, 0.01f, LayerMask.GetMask("DropArea"));

        if (colliders.Length > 0)
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
        controllObject = Instantiate(ingredientsObject, Input.mousePosition, Quaternion.identity);
        controllObject.GetComponent<Rigidbody>().useGravity = false;
        StartCoroutine(cookInventoryManager.HidePanel());
        isControll = true;
    }
}


