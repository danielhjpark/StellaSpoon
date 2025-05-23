using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class CuttingLineSystem : MonoBehaviour
{
    [SerializeField] GameObject LinePrefab;
    [SerializeField] GameObject ScannerObject;
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;

    public List<GameObject> cuttingLines = new List<GameObject>();
    public Action OnCuttingSystem;
    public float scanValue { get; set; } //Store unlock this value;

    private void CreateLine(Vector3 getPosition, Vector3 getSize)
    {
        Vector3 cuttingPosition = new Vector3(getPosition.x, getPosition.y, getPosition.z);
        Vector3 cuttingSize = new Vector3(0.01f, getSize.y, getSize.z);
        GameObject newObject = Instantiate(LinePrefab, cuttingPosition, Quaternion.identity);
        newObject.transform.localScale = cuttingSize;
        newObject.transform.parent = this.gameObject.transform;
        cuttingLines.Add(newObject);
        newObject.SetActive(true);
    }

    private void CalculateCuttingLine(Renderer renderer, float t)
    {
        Vector3 min = renderer.bounds.min;
        Vector3 max = renderer.bounds.max;
        Vector3 slicePosition = Vector3.Lerp(min, max, t);
        Vector3 cuttingPosition = new Vector3(slicePosition.x, (min.y + max.y) / 2f, (min.z + max.z) / 2f);

        float sizeY = Mathf.Abs(min.y - max.y);
        float sizeZ = Mathf.Abs(min.z - max.z);
        Vector3 cuttingSize = new Vector3(0, sizeY, sizeZ);

        CreateLine(cuttingPosition, cuttingSize);
    }

    public void CreateCuttingLine(int count, GameObject targetObject)
    {
        ScanReset();
        for (int i = 1; i < count; i++)
        {
            float t = i / (float)count;
            CalculateCuttingLine(targetObject.GetComponent<Renderer>(), t);
        }
    }

    public IEnumerator ScanObject()
    {
        float t = 0;
        ScannerObject.SetActive(true);

        while (true)
        {
            if (RestaurantManager.instance.currentCuttingBoardLevel >= 2) t = 1; //Store Upgrade;
            t += Time.deltaTime * 0.5f;
            ScannerObject.transform.position = Vector3.Lerp(startPos.position, endPos.position, t);
            if (t >= 1)
            {
                ScannerObject.SetActive(false);
                VisibleCuttingLines();
                break;
            }
            yield return null;
        }
        OnCuttingSystem?.Invoke();
    }

    public void ScanReset()
    {
        foreach (GameObject cuttingLine in cuttingLines)
        {
            Destroy(cuttingLine);
        }
        cuttingLines = new List<GameObject>();
    }

    public void VisibleCuttingLines()
    {
        foreach (GameObject cuttingLine in cuttingLines)
        {
            cuttingLine.SetActive(true);
        }
    }

}
