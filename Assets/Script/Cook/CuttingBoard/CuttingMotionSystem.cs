using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CuttingMotionSystem : MonoBehaviour
{
    public enum CuttingMode
    {
        Horizontal,
        Cube
    }

    private CuttingObjectSystem cuttingObjectSystem;
    private CuttingLineSystem cuttingLine;


    [Header("Objects Setting ")]
    [SerializeField] Transform dropPos;
    [SerializeField] Transform knifePos;
    [SerializeField] GameObject knifeObject;
    [SerializeField] GameObject knifeOriginalObject;
    [SerializeField] GameObject rotateObject;

    [Header("UI Setting")]
    [SerializeField] GameObject cuttingBoardUI;
    [SerializeField] GameObject sliceUI;
    [SerializeField] GameObject rotateUI;

    [Header("Mode Setting")]
    public CuttingMode cuttingMode;
    public int horizontalCount; // 원하는 분할 개수
    public int verticalCount;

    [Header("Motion Speed Setting")]
    [SerializeField] float knifeSpeed;
    [SerializeField] float rotateSpeed;

    bool isCutting = false;
    GameObject targetObject;
    List<GameObject> cuttingLines;
    Material surfaceMaterial;
    void Start()
    {
        cuttingObjectSystem = GetComponent<CuttingObjectSystem>();
    }

    public void Initialize(GameObject targetObject, List<GameObject> cuttingLines, CuttingSetting cuttingSetting)
    {
        this.horizontalCount = cuttingSetting.cuttingCount;
        this.verticalCount = cuttingSetting.cuttingCount;
        this.targetObject = targetObject;
        this.cuttingLines = cuttingLines;
        surfaceMaterial = cuttingSetting.surfaceMaterial;
    }

    public IEnumerator CuttingHorizontal()
    {
        List<GameObject> sliceAllObjects = cuttingObjectSystem.SliceHorizontal(targetObject, horizontalCount, surfaceMaterial);
        //List<GameObject> cuttingLines = cuttingLine.cuttingLines;
        int sliceCount = sliceAllObjects.Count;
        int currentCount = 0;

        while (currentCount < sliceCount)
        {
            if (currentCount + 1 == sliceCount)
            {
                AddPhysics(sliceAllObjects[currentCount], Vector3.right);
                break;
            }
            if (Input.GetKeyDown(KeyCode.V) && !isCutting)
            {
                AddPhysics(sliceAllObjects[currentCount], Vector3.left);
                StartCoroutine(knifeMotion(cuttingLines[currentCount]));
                if (cuttingLines.Count > currentCount) cuttingLines[currentCount].SetActive(false);
                currentCount++;
            }

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        ObjectReset(sliceAllObjects);
    }


    public IEnumerator CuttingCube()
    {
        List<GameObject> sliceAllObjects = cuttingObjectSystem.SliceHorizontal(targetObject, horizontalCount, surfaceMaterial);
        int sliceCount = sliceAllObjects.Count;
        int currentCount = 0;

        while (currentCount < sliceCount)
        {
            if (currentCount + 1 == sliceCount)
            {
                break;
            }
            if (Input.GetKeyDown(KeyCode.V) && !isCutting)
            {
                StartCoroutine(knifeMotion(cuttingLines[currentCount]));
                if (cuttingLines.Count > currentCount) cuttingLines[currentCount].SetActive(false);
                currentCount++;
            }

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CuttingReset();
        //slice Object Resetting;

        List<GameObject> sliceAllObjects2 = new List<GameObject>();
        foreach (GameObject sliceObject in sliceAllObjects)
        {
            //sliceObject.transform.SetParent(rotateObject.transform);
            
            List<GameObject> sliceVerticals = cuttingObjectSystem.SliceVertical(sliceObject, verticalCount, surfaceMaterial);
            foreach (GameObject sliceVertical in sliceVerticals)
            {
                sliceVertical.transform.localPosition = new Vector3(0, 0, sliceVertical.transform.localPosition.z);
                sliceVertical.transform.rotation = Quaternion.identity;
                sliceAllObjects2.Add(sliceVertical);
            }
        }

        //targetObject.transform.SetParent(rotateObject.transform);

        //Rotation Object
        float t = 0;
        bool isRot = false;
        rotateUI.SetActive(true);
        sliceUI.SetActive(false);
        Quaternion startRot = Quaternion.Euler(-90, 0, 0);
        Quaternion endRot = Quaternion.Euler(-90, 0, 90);
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space)) isRot = true;
            if (!isRot)
            {
                yield return null;
                continue;
            }
            t += Time.deltaTime * rotateSpeed;

            rotateObject.transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            if (t >= 1)
            {
                rotateUI.SetActive(false);
                sliceUI.SetActive(true);
                break;
            }
            yield return null;
        }

        //Vertical Slice;
        //CuttingSetup();
        //CreateCuttingLine(verticalCount);
        sliceCount = verticalCount;
        currentCount = 0;
       // cuttingLines = cuttingLine.cuttingLines;

        while (currentCount < sliceCount)
        {
            if (currentCount + 1 == sliceCount)
            {
                for (int i = 0; i < horizontalCount; i++)
                {
                    AddPhysics(sliceAllObjects2[currentCount + i * verticalCount], Vector3.right);
                }
                break;
            }
            if (Input.GetKeyDown(KeyCode.V) && !isCutting)
            {
                for (int i = 0; i < horizontalCount; i++)
                {
                    AddPhysics(sliceAllObjects2[currentCount + i * verticalCount], Vector3.left);
                }
                StartCoroutine(knifeMotion(cuttingLines[currentCount]));
                if (cuttingLines.Count > currentCount) cuttingLines[currentCount].SetActive(false);
                currentCount++;
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        ObjectReset(sliceAllObjects2);
        //CuttingReset();
        //StartCoroutine(cookUIManager.VisiblePanel());
    }

    IEnumerator knifeMotion(GameObject cuttingLine)
    {
        isCutting = true;
        Renderer cuttingLineRenderer = cuttingLine.GetComponent<Renderer>();

        Vector3 max = cuttingLineRenderer.bounds.max;
        Vector3 min = cuttingLineRenderer.bounds.min;

        Vector3 knifeMaxPos = new Vector3(cuttingLine.transform.position.x, max.y, cuttingLine.transform.position.z);
        Vector3 knifeMinPos = new Vector3(cuttingLine.transform.position.x, min.y, cuttingLine.transform.position.z);

        Quaternion startRotation = Quaternion.Euler(0, 90, -30);
        Quaternion endRotation = Quaternion.Euler(0, 90, 0);

        float t = 0;

        while (true)
        {
            t += Time.deltaTime * knifeSpeed;
            knifeObject.transform.position = Vector3.Lerp(knifeMaxPos, knifeMinPos, t);
            knifeOriginalObject.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            if (t >= 1)
            {
                break;
            }
            yield return null;
        }
        isCutting = false;
    }


    private void AddPhysics(GameObject obj, Vector3 dir)
    {
        if (!obj.TryGetComponent<Rigidbody>(out Rigidbody objRb))
        {
            obj.AddComponent<Rigidbody>();
        }
        obj.GetComponent<Rigidbody>().mass = 100.0f;
        obj.AddComponent<MeshCollider>().convex = true;
        obj.GetComponentInChildren<Rigidbody>().AddForce(dir * 2, ForceMode.VelocityChange);
    }

    private void ObjectReset(List<GameObject> sliceAllObjects)
    {
        foreach (GameObject sliceObject in sliceAllObjects)
        {
            Destroy(sliceObject);
        }
        Destroy(targetObject);
    }

    private void CuttingSetup()
    {
        cuttingBoardUI.SetActive(true);
        sliceUI.SetActive(true);
        knifeObject.SetActive(true);
        knifeObject.transform.position = knifePos.position;
        knifeOriginalObject.transform.localRotation = Quaternion.Euler(0, 90, -30);
    }

    private void CuttingReset()
    {
        knifeObject.SetActive(false);
        cuttingBoardUI.SetActive(false);
        sliceUI.SetActive(false);
        cuttingLine.ScanReset();
        rotateObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
}
