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
        if (instance == null) instance = this;
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

    }
}
