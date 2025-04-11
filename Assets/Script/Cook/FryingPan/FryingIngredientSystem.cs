using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FryingIngredientSystem : MonoBehaviour
{
    [Header("Set Objects")]
    [SerializeField] private Transform dropPos;
    [SerializeField] private Transform dropPos2;
    [SerializeField] private GameObject mainIngredientParent;
    [SerializeField] private GameObject subIngredientParent;
    public GameObject fryingMainIngredient;
    public List<IngredientAmount> checkIngredients = new List<IngredientAmount>();
    //---------------------------------------//

    public IEnumerator DropIngredient()
    {
        if (fryingMainIngredient == null) yield break;
        float time = 0;
        foreach (Transform ingredient in fryingMainIngredient.transform)
        {
            ingredient.GetComponent<Rigidbody>().isKinematic = true;
            ingredient.GetComponent<Rigidbody>().useGravity = false;
            ingredient.GetComponent<Collider>().enabled = false;
        }
        
        while (true)
        {
            time += Time.deltaTime * 5f;
            fryingMainIngredient.transform.localPosition = Vector3.Lerp(dropPos.localPosition, new Vector3(0, 0, 0), time);
            if (fryingMainIngredient.transform.localPosition.y <= 0f) break;
            yield return null;
        }
    }

    public void AddMainIngredient(GameObject ingredients, Ingredient ingredient)
    {
        Debug.Log("Main");
        fryingMainIngredient = ingredients;

        ingredients.transform.position = dropPos.position;
        ingredients.transform.SetParent(mainIngredientParent.transform);
        IngredientAddAmount(checkIngredients, ingredient, 1);

        StartCoroutine(DropIngredient());
    }

    public void AddSubIngredient(GameObject ingredients, Ingredient ingredientData)
    {
        Debug.Log("Sub");
        ingredients.transform.position = dropPos2.position;
        ingredients.transform.SetParent(subIngredientParent.transform);
        IngredientAddAmount(checkIngredients, ingredientData, ingredientData.ingredientUseCount);

        foreach (Transform ingredient in ingredients.transform)
        {
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }
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

}
