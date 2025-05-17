using System.Collections;
using System.Collections.Generic;
using LiquidVolumeFX;
using Unity.VisualScripting;
using UnityEngine;

public class WokSauceSystem : SauceSystem
{
    [SerializeField] SauceController sauceController;
    private WokAudioSystem wokAudioSystem;

    private WaitForSeconds sauceDelay = new WaitForSeconds(0.01f);
    private float totalLevel;
    private float currentLevel;
    private float levelValue;

    void Start()
    {
        wokAudioSystem = GetComponent<WokAudioSystem>();
        isLiquidFilled = false;
        isCanFillLiquid = false;
    }

    public void Initialize(TossingSetting tossingSetting)
    {
        this.totalLevel = tossingSetting.secondTossingCount;
        this.sauceType = tossingSetting.sauceType;
        this.sauceController.Initialize(sauceType);
        this.SetSauceColor();
        this.InitLevel();
    }

    public void InitializeMakeMode(TossingSetting tossingSetting)
    {
        if (tossingSetting.secondTossingCount == 0) this.totalLevel = 2;
        else this.totalLevel = tossingSetting.secondTossingCount;
        this.sauceController.InitializeMakeMode();
        this.InitLevel();
    }

    private void InitLevel()
    {
        levelValue = (maxRange - minRange) / (totalLevel * 2);
        currentLevel = maxRange;
        liquidVolume.level = 0;
    }


    public void AddSauce()
    {
        sauceController.enabled = true;
    }

    public IEnumerator UseSauce()
    {
        if (sauceType == SauceType.None) yield break;
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

    public override IEnumerator StartLiquidLevel()
    {
        wokAudioSystem.StartAudioSource(WokAudioSystem.AudioType.PouringSauce);
        yield return base.StartLiquidLevel();
        wokAudioSystem.StopAudioSource(WokAudioSystem.AudioType.PouringSauce);
    }

}
