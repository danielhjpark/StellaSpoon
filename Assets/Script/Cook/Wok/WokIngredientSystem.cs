using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

public class WokIngredientSystem : MonoBehaviour
{
    [SerializeField] private Transform dropPos;
    [SerializeField] private GameObject dropIngredient;
    [NonSerialized] public GameObject mainIngredient;
    public List<GameObject> wokIngredients = new List<GameObject>();
    public List<IngredientAmount> checkIngredients = new List<IngredientAmount>();
    private List<IngredientShader> mainIngredientShaders = new List<IngredientShader>();
    public bool isShader = false;


    public void AddMainIngredient(GameObject ingredients, Ingredient ingredient, int totalTossingCount)
    {
        AddIngredientList(ingredients);
        IngredientAddAmount(checkIngredients, ingredient, 1);
        //InitializeIngredientShader(ingredients, totalTossingCount);
        InitializeIngredientSlope(ingredients);
        mainIngredient = ingredients;
    }

    public void AddSubIngredient(GameObject ingredients, Ingredient ingredientData)
    {
        IngredientAddAmount(checkIngredients, ingredientData, ingredientData.ingredientUseCount);
        AddIngredientList(ingredients);
        InitializeIngredientSlope(ingredients);
    }


    void IngredientAddAmount(List<IngredientAmount> list, Ingredient ingredient, int count)
    {
        var existing = list.FirstOrDefault(i => i.ingredient.Equals(ingredient));
        if (existing != null)
        {
            existing.amount += count;
        }
        else
        {
            list.Add(new IngredientAmount(ingredient, count));
        }
    }

    //----------------------------------------------------------------------//
    public void AddIngredientList(GameObject ingredients)
    {
        ingredients.transform.SetParent(dropIngredient.transform, false);
        ingredients.transform.localPosition = Vector3.zero;
        //ingredients.transform.position = dropPos.position;
        foreach (Transform ingredient in ingredients.transform)
        {
            wokIngredients.Add(ingredient.gameObject);
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }

    }

    public void InitializeIngredientShader(GameObject mainIngredientParent, int totalTossingCount)
    {
        foreach (Transform mainIngredient in mainIngredientParent.transform)
        {
            IngredientShader currentShader;
            if (mainIngredient.TryGetComponent<IngredientShader>(out currentShader))
            {
                currentShader = mainIngredient.GetComponent<IngredientShader>();
                currentShader.Initialize(totalTossingCount + 1);
                mainIngredientShaders.Add(currentShader);
                isShader = true;
            }
            else
            {
                isShader = false;
                return;
            }

        }
    }
    private void InitializeIngredientSlope(GameObject mainIngredientParent)
    {
        foreach (Transform mainIngredient in mainIngredientParent.transform)
        {
            IngredientSlope currentSlope;
            if (mainIngredient.TryGetComponent<IngredientSlope>(out currentSlope))
            {
                currentSlope = mainIngredient.GetComponent<IngredientSlope>();
                currentSlope.Initialize(dropIngredient.transform);
            }
        }
    }
    public void ApplyIngredientShader()
    {
        if (!isShader) return;
        foreach (IngredientShader mainIngredientShader in mainIngredientShaders)
        {
            mainIngredientShader.ApplyShaderAlpha();
        }
    }

}
