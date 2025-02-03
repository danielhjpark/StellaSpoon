using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PotManager : MonoBehaviour
{

    [SerializeField] Transform dropPos;
    [SerializeField] Transform centerPos;
    public float power;
    public float radius;
    List<GameObject> potIngredients = new List<GameObject>();
    public bool applyRight = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           StartCoroutine(AddForceWithRotation());
        }
    }

    public void LocateIngredient(GameObject obj) {
       // obj.GetComponent<Rigidbody>().useGravity = true;
        //obj.GetComponent<MeshCollider>().enabled = true;
        obj.transform.position = dropPos.position;
        AddIngredient(obj);
    }

    public void AddIngredient(GameObject ingredients) {
        foreach(Transform ingredient in ingredients.transform) {
            potIngredients.Add(ingredient.gameObject);
            ingredient.GetComponent<Rigidbody>().useGravity = true;
            ingredient.GetComponent<Collider>().enabled = true;
        }
        //potIngredient.transform.SetParent(dropIngredient.transform);
        StartCoroutine(CookManager.instance.cookUIManager.VisiblePanel());
    }

    IEnumerator AddForceWithRotation() {
        while (true) {
            foreach (GameObject obj in potIngredients)
            {
                if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    Vector3 center = centerPos.position; 
                    Vector3 position = obj.transform.position; 
                    Vector3 direction = position - center; 
                    
                    float distance = direction.magnitude; 
                    Vector3 normal = direction.normalized;

                    Vector3 forceDirection = applyRight 
                        ? Vector3.Cross(normal, Vector3.up).normalized  // 시계방향
                        : -Vector3.Cross(normal, Vector3.up).normalized; // 반시계방향
                    Vector3 forceToCenterOrOutward = (distance > radius * 0.3f) 
                        ? -direction.normalized  // 바깥 방향
                        : direction.normalized; // 중심 방향

                    Vector3 finalForce = (forceDirection) + (forceToCenterOrOutward * 3f);
                    
                    rb.AddForce(finalForce * 5f * power, ForceMode.Acceleration);
                    //rb.AddForce(forceToCenterOrOutward, ForceMode.Impulse);

                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

}
