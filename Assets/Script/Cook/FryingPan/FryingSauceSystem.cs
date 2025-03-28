using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingSauceSystem : SauceSystem
{
    [SerializeField] SauceController sauceController;
    [SerializeField] GameObject sauceObject;
    [SerializeField] float minScale, maxScale;
    
    void Start()
    {
        isLiquidFilled = false;
        isCanFillLiquid = false;
        maxScale = 20;
        minScale = 0;
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

    public void AddSauce()
    {
        sauceController.enabled = true;
    }

    public override IEnumerator StartLiquidLevel()
    {
        float currentScale = minScale;
        liquidVolume.gameObject.transform.localScale = new Vector3(0, 0.2f, 0);
        while (true)
        {
            currentScale += 0.1f;
            liquidVolume.gameObject.transform.localScale = new Vector3(currentScale, 0.2f, currentScale);
            if (currentScale >= maxScale)
            {
                currentScale = maxScale;
                liquidVolume.gameObject.transform.localScale = new Vector3(maxScale, 0.2f, maxScale);
                break;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void DecreaseLiquidLevel()
    {

    }

    public void IncreaseLiquidLevel() { currentLevel++; }
}
