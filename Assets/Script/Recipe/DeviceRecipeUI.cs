using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DeviceRecipeUI : MonoBehaviour
{

    [SerializeField] private GameObject recipeBase;
    [SerializeField] private GameObject recipeInfoBase;

    [SerializeField] private List<GameObject> deActivatePanels; // ���� ���� �г��� �����ϴ� ����Ʈ
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

        if (Input.GetKeyDown(KeyCode.X)) // �׽�Ʈ �ڵ�
        {
            ToggleAllPanels(false); // ��� �г� ��Ȱ��ȭ
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleSpecificPanel(1, false); // �� ��° �гθ� ��Ȱ��ȭ (�ε��� 1)
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

    private void ToggleAllPanels(bool isActive) // �׽�Ʈ
    {
        foreach (GameObject panel in deActivatePanels)
        {
            panel.SetActive(isActive);
        }
    }

    private void ToggleSpecificPanel(int index, bool isActive) // �׽�Ʈ
    {
        if (index >= 0 && index < deActivatePanels.Count) // ��ȿ�� �ε������� üũ
        {
            deActivatePanels[index].SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("�߸��� �г� �ε���: " + index);
        }
    }

    public void UnlockRecipe()
    {
        
    }
}
