using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager instance;

    [SerializeField] public int currentPanLevel = 1;
    [SerializeField] public int currentWorLevel = 1;
    [SerializeField] public int currentCuttingBoardLevel = 1;
    [SerializeField] public int currentPotLevel = 1;

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
