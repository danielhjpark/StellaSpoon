using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class RestaurantOpenSystem : MonoBehaviour
{
    [SerializeField] private float range = 2.0f; 
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
    [SerializeField] Image pressGagueImage;

    [Header("Door")]
    [SerializeField] Animator doorAnimator;
    private string doorOpenName = "character_nearby";

    private enum signState { Open = 0, Close = 1}
    private bool isOpened; 
    

    void Start() {
        playerTransform = GameObject.FindWithTag("Player").transform;
        isOpened = false;
        signRenderer.material = signMaterial[(int)signState.Close];
    }

    void Update()
    {
        CheckSign();
        CheckOpenRestaurant();
    }

    public void FillGague() {
        pressGagueImage.fillAmount += Time.deltaTime * 0.5f;
    }

    public void ResetGague() {
        pressGagueImage.fillAmount = 0f;
    }

    //------------------------------------------------------//
    private void CheckSign()
    {
        Vector3 rayOrigin = playerTransform.position + Vector3.up * 0.5f; 
        Vector3 rayDirection = playerTransform.forward; 

        if(Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, range, signLayer)) {
      
            OpenAndClosePanel.SetActive(true);
            if(Input.GetKey(KeyCode.F)) {
                FillGague();
            }
            else {
                ResetGague();
            }
        }
        else {
            OpenAndClosePanel.SetActive(false);
            ResetGague();
        }
    }

    private void CheckOpenRestaurant() {
        //int time = gameTimeManager.gameHours;
        int time = 24;
        int openTime = 18;

        if(pressGagueImage.fillAmount >= 1 && !isOpened && time >= openTime) {
            isOpened = true;
            OpenUI.SetActive(false);
            CloseUI.SetActive(true);
            pressGagueImage.fillAmount = 0f;
            signRenderer.material = signMaterial[(int)signState.Open];

            doorAnimator.SetBool(doorOpenName, true);
            OrderManager.instance.OpenRestaurant();
        }
        else if(pressGagueImage.fillAmount >= 1 && isOpened) {
            isOpened = false;
            OpenUI.SetActive(true);
            CloseUI.SetActive(false);
            pressGagueImage.fillAmount = 0f;
            signRenderer.material = signMaterial[(int)signState.Close];

            doorAnimator.SetBool(doorOpenName, false);
            OrderManager.instance.CloseRestaurant();
        }
    }

    private void ReadyOpenSign()
    {
        //actionText.gameObject.SetActive(true);
    }

    private void EarlyOpenSign()
    {
        //actionText.gameObject.SetActive(true);
    }
}
