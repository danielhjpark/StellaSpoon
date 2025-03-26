using System.Collections;
using System.Collections.Generic;
using cakeslice;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SauceController : MonoBehaviour
{
    [SerializeField] Transform SaucePos;
    [SerializeField] OutlineEffect outlineEffect;
    [SerializeField] GameObject[] sauceContainers = new GameObject[3];
    [SerializeField] SauceSystem bottomSauceSystem;

    private SauceAnimator sauceAnimator;
    [SerializeField] GameObject controllObject;
    private Vector3 initPos;
    private bool isControll;
    private SauceType sauceType;

    void Start()
    {
        sauceAnimator = GetComponent<SauceAnimator>();
        outlineEffect.enabled = false;
        isControll = false;
        for (int i = 0; i < 3; i++)
        {
            sauceContainers[i].GetComponent<Collider>().enabled = false;
        }
    }

    void Update()
    {
        ControllCheck();
        if (controllObject != null && isControll)
        {
            ObjectMove();
        }
        if (isControll)
        {
            ObjectDrop();
        }
    }

    public void Initialize(SauceType sauceType)
    {
        this.sauceType = sauceType;
        outlineEffect.enabled = true;
        if (sauceType == SauceType.None) return;

        sauceContainers[(int)sauceType].GetComponent<Collider>().enabled = true;
        sauceContainers[(int)sauceType].GetComponent<SauceSystem>().SetSauceColor(sauceType);
        sauceContainers[(int)sauceType].transform.GetChild(0).AddComponent<cakeslice.Outline>().enabled = true;
    }

    public void InitializeMakeMode()
    {
        outlineEffect.enabled = true;
        for (int i = 0; i < 3; i++)
        {
            sauceContainers[i].GetComponent<Collider>().enabled = true;
            sauceContainers[i].GetComponent<SauceSystem>().SetSauceColor((SauceType)i);
            sauceContainers[i].transform.GetChild(0).AddComponent<cakeslice.Outline>().enabled = true;
        }

    }

    private void OutlineDisable()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject outlineObject = sauceContainers[i].transform.GetChild(0).gameObject;
            if (outlineObject.TryGetComponent<cakeslice.Outline>(out cakeslice.Outline outline))
                outline.enabled = false;
        }
    }

    public void ObjectMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitLayerMask, Mathf.Infinity, LayerMask.GetMask("ControllArea")))
        {
            controllObject.transform.position = hitLayerMask.point;
        }
        else
        {
            controllObject.transform.position = Input.mousePosition;
        }
    }

    public void ObjectDrop()
    {
        if (Input.GetMouseButtonUp(0))
        {
            bool isCanDrop = DropCheck();
            if (isCanDrop)
            {
                isControll = false;
                controllObject.transform.position = SaucePos.transform.position;
                sauceAnimator = controllObject.GetComponent<SauceAnimator>();
                bottomSauceSystem.SetSauceColor(controllObject.GetComponent<SauceSystem>().sauceType);
                OutlineDisable();
                StartCoroutine(AddSauce());
            }
            else
            {
                controllObject.transform.position = initPos;
                controllObject = null;
                isControll = false;
            }

        }

    }

    public IEnumerator AddSauce()
    {
        yield return StartCoroutine(sauceAnimator.TiltSauceContainer());
        yield return new WaitForSeconds(0.1f);
        controllObject.transform.position = initPos;
        controllObject = null;
        this.gameObject.GetComponent<SauceController>().enabled = false;
    }

    bool DropCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, Mathf.Infinity, LayerMask.GetMask("DropArea")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ControllCheck()
    {
        if (!Input.GetMouseButton(0)) { return; }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitObject, Mathf.Infinity, LayerMask.GetMask("Item")))
        {
            Debug.Log(hitObject.transform.name);
            if (hitObject.transform.name == "Sauce" && controllObject == null)
            {
                Debug.Log("Interact");
                initPos = hitObject.transform.position;
                controllObject = hitObject.transform.gameObject;
                isControll = true;
            }
        }
    }
}
