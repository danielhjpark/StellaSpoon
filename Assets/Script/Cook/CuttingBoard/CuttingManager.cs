using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingManager : CookManagerBase
{
    private CuttingObjectSystem cuttingObjectSystem;
    private CuttingLineSystem cuttingLineSystem;
    private CuttingMotionSystem cuttingMotionSystem;
    private CuttingAudioSystem cuttingAudioSystem;
    private CuttingBoardUI cuttingBoardUI;

    [SerializeField] CookUIManager cookUIManager;

    [Header("Objects Setting ")]
    [SerializeField] Transform dropPos;
    [SerializeField] Transform parentPos;

    [Header("Mode Setting")]
    public int horizontalCount; // 원하는 분할 개수
    public int verticalCount;

    GameObject targetObject;
    Ingredient currentIngredient;

    void Awake()
    {
        CookManager.instance.BindingManager(this);
    }

    void Start()
    {
        cuttingObjectSystem = GetComponent<CuttingObjectSystem>();
        cuttingLineSystem = GetComponent<CuttingLineSystem>();
        cuttingMotionSystem = GetComponent<CuttingMotionSystem>();
        cuttingAudioSystem = GetComponent<CuttingAudioSystem>();
        cuttingBoardUI = GetComponent<CuttingBoardUI>();

        cookUIManager.SelectRecipeMode();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CookSceneManager.instance.UnloadScene();
        }
    }
    /// --------------Virtual Method --------------------------//

    public override void SelectRecipe(Recipe menu)
    {
        base.SelectRecipe(menu);
        StartCoroutine(cookUIManager.VisiblePanel());
        horizontalCount = currentMenu.cuttingSetting.cuttingCount;
        verticalCount = currentMenu.cuttingSetting.cuttingCount;
    }

    public override IEnumerator UseCookingStep()
    {
        yield return StartCoroutine(DropIngredient());
        yield return StartCoroutine(ScanIngredient());
        yield return StartCoroutine(CuttingIngredient());

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
        // obj.GetComponent<Rigidbody>().useGravity = true;
        // obj.GetComponent<Collider>().enabled = true;
        obj.transform.position = dropPos.position;
        //obj.transform.SetParent(parentPos);

        StartCoroutine(UseCookingStep());
    }

    //------------------------------------------------------//
    public IEnumerator DropIngredient()
    {
        if (targetObject == null)
        {
            Debug.LogError("Not found Ingredient");
            yield break;
        }
        float time = 0;
        targetObject.GetComponent<Rigidbody>().isKinematic = true;
        targetObject.GetComponent<Rigidbody>().useGravity = false;
        //targetObject.GetComponent<Collider>().enabled = false;

        while (true)
        {
            time += Time.deltaTime * 5f;
            targetObject.transform.position = Vector3.Lerp(dropPos.position, parentPos.position, time);
            //if (targetObject.transform.localPosition.y <= 0f) break;
            if (targetObject.transform.position.y <= parentPos.position.y) break;
            yield return null;
        }
        targetObject.GetComponent<Rigidbody>().useGravity = true;
        targetObject.GetComponent<Collider>().enabled = true;

    }

    IEnumerator ScanIngredient()
    {
        cuttingAudioSystem.StartAudioSource(CuttingAudioSystem.AudioType.IngredientScan);
        yield return StartCoroutine(cuttingLineSystem.ScanObject());
        cuttingAudioSystem.StopAudioSource(CuttingAudioSystem.AudioType.IngredientScan);
    }

    IEnumerator CuttingIngredient()
    {
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
    }


    IEnumerator CuttingHorizontalSystem()
    {
        cuttingBoardUI.VisibleSliceUI();

        cuttingLineSystem.CreateCuttingLine(horizontalCount, targetObject);
        cuttingMotionSystem.Initialize(targetObject, currentMenu.cuttingSetting);
        yield return StartCoroutine(cuttingMotionSystem.CuttingHorizontal());

        cuttingBoardUI.HideCuttingBoardUI();
        cuttingBoardUI.VisibleSliceUI();
        //StartCoroutine(cookUIManager.VisiblePanel());
    }

    IEnumerator CuttingCubeSystem()
    {
        cuttingBoardUI.VisibleSliceUI();
        cuttingLineSystem.CreateCuttingLine(horizontalCount, targetObject);
        cuttingMotionSystem.Initialize(targetObject, currentMenu.cuttingSetting);
        //yield return StartCoroutine(cuttingMotionSystem.CuttingCube());
        yield return StartCoroutine(cuttingMotionSystem.CuttingCube2());

        cuttingBoardUI.HideCuttingBoardUI();
        cuttingBoardUI.VisibleSliceUI();
        //StartCoroutine(cookUIManager.VisiblePanel());
    }


}
