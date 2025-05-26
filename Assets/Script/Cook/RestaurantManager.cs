using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager instance;

    [SerializeField] public int currentPanLevel;
    [SerializeField] public int currentWorLevel;
    [SerializeField] public int currentCuttingBoardLevel;
    [SerializeField] public int currentPotLevel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    void Upgrade()
    {

    }

    void StoreInitialize()
    {

    }

    void Update()
    {
        currentPanLevel = StoreUIManager.currentPanLevel;
        currentWorLevel = StoreUIManager.currentWorLevel;
        currentCuttingBoardLevel = StoreUIManager.currentCuttingBoardLevel;
        currentPotLevel = StoreUIManager.currentPotLevel;
    }
}
