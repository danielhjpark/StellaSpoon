using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class LiquidSystem : MonoBehaviour
{
    [SerializeField] LiquidVolume liquidSystem;
    
    void Start()
    {
        SetLiquidColor();
    }

    void SetLiquidColor() {
        liquidSystem.liquidColor1 = Color.red;
        liquidSystem.liquidColor2 = Color.white;
        liquidSystem.alpha = 0.1f;
    }

    
    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag == "PotIngredient") {
            other.transform.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.transform.tag == "PotIngredient") {
            other.transform.GetComponent<Rigidbody>().useGravity = true;
        }
    }

}
