using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class WokTossingSystem : MonoBehaviour
{
    //"Wok System"
    private WokSauceSystem wokSauceSystem;
    private WokIngredientSystem wokIngredientSystem;
    private WokAudioSystem wokAudioSystem;

    [Header("TimeLine")]
    [SerializeField] PlayableDirector wokTimeLine;
    [Header("Wok Rotate Object")]
    [SerializeField] GameObject wokObject;
    [SerializeField] Transform wokCenter;

    [Header("UI Objects")]
    [SerializeField] WokUI wokUI;

    private List<GameObject> wokIngredients = new List<GameObject>();
    private int successTossingCount;
    private bool isTossing = false;

    void Start()
    {
        wokSauceSystem = GetComponent<WokSauceSystem>();
        wokIngredientSystem = GetComponent<WokIngredientSystem>();
        wokAudioSystem = GetComponent<WokAudioSystem>();
        wokUI.OnWokSystem += CheckTossing;
    }

    public void BindTossingObject(List<GameObject> wokIngredients)
    {

        this.wokIngredients = wokIngredients;
        successTossingCount = 0;
    }

    public IEnumerator WokTossing(int tossingCount, System.Action<int> callback)
    {
        if (tossingCount <= 0)
        {
            callback(successTossingCount);
            yield break;
        }
        wokUI.OnWokUI();
        wokTimeLine.time = 0;
        bool isStartTimeLine = false;
        bool isAddForce = false;
        bool isUseSauce = false;

        Coroutine wokUIMark = StartCoroutine(wokUI.MoveMark());
        Coroutine wokRotate = StartCoroutine(RotateWok());
        wokAudioSystem.StartAudioSource(WokAudioSystem.AudioType.WokDefault);
        while (true)
        {
            if (wokTimeLine.time >= wokTimeLine.duration) break;
            else if (wokUI.IsCheckEnd()) break;

            if (Input.GetKeyDown(KeyCode.V))
            {
                if (isTossing && !isStartTimeLine)
                {
                    isStartTimeLine = true;
                    successTossingCount++;
                    wokTimeLine.Play();
                    wokAudioSystem.StartAudioSource(WokAudioSystem.AudioType.WokTossing);
                    AddForceForwardIngredient();
                    StopCoroutine(wokUIMark);
                }
                else
                {
                    StopCoroutine(wokUIMark);
                    break;
                }
            }

            //Add Motion
            if (!isAddForce && wokTimeLine.time >= wokTimeLine.duration * 0.45)
            {
                isAddForce = true;
                isUseSauce = true;
                AddForceUpIngredient();
                StartCoroutine(wokSauceSystem.UseSauce());
                StopCoroutine(wokRotate);
                wokIngredientSystem.ApplyIngredientShader();
            }
            yield return null;
        }
        if (!isUseSauce)
        {
            wokIngredientSystem.ApplyIngredientShader();
            yield return StartCoroutine(wokSauceSystem.UseSauce());
            //StopCoroutine(wokRotate);
        }
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(WokTossing(--tossingCount, callback));
    }

    IEnumerator RotateWok()
    {
        float radius = 1f;    // 회전 반경
        float speed = 180f;    // 회전 속도 (도/초)
        float angle = 0f;    // 현재 각도

        while (true)
        {
            angle += speed * Time.deltaTime; // 회전 속도 적용
            if (angle >= 360f) angle -= 360f;

            // 각도를 방향벡터로 변환
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

            // 중심점 + 방향벡터 * 반지름으로 위치 갱신
            wokObject.transform.position = wokCenter.position + dir * 0.1f * radius;

            yield return null;
        }
    }

    private void AddForceForwardIngredient()
    {
        foreach (GameObject ingredient in wokIngredients)
        {
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.forward * 1, ForceMode.VelocityChange);
        }
    }

    private void AddForceUpIngredient()
    {
        foreach (GameObject ingredient in wokIngredients)
        {
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.VelocityChange);
            ingredient.GetComponent<Rigidbody>().AddForce(Vector3.back * 1, ForceMode.VelocityChange);
        }
    }

    void CheckTossing(bool isTossing)
    {
        this.isTossing = isTossing;
    }


}
