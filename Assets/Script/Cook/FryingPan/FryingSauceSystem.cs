using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingSauceSystem : SauceSystem
{
    [SerializeField] SauceController sauceController;
    [SerializeField] GameObject sauceObject;
    Vector3 scale = new Vector3(20, 2, 20);
    void Start()
    {
        //sauceController.enabled = false;
        isLiquidFilled = false;
        liquidVolume.level = 0;
        isCanFillLiquid = false;
    }

    public void Initialize(FryingSetting fryingSetting)
    {
        this.sauceType = fryingSetting.sauceType;
        this.sauceController.Initialize(sauceType);
        this.SetSauceColor();
    }

    public void InitializeMakeMode()
    {
        this.sauceController.InitializeMakeMode();
    }


    void Update()
    {
        //UpdateLiquidLevel();
    }

    public void AddSauce()
    {
        sauceController.enabled = true;

    }

    public override IEnumerator StartLiquidLevel()
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
    public void IncreaseLiquidLevel() { currentLevel++; }
}
