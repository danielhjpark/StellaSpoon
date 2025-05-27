using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceRecipeInfo : MonoBehaviour
{
    [SerializeField] GameObject recipeIngredient;
    [SerializeField] GameObject recipeDescription;

    // Update is called once per frame
    void Update()
    {

    }

    //Button
    public void OpenRecipeIngredient()
    {
        recipeIngredient.SetActive(true);
        recipeDescription.SetActive(false);
    }

    public void OpenRecipeDescription()
    {
        recipeIngredient.SetActive(false);
        recipeDescription.SetActive(true);
    }
    
}
