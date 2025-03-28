using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Diagnostics;

public class PotBoilingSystem : MonoBehaviour
{
    [Header("Button UI Text")]
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] Transform centerPos;

    private float rotatePower = 0;
    
    public bool applyRight = true;
    private float completeTime;
    private float currentTime;

    private bool isRotate;
    private List<GameObject> potIngredients;

    public void Initialize(BoilingSetting boilingSetting, List<GameObject>potIngredients) {
        isRotate = false;
        currentTime = 0;
        completeTime = 20f;
        //completeTime = boilingSetting.cookTime;
        this.potIngredients = potIngredients;
    }

    public void OnIncreasePower()
    {
        if (rotatePower < 3) rotatePower++;
        powerText.text = rotatePower.ToString();
        if (!isRotate) {
            StartCoroutine(AddForceWithRotation());
        }

    }

    public void OnDecreasePower()
    {
        if (rotatePower > 0) rotatePower--;
        powerText.text = rotatePower.ToString();
        if (!isRotate) {
            StartCoroutine(AddForceWithRotation());
        }
    }

    public IEnumerator StartBoilingSystem() {
        Debug.Log("Start");
        yield return new WaitUntil(() => isRotate);
        Debug.Log("end");
    }

    IEnumerator AddForceWithRotation()
    {
        isRotate = true;
        WaitForSeconds addForceTime = new WaitForSeconds(0.1f);
        float radius = 1f;
        while (true)
        {
            if (currentTime >= completeTime)
            {
                break;
            }
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

                    rb.AddForce(finalForce * 5f * rotatePower, ForceMode.Acceleration);
                    //rb.AddForce(forceToCenterOrOutward, ForceMode.Impulse);

                }
            }
            currentTime += 0.1f;
            yield return addForceTime;
        }
    }


}
