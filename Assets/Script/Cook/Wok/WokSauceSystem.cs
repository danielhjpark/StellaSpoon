using System.Collections;
using System.Collections.Generic;
using LiquidVolumeFX;
using Unity.VisualScripting;
using UnityEngine;

public class WokSauceSystem : SauceSystem
{
    [SerializeField] SauceController sauceController;
    private WaitForSeconds sauceDelay = new WaitForSeconds(0.01f);

    private float totalLevel;
    private float currentLevel;
    private float levelValue;

    void Start()
    {
        isLiquidFilled = false;
        isCanFillLiquid = false;
    }

    public void Initialize(TossingSetting tossingSetting) {
        this.totalLevel = tossingSetting.tossingCount;
        this.sauceType = tossingSetting.sauceType;
        this.sauceController.Initialize(sauceType);
        this.SetSauceColor();
        this.InitLevel();
    }
    
    public void InitializeMakeMode(TossingSetting tossingSetting) {
        if(tossingSetting.tossingCount == 0) this.totalLevel = 2;
        else this.totalLevel = tossingSetting.tossingCount;
        this.sauceController.InitializeMakeMode();
        this.InitLevel();
    }

    private void InitLevel() {
        levelValue = (maxRange - minRange) /(totalLevel * 2);
        currentLevel = maxRange;
        liquidVolume.level = 0;
    }
    

    public void AddSauce() {
        sauceController.enabled = true;
    }

    public IEnumerator UseSauce()
    {
        float targetLevel = currentLevel - levelValue;
        while (true)
        {
            currentLevel -= 0.005f;
            liquidVolume.level = currentLevel;
            if (currentLevel <= targetLevel)
            {
                currentLevel = targetLevel;
                liquidVolume.level = currentLevel;
                break;
            }

            yield return sauceDelay;
        }
    }

}
