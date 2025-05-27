using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DeviceRecipeSlot : MonoBehaviour
{
    [SerializeField] Image recipeImage;
    [SerializeField] GameObject deactiveImage;
    GameObject recipeInfo;
    [SerializeField] GameObject recipeIngredient;
    [SerializeField] GameObject recipeDescription;
    Recipe currentRecipe;
    public event Action OnSelectRecipe;

    public void Initialize(GameObject recipeInfo, Recipe recipe)
    {
        this.currentRecipe = recipe;
        this.recipeInfo = recipeInfo;
        recipeImage.sprite = recipe.menuImage;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRecipe != null && RecipeManager.instance.RecipeUnlockCheck[currentRecipe])
        {
            deactiveImage.SetActive(false);
        }
        else
        {
            deactiveImage.SetActive(true);
        }
    }

    public void OnClick()
    {
        OnSelectRecipe?.Invoke();
        recipeInfo.SetActive(true);
        SoundManager.instance.PlaySound(SoundManager.Display.Receipe_Select);
    }
}
