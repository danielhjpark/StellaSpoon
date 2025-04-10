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
    [SerializeField] GameObject gravityLimitLine;
    PotUI potUI;
    PotViewportSystem potViewportSystem;

    public int rotatePower = 0;
    private float completeTime;
    private float currentTime;

    private bool isRotate;
    private List<GameObject> potIngredients;
    private Coroutine rotateCoroutine;

    void Awake()
    {
        gravityLimitLine.SetActive(false);
        potUI = this.GetComponent<PotUI>();
        potViewportSystem = this.GetComponent<PotViewportSystem>();
    }

    public void Initialize(BoilingSetting boilingSetting, List<GameObject> potIngredients)
    {
        isRotate = false;
        currentTime = 0;
        completeTime = boilingSetting.cookTime;
        //completeTime = boilingSetting.cookTime;
        this.potIngredients = potIngredients;
    }

    public void OnIncreasePower()
    {
        if (rotatePower < 3) rotatePower++;
        powerText.text = rotatePower.ToString();
        if (rotateCoroutine == null)
        {
            rotateCoroutine = StartCoroutine(AddForceWithRotation());
        }
    }

    public void OnDecreasePower()
    {
        if (rotatePower > 0) rotatePower--;
        powerText.text = rotatePower.ToString();
        if (rotateCoroutine == null)
        {
            rotateCoroutine = StartCoroutine(AddForceWithRotation());
        }
    }

    public IEnumerator StartBoilingSystem()
    {
        yield return new WaitUntil(() => isRotate);
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                yield return StartCoroutine(potViewportSystem.CloseLid());
                break;
            }
            yield return null;
        }

        yield return StartCoroutine(potUI.LinkTimerStart(completeTime));
    }

    IEnumerator AddForceWithRotation()
    {
        potUI.SetActiveFrontButton();
        WaitForSeconds addForceTime = new WaitForSeconds(0.1f);
        gravityLimitLine.SetActive(true);
        float radius = 3f;
        bool applyRight = true;
        isRotate = true;
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
                    rb.useGravity = false;
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

        rotateCoroutine = null;
    }


}
