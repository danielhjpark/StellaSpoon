using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoardUI : MonoBehaviour
{
    [Header("UI Setting")]
    [SerializeField] GameObject cuttingBoardUI;
    [SerializeField] GameObject sliceUI;
    [SerializeField] GameObject sliceClickUI;
    [SerializeField] GameObject rotateUI;

    public void VisibleRotateUI()
    {
        cuttingBoardUI.SetActive(true);
        rotateUI.SetActive(true);
        sliceUI.SetActive(false);
    }

    public void VisibleSliceUI()
    {
        cuttingBoardUI.SetActive(true);
        rotateUI.SetActive(false);
        sliceUI.SetActive(true);
    }

    public void VisibleSliceClickUI()
    {
        sliceClickUI.SetActive(true);
    }

    public void HideSliceClickUI()
    {
        sliceClickUI.SetActive(false);
    }

    public void HideCuttingBoardUI()
    {
        cuttingBoardUI.SetActive(false);
        rotateUI.SetActive(false);
        sliceUI.SetActive(false);
    }


}
