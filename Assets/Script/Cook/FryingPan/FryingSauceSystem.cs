using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingSauceSystem : SauceSystem
{
    [SerializeField] SauceController sauceController;
    [SerializeField] GameObject sauceObject;
    [SerializeField] float minScale, maxScale;
    private float currentScale;
    private float scaleValue;
    private WaitForSeconds sauceDelay = new WaitForSeconds(0.01f);
    
    void Start()
    {
        isLiquidFilled = false;
        isCanFillLiquid = false;
        maxScale = 25;
        minScale = 0;
    }

    public void Initialize(FryingSetting fryingSetting)
    {
        scaleValue = maxScale / fryingSetting.fryingCount;
        this.sauceType = fryingSetting.sauceType;
        this.sauceController.Initialize(sauceType);
        this.SetSauceColor();
    }

    public void InitializeMakeMode(FryingSetting fryingSetting)
    {
        scaleValue = (maxScale /fryingSetting.fryingCount)/2;
        this.sauceController.InitializeMakeMode();
    }

    public void AddSauce()
    {
        sauceController.enabled = true;
    }

    public override IEnumerator StartLiquidLevel()
    {
        currentScale = minScale;
        liquidVolume.gameObject.transform.localScale = new Vector3(0, 1f, 0);
        while (true)
        {
            currentScale += 0.1f;
            liquidVolume.gameObject.transform.localScale = new Vector3(currentScale, 1f, currentScale);
            if (currentScale >= maxScale)
            {
                currentScale = maxScale;
                liquidVolume.gameObject.transform.localScale = new Vector3(maxScale, 1f, maxScale);
                break;
            }

            yield return sauceDelay;
        }
    }

    public IEnumerator UseSauce()
    {
        float targetScale = currentScale - scaleValue;
        while (true)
        {
            currentScale -= 0.1f;
            liquidVolume.gameObject.transform.localScale = new Vector3(currentScale, 1f, currentScale);
            if (currentScale <= targetScale)
            {
                currentScale = targetScale;
                liquidVolume.gameObject.transform.localScale = new Vector3(targetScale, 1f, targetScale);
                break;
            }

            yield return sauceDelay;
        }
    }
}
