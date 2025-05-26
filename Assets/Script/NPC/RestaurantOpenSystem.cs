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

    int currentTime;

    const int openTime = 18;
    const int lateOpenTime = 21;
    const int closeTime = 22;
    const int forceCloseTime = 0;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        isRestaurantOpened = false;
        signRenderer.material = signMaterial[(int)signState.Close];
    }

    void Update()
    {
        currentTime = gameTimeManager.gameHours;
        AutoCloseRestaurant();
        CheckSign();
        CheckRestaurant();
        CheckDoorAnimation();
    }


    //------------------------------------------------------//
    private bool IsCanInteractSign()
    {
        //오픈이 안되어 있고 오픈 시간 보다 이전일 때,
        if (!isRestaurantOpened && currentTime < openTime)
        {
            InteractUIManger.instance.UsingText(InteractUIManger.TextType.Open);
            return false;
        }
        //오픈이 안되어 있고 21시가 넘었을 때,
        else if (!isRestaurantOpened && currentTime >= lateOpenTime && currentTime <= 23)
        {
            InteractUIManger.instance.UsingText(InteractUIManger.TextType.Open3);
            return false;
        }
        // 오픈이 안되어 있고 당일메뉴가 없을 때,
        else if (!isRestaurantOpened && currentTime >= openTime && DailyMenuManager.dailyMenuList.Count <= 0)
        {
            InteractUIManger.instance.UsingText(InteractUIManger.TextType.Open2);
            return false;
        }
        //오픈된 상태이며, 클로즈 시간보다 이전일 때,
        else if (isRestaurantOpened && openTime < currentTime && currentTime < closeTime && DailyMenuManager.dailyMenuList.Count > 0 && NpcManager.instance.npcList.Count > 0)
        {
            InteractUIManger.instance.UsingText(InteractUIManger.TextType.Close);
            return false;
        }
        

        if (!isRestaurantOpened && currentTime >= openTime && DailyMenuManager.dailyMenuList.Count > 0)
        {
            return true;
        }
        else if (isRestaurantOpened && currentTime >= closeTime /*&& DailyMenuManager.dailyMenuList.Count <= 0 && NpcManager.instance.npcList.Count <= 0*/)
        {
            return true;
        }
        else return false;


    }

    private void CheckSign()
    {
        Vector3 rayOrigin = playerTransform.position + Vector3.up * 0.5f;
        Vector3 rayDirection = playerTransform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, range, signLayer))
        {
            if (!IsCanInteractSign())
            {
                OpenAndClosePanel.SetActive(false);
                ResetGague();
                return;
            }
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
        if (isRestaurantOpened /*&& currentTime >= closeTime*/
        && DailyMenuManager.dailyMenuList.Count <= 0
        && NpcManager.instance.npcList.Count <= 0
        && OrderManager.instance.restaurantCoroutine == null)
        {
            isRestaurantOpened = false;
            OpenUI.SetActive(true);
            CloseUI.SetActive(false);
            pressGagueImage.fillAmount = 0f;
            signRenderer.material = signMaterial[(int)signState.Close];

            InteractUIManger.instance.UsingText(InteractUIManger.TextType.Ingredient);
            OrderManager.instance.CloseRestaurant();

        }

        else if (isRestaurantOpened && currentTime < openTime && currentTime >= forceCloseTime)
        {
            isRestaurantOpened = false;
            OpenUI.SetActive(true);
            CloseUI.SetActive(false);
            pressGagueImage.fillAmount = 0f;
            signRenderer.material = signMaterial[(int)signState.Close];

            InteractUIManger.instance.UsingText(InteractUIManger.TextType.Close2);
            OrderManager.instance.CloseRestaurant();
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

            OrderManager.instance.OpenRestaurant();
        }
        else
        {
            isRestaurantOpened = false;
            OpenUI.SetActive(true);
            CloseUI.SetActive(false);
            pressGagueImage.fillAmount = 0f;
            signRenderer.material = signMaterial[(int)signState.Close];

            OrderManager.instance.CloseRestaurant();
        }


    }


    private void CheckDoorAnimation()
    {
        if (isRestaurantOpened)
        {
            doorAnimator.SetBool(doorOpenName, true);
        }
        else if (!isRestaurantOpened && NpcManager.instance.npcList.Count <= 0)
        {
            doorAnimator.SetBool(doorOpenName, false);
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
