using System.Collections;
using System.Collections.Generic;
using LiquidVolumeFX;
using Unity.VisualScripting;
using UnityEngine;

public class WokSauceSystem : SauceSystem
{
    [SerializeField] SauceController sauceController;
    void Start()
    {
        //sauceController.enabled = false;
        isLiquidFilled = false;
        liquidVolume.level = 0;
        isCanFillLiquid = false;
    }

    public void Initialize(TossingSetting tossingSetting) {
        
        this.totalLevel = tossingSetting.tossingCount;
        this.sauceType = tossingSetting.sauceType;
        this.sauceController.Initialize(sauceType);
        this.SetSauceColor();
    }
    
    public void InitializeMakeMode() {
        this.sauceController.InitializeMakeMode();
    }
    

    void Update() {
        //UpdateLiquidLevel();
    }

    public void AddSauce() {
        sauceController.enabled = true;
        
    }

    public void IncreaseLiquidLevel() {currentLevel++;}

}
