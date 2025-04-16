using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingManager : CookManagerBase
{

    private CuttingObjectSystem cuttingObjectSystem;
    private CuttingLineSystem cuttingLine;
    private CuttingMotionSystem cuttingMotionSystem;
    private CuttingAudioSystem cuttingAudioSystem;

    [SerializeField] CookUIManager cookUIManager;

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
    public int horizontalCount; // 원하는 분할 개수
    public int verticalCount;

    [Header("Motion Speed Setting")]
    [SerializeField] float knifeSpeed;
    [SerializeField] float rotateSpeed;

    GameObject targetObject;
    Ingredient currentIngredient;

    void Awake()
    {
        CookManager.instance.BindingManager(this);
    }

    void Start()
    {
        cuttingObjectSystem = GetComponent<CuttingObjectSystem>();
        cuttingLine = GetComponent<CuttingLineSystem>();
        cuttingMotionSystem = GetComponent<CuttingMotionSystem>();
        cuttingAudioSystem = GetComponent<CuttingAudioSystem>();
        //cuttingBoard.OnCuttingSystem += StartCuttingObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CookSceneManager.instance.UnloadScene();
        }
    }
    /// --------------Virtual Method --------------------------//

    public override IEnumerator UseCookingStep()
    {
        horizontalCount = currentMenu.cuttingSetting.cuttingCount;
        //SelectRecipe();
        //yield return AddIngredient();
        //yield return StartCoroutine(DropIngredient());
        cuttingAudioSystem.StartAudioSource(CuttingAudioSystem.AudioType.IngredientScan);
        yield return StartCoroutine(cuttingLine.ScanObject());
        cuttingAudioSystem.StopAudioSource(CuttingAudioSystem.AudioType.IngredientScan);
        switch (currentMenu.cuttingSetting.cuttingType)
        {
            case CuttingType.Horizontal:
                yield return StartCoroutine(CuttingHorizontalSystem());
                break;
            case CuttingType.Quater:
                yield return StartCoroutine(CuttingCubeSystem());
                break;
                // case CuttingMode.Quarter:
                //     break;
        }
        CookCompleteCheck();
    }

    public override void CookCompleteCheck()
    {
        Item trimItem = currentMenu.cuttingSetting.trimItem;
        int trimItemCount = currentMenu.cuttingSetting.trimItemCount;
        Ingredient trimIngredient = IngredientManager.instance.FindIngredient(trimItem.itemName);
        IngredientManager.IngredientAmount[trimIngredient] += trimItemCount;
        RefrigeratorManager.instance.AddItem(trimItem, trimItemCount);

        CookSceneManager.instance.UnloadScene("CuttingBoardMergeTest", currentMenu);
    }

    public override void AddIngredient(GameObject obj, Ingredient ingredient)
    {
        currentIngredient = ingredient;
        targetObject = obj;
        obj.GetComponent<Rigidbody>().useGravity = true;
        obj.GetComponent<Collider>().enabled = true;
        obj.transform.position = dropPos.position;
        StartCoroutine(UseCookingStep());
    }

    //------------------------------------------------------//

    IEnumerator AddIngredient()
    {
        GameObject mainIngredient = Instantiate(currentMenu.mainIngredient.ingredientPrefab, Vector3.zero, Quaternion.identity);
        AddIngredient(mainIngredient, currentMenu.mainIngredient);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator CuttingHorizontalSystem()
    {
        CuttingSetup();
        CreateCuttingLine(horizontalCount);
        cuttingMotionSystem.Initialize(targetObject, cuttingLine.cuttingLines, currentMenu.cuttingSetting);
        yield return StartCoroutine(cuttingMotionSystem.CuttingHorizontal());

        CuttingReset();
        StartCoroutine(cookUIManager.VisiblePanel());
    }

    IEnumerator CuttingCubeSystem()
    {
        CuttingSetup();
        CreateCuttingLine(horizontalCount);
        cuttingMotionSystem.Initialize(targetObject, cuttingLine.cuttingLines, currentMenu.cuttingSetting);
        yield return StartCoroutine(cuttingMotionSystem.CuttingCube());

        CuttingReset();
        StartCoroutine(cookUIManager.VisiblePanel());
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

    private void CreateCuttingLine(int count)
    {
        cuttingLine.ScanReset();
        for (int i = 1; i < count; i++)
        {
            float t = i / (float)count;
            cuttingLine.CalculateCuttingLine(targetObject.GetComponent<Renderer>(), t);
        }
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
