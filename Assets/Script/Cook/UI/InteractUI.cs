using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] GameObject interactObject;

    private RectTransform interactRect;
    private GameObject currentTarget;
    private Vector3 currentPivotPos;

    void Awake()
    {
        interactRect = interactObject.GetComponent<RectTransform>();
        currentPivotPos = Vector3.zero;
    }

    private void Update()
    {
        UpdatePos();
    }
    //Ȱ��ȭ
    public void UseInteractUI(GameObject target)
    {
        currentTarget = target;
        interactObject.SetActive(true);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTarget.transform.position);
        interactRect.position = screenPos;
    }

    public void UseInteractUI(GameObject target, Vector3 pivotPos)
    {
        currentTarget = target;
        currentPivotPos = pivotPos;
        interactObject.SetActive(true);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTarget.transform.position + currentPivotPos);
        interactRect.position = screenPos;
    }

    //��Ȱ��ȭ
    public void DisableInteractUI()
    {
        currentTarget = null;
        interactObject.SetActive(false);
    }

    public void DisableInteractUI(GameObject target)
    {
        if (target == currentTarget)
        {
            currentTarget = null;
            interactObject.SetActive(false);
        }
    }
    //��ǥ ����
    public void UpdatePos()
    {
        if (currentTarget == null)
        {
            DisableInteractUI();
            return;
        }
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTarget.transform.position + currentPivotPos);
        interactRect.position = screenPos;
    }
}
