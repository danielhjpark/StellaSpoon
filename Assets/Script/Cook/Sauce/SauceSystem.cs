using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;
using System;

public class SauceSystem : MonoBehaviour
{
    [SerializeField] public LiquidVolume liquidVolume;
    [SerializeField, Range(0f, 1f)] protected float maxRange;
    [SerializeField, Range(0f, 1f)] protected float minRange;

    public SauceType sauceType;
    public bool startLiquidFilled;
    protected bool isCanFillLiquid;
    protected bool isLiquidFilled;

    public void SetSauceColor(SauceType sauceType)
    {
        this.sauceType = sauceType;
        SetSauceColor();
    }

    public void SetSauceColor()
    {
        liquidVolume.alpha = 0.1f;
        switch (sauceType)
        {
            case SauceType.Brown:
                liquidVolume.liquidColor1 = new Color32(159, 100, 0, 255);
                liquidVolume.liquidScale1 = 4.85f;
                liquidVolume.liquidColor2 = new Color32(255, 125, 0, 255);
                liquidVolume.liquidScale2 = 4.85f;
                break;
            case SauceType.Red:
                liquidVolume.liquidColor1 = Color.red;
                liquidVolume.liquidColor2 = Color.red;
                break;
            case SauceType.White:
                liquidVolume.liquidColor1 = Color.white;
                liquidVolume.liquidColor2 = Color.white;
                break;
        }

    }

    public LiquidVolume GetLiquidVolume()
    {
        return liquidVolume;
    }


    public virtual IEnumerator StartLiquidLevel()
    {
        float levelValue = 0.005f;
        while (true)
        {
            if (liquidVolume.level >= maxRange)
            {
                liquidVolume.level = maxRange;
                break;
            }
            liquidVolume.level += levelValue;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void IsLiquidFilled(bool isLiquidFilled) { this.isLiquidFilled = isLiquidFilled; }
    public bool IsLiquidFilled() { return isLiquidFilled; }
}
