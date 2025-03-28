using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotSauceSystem : SauceSystem
{
    [SerializeField] SauceController sauceController;
    void Start()
    {
        isLiquidFilled = false;
        isCanFillLiquid = false;
        liquidVolume.level = 0;
        
    }

    public void Initialize(BoilingSetting boilingSetting)
    {
        this.sauceType = boilingSetting.sauceType;
        this.sauceController.Initialize(sauceType);
        this.SetSauceColor();
    }

    public void InitializeMakeMode()
    {
        this.sauceController.InitializeMakeMode();
    }

    public void AddSauce()
    {
        sauceController.enabled = true;
    }

    Color targetColor1;
    Color targetColor2;

    public void SetTargetColor()
    {
        liquidVolume.alpha = 0.1f;
        liquidVolume.liquidScale1 = 4.85f;
        liquidVolume.liquidScale2 = 4.85f;
        //liquidVolume.liquidColor1.WithAlpha
        switch (sauceType)
        {
            case SauceType.Brown:
                targetColor1 = new Color32(159, 100, 0, 255);
                targetColor2 = new Color32(255, 125, 0, 255);
                break;
            case SauceType.Red:
                targetColor1 = Color.red;
                targetColor2 = Color.red;
                break;
            case SauceType.White:
                targetColor1 = Color.white;
                targetColor2 = Color.white;
                break;
        }
    }


    public override IEnumerator StartLiquidLevel()
    {
        SetTargetColor();
        targetColor1.a = 0;
        targetColor2.a = 0;
        liquidVolume.level = 0.6f;
        while (true)
        {
            targetColor1.a += 0.005f;
            targetColor2.a += 0.005f;
            liquidVolume.liquidColor1 = targetColor1;
            liquidVolume.liquidColor2 = targetColor2;

            if (targetColor1.a >= 0.99f)
            {
                targetColor1.a = 1;
                liquidVolume.liquidColor1 = targetColor1;
                targetColor2.a = 1;
                liquidVolume.liquidColor2 = targetColor2;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void IncreaseLiquidLevel() { currentLevel++; }
}
