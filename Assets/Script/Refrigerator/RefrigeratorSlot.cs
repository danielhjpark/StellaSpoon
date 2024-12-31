using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RefrigeratorSlot : MonoBehaviour
{
    bool isEmpty;
    [SerializeField] Image ingredientImage;
    [SerializeField] TextMeshProUGUI ingredientName;
    Ingredient currentIngredient;

    void Start()
    {
        isEmpty = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckRequireIngredient() {
        
    }

    public void AddIngredient(Ingredient ingredient, int ingredientAmount) {
        currentIngredient = ingredient;
        ingredientImage.sprite = ingredient.ingredientImage;
        ingredientName.text = ingredient.ingredientName;
        isEmpty = false;
    }

    public void BindingIngredient(Ingredient ingredient) {
        currentIngredient = ingredient;
        ingredientImage.sprite = ingredient.ingredientImage;
        ingredientName.text = ingredient.ingredientName;
        isEmpty = false;
    }

    public Ingredient GetIngredient() {
        return currentIngredient;
    }

    public bool IsEmpty() {
        return isEmpty;
    }

    public void SlotClear() {
        ingredientImage.sprite = null;
        ingredientName.text = null;
        isEmpty = true;
    }
}
