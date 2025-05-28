using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] GameObject interactObject;

    private RectTransform interactRect;
    private GameObject currentTarget;

    void Awake()
    {
        interactRect = interactObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdatePos();
    }

    public void UseInteractUI(GameObject target)
    {
        currentTarget = target;
        interactObject.SetActive(true);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTarget.transform.position);
        // Vector3 dir = Camera.main.transform.position - transform.position;
        //dir.y = 0; // y축 고정
        //transform.rotation = Quaternion.LookRotation(-dir);
        interactRect.position = screenPos;
    }

    public void DisableInteractUI()
    {
        currentTarget = null;
        interactObject.SetActive(false);
    }

    // Update is called once per frame
    public void UpdatePos()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTarget.transform.position);
        // Vector3 dir = Camera.main.transform.position - transform.position;
        //dir.y = 0; // y축 고정
        //transform.rotation = Quaternion.LookRotation(-dir);
        interactRect.position = screenPos;
    }
}
