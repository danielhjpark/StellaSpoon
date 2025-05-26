using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DeviceRecipeUI : MonoBehaviour
{

    [SerializeField] private GameObject recipeBase;
    [SerializeField] private GameObject recipeInfoBase;

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

    public void OpenRecipe()
    {
        if (recipeActivated) return;

        recipeBase.SetActive(true);

        recipeActivated = true;
        SoundManager.instance.PlaySound(SoundManager.Display.Display_Menu_Button);
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

}
