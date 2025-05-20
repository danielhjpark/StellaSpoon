using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Rendering;
using Unity.VisualScripting;

public class RestaurantOpenSystem : MonoBehaviour
{
    private float range = 2.0f;
    private Transform playerTransform;

    [Header("GameTime")]
    [SerializeField] GameTimeManager gameTimeManager;

    [Header("Sign")]
    [SerializeField] private Renderer signRenderer;
    [SerializeField] private Material[] signMaterial;
    [SerializeField] private LayerMask signLayer;

    [Header("UI")]
    [SerializeField] private GameObject OpenAndClosePanel;
    [SerializeField] private GameObject OpenUI;
    [SerializeField] private GameObject CloseUI;
    [SerializeField] private Image pressGagueImage;

    [Header("Door")]
    [SerializeField] Animator doorAnimator;
    private string doorOpenName = "character_nearby";

    private enum signState { Open = 0, Close = 1 }

    static public bool isRestaurantOpened;

    int currentTime = 24;
    const int openTime = 18;
    const int closeTime = 22;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        isRestaurantOpened = false;
        signRenderer.material = signMaterial[(int)signState.Close];
    }

    void Update()
    {
        //currentTime = gameTimeManager.gameHours;
        AutoCloseRestaurant();
        CheckSign();
        CheckRestaurant();
    }


    //------------------------------------------------------//
    private bool IsCanInteractSign()
    {

        if (!isRestaurantOpened && currentTime >= openTime && DailyMenuManager.dailyMenuList.Count > 0)
        {
            return true;
        }
        else if (isRestaurantOpened && currentTime >= closeTime && DailyMenuManager.dailyMenuList.Count <= 0 && NpcManager.instance.npcList.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;//early return
        }
    }

    private void CheckSign()
    {
        if (!IsCanInteractSign())
        {
            OpenAndClosePanel.SetActive(false);
            ResetGague();
            return;
        }

        Vector3 rayOrigin = playerTransform.position + Vector3.up * 0.5f;
        Vector3 rayDirection = playerTransform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, range, signLayer))
        {

            OpenAndClosePanel.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                FillGague();
            }
            else
            {
                ResetGague();
            }
        }
        else
        {
            OpenAndClosePanel.SetActive(false);
            ResetGague();
        }
    }

    private void AutoCloseRestaurant()
    {
        if (isRestaurantOpened && currentTime >= closeTime
        && DailyMenuManager.dailyMenuList.Count <= 0
        && NpcManager.instance.npcList.Count <= 0
        && OrderManager.instance.restaurantCoroutine == null)
        {
            isRestaurantOpened = false;
            OpenUI.SetActive(true);
            CloseUI.SetActive(false);
            pressGagueImage.fillAmount = 0f;
            signRenderer.material = signMaterial[(int)signState.Close];

            doorAnimator.SetBool(doorOpenName, false);
        }
    }

    private void CheckRestaurant()
    {
        if (pressGagueImage.fillAmount < 1) return;
        if (!isRestaurantOpened)
        {
            isRestaurantOpened = true;
            OpenUI.SetActive(false);
            CloseUI.SetActive(true);
            pressGagueImage.fillAmount = 0f;
            signRenderer.material = signMaterial[(int)signState.Open];

            doorAnimator.SetBool(doorOpenName, true);
            OrderManager.instance.OpenRestaurant();
        }
        else
        {
            isRestaurantOpened = false;
            OpenUI.SetActive(true);
            CloseUI.SetActive(false);
            pressGagueImage.fillAmount = 0f;
            signRenderer.material = signMaterial[(int)signState.Close];

            doorAnimator.SetBool(doorOpenName, false);
            OrderManager.instance.CloseRestaurant();
        }
    }


    private void FillGague()
    {
        pressGagueImage.fillAmount += Time.deltaTime * 0.5f;
    }

    private void ResetGague()
    {
        pressGagueImage.fillAmount = 0f;
    }

}
