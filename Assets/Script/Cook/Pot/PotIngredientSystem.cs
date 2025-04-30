using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PotIngredientSystem : MonoBehaviour
{
    PotAudioSystem potAudioSystem;

    [Header("Transform Objects")]
    [SerializeField] Transform dropPos;
    [SerializeField] GameObject dropIngredient;

    private Ingredient mainIngredient;
    private List<GameObject> potIngredients = new List<GameObject>();
    private List<IngredientAmount> checkIngredients = new List<IngredientAmount>();

    // Start is called before the first frame update
    void Start()
    {
        potAudioSystem = GetComponent<PotAudioSystem>();
    }

    public void AddIngredient(GameObject obj, Ingredient ingredient)
    {
        obj.transform.position = dropPos.position;
        AddIngredientList(obj);
        //StartCoroutine(cookUIManager.VisiblePanel());

        if (ingredient.ingredientType == IngredientType.Main)
        {
            mainIngredient = ingredient;
            potAudioSystem.StartAudioSource(PotAudioSystem.AudioType.MainIngredientDrop);
            //IngredientAddAmount(checkIngredients, ingredient, 1);
            return;
        }
        if (ingredient.ingredientType == IngredientType.Sub)
        {
            IngredientAddAmount(checkIngredients, ingredient, ingredient.ingredientUseCount);
            potAudioSystem.StartAudioSource(PotAudioSystem.AudioType.SubIngredientDrop);
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

    public void AddIngredientList(GameObject ingredients)
    {
        ingredients.transform.SetParent(dropIngredient.transform);
        foreach (Transform ingredient in ingredients.transform)
        {
            potIngredients.Add(ingredient.gameObject);
            ingredient.GetComponent<Rigidbody>().mass = 10;
            ingredient.GetComponent<Rigidbody>().isKinematic = false;
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }
    }

}
