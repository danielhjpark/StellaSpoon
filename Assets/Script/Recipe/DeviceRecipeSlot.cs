using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DeviceRecipeSlot : MonoBehaviour, IPointerClickHandler
{
    GameObject recipeInfo;
    [SerializeField] Image recipeImage;
    Image DeactiveImage;

    public event Action OnSelectRecipe;

    public void Initialize(GameObject recipeInfo, Recipe recipe)
    {
        this.recipeInfo = recipeInfo;
        recipeImage.sprite = recipe.menuImage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelectRecipe?.Invoke();
        recipeInfo.SetActive(true);
    }
}
