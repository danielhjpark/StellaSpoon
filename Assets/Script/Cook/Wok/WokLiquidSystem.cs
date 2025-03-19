using System.Collections;
using System.Collections.Generic;
using LiquidVolumeFX;
using UnityEngine;

public class WokLiquidSystem : LiquidSystem
{
    [SerializeField, Range(0f, 1f)] float maxRange;
    [SerializeField, Range(0f, 1f)] float minRange;

    int totalLevel;
    int currentLevel;
    bool isCanFillLiquid;

    void Start()
    {
        liquidVolume.level = 0;
        isCanFillLiquid = false;
    }

    public void Initialize(int totalLevel) {
        isCanFillLiquid = true;
        this.totalLevel = totalLevel;
    }

    void Update() {
        UpdateLiquidLevel();
    }

    void UpdateLiquidLevel() {
        if(!isCanFillLiquid) return;

        float range = maxRange - minRange;
        float levelValue = range / totalLevel;
        liquidVolume.level = maxRange - (currentLevel * levelValue);
    }

    public void IncreaseLiquidLevel() {currentLevel++;}

}
