using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingSauceSystem : SauceSystem
{
    [SerializeField] SauceController sauceController;
    [SerializeField] GameObject sauceObject;
    float minScale, maxScale;
    private float currentScale;
    private float scaleValue;
    private WaitForSeconds sauceDelay = new WaitForSeconds(0.01f);
    private FryingPanAudioSystem fryingPanAudioSystem;

    void Start()
    {
        fryingPanAudioSystem = GetComponent<FryingPanAudioSystem>();
        isLiquidFilled = false;
        isCanFillLiquid = false;
        startLiquidFilled = false;
        maxScale = 25;
        minScale = 0;
        sauceType = SauceType.None;
    }

    public void Initialize(FryingSetting fryingSetting)
    {
        scaleValue = maxScale / (fryingSetting.secondFryingCount * 2);
        this.sauceType = fryingSetting.sauceType;
        this.sauceController.Initialize(sauceType);
        this.SetSauceColor();
    }

    public void InitializeMakeMode(int fryingCount)
    {
        scaleValue = maxScale / (fryingCount * 2);
        this.sauceController.InitializeMakeMode();
    }

    public void SetSauceColor(SauceType sauceType)
    {
        this.sauceType = sauceType;
        Debug.Log("SAUCE1");
        SetSauceColor();
    }

    public void AddSauce()
    {
        sauceController.enabled = true;
    }

    public override IEnumerator StartLiquidLevel()
    {
        currentScale = minScale;
        liquidVolume.gameObject.transform.localScale = new Vector3(0, 1f, 0);
        fryingPanAudioSystem.StartAudioSource(FryingPanAudioSystem.AudioType.PouringSauce);
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
        fryingPanAudioSystem.StopAudioSource(FryingPanAudioSystem.AudioType.PouringSauce);
    }

    public IEnumerator UseSauce()
    {
        float targetScale = currentScale - scaleValue;
        if (this.sauceType == SauceType.None) yield break;
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
