using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class LiquidSystem : MonoBehaviour
{
    enum SauceColor {Brown, Red, White};
    [SerializeField] public LiquidVolume liquidVolume;
    [SerializeField] SauceColor sauceColor;

    private void Update() {
        SetLiquidColor();
    }

    void SetLiquidColor() {
        liquidVolume.alpha = 0.1f;
        switch(sauceColor) {
            case SauceColor.Brown:
                liquidVolume.liquidColor1 = new Color32(159, 100, 0 , 255);
                liquidVolume.liquidScale1 = 4.85f;
                liquidVolume.liquidColor2 = new Color32(255, 125, 0 , 255);
                liquidVolume.liquidScale2 = 4.85f;
                break;
            case SauceColor.Red:
                liquidVolume.liquidColor1 = Color.red;
                liquidVolume.liquidColor2 = Color.red;
                break;
            case SauceColor.White:
                liquidVolume.liquidColor1 = Color.white;
                liquidVolume.liquidColor2 = Color.white;
                break;
        }
        
    }

    public LiquidVolume GetLiquidVolume() {
        return liquidVolume;
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
