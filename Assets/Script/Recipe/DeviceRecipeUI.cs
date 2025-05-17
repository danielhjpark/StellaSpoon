using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DeviceRecipeUI : MonoBehaviour
{

    [SerializeField] private GameObject recipeBase;
    [SerializeField] private GameObject recipeInfoBase;

    [SerializeField] private List<GameObject> deActivatePanels; // 여러 개의 패널을 관리하는 리스트
    [SerializeField] private List<GameObject> recipeInfos = new List<GameObject>();
    [SerializeField] private List<GameObject> recipeSlots = new List<GameObject>();
    
    public static bool recipeActivated;

    private void Start()
    {
        foreach (Transform recipeSlot in recipeBase.transform) recipeSlots.Add(recipeSlot.gameObject);
        foreach (Transform recipeInfo in recipeInfoBase.transform) recipeInfos.Add(recipeInfo.gameObject);

        int count = 0;
        foreach (KeyValuePair<string, Recipe> Recipe in RecipeManager.instance.RecipeList) {
            DeviceRecipeSlot currentRecipeSlot = recipeSlots[count].gameObject.GetComponent<DeviceRecipeSlot>();
            GameObject recipeInfo = recipeInfos.FirstOrDefault(objects => objects.gameObject.name == Recipe.Key);
            if (recipeInfo == null || currentRecipeSlot == null) continue;
            currentRecipeSlot.Initialize(recipeInfo, Recipe.Value);
            currentRecipeSlot.OnSelectRecipe += CloseRecipe;
            count++;
        }

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.X)) // 테스트 코드
        {
            ToggleAllPanels(false); // 모든 패널 비활성화
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleSpecificPanel(1, false); // 두 번째 패널만 비활성화 (인덱스 1)
        }
    }

    public void OpenRecipe()
    {
        if (recipeActivated) return;

        recipeBase.SetActive(true);
        
        recipeActivated = true;
    }

    public void CloseRecipe()
    {
        recipeBase.SetActive(false);
        recipeInfoBase.SetActive(true);
        recipeActivated = false;
        foreach (GameObject recipeInfo in recipeInfos)
        {
            recipeInfo.gameObject.SetActive(false);
        }
    }

    private void ToggleAllPanels(bool isActive) // 테스트
    {
        foreach (GameObject panel in deActivatePanels)
        {
            panel.SetActive(isActive);
        }
    }

    private void ToggleSpecificPanel(int index, bool isActive) // 테스트
    {
        if (index >= 0 && index < deActivatePanels.Count) // 유효한 인덱스인지 체크
        {
            deActivatePanels[index].SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("잘못된 패널 인덱스: " + index);
        }
    }

    public void UnlockRecipe()
    {
        
    }
}
