using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingAudioSystem : CookAudioSystem<CuttingAudioSystem.AudioType>
{
    [SerializeField] AudioSource IngredientScanAudio;
    [SerializeField] AudioSource KnifeMotionAudio;


    public enum AudioType
    {
        IngredientScan, KnifeMotion
    }

    protected override AudioSource CallAudioSource(AudioType audioType)
    {
        AudioSource currentAudioSource;
        switch (audioType)
        {
            case AudioType.IngredientScan:
                currentAudioSource = IngredientScanAudio;
                break;
            case AudioType.KnifeMotion:
                currentAudioSource = KnifeMotionAudio;
                break;
            default:
                currentAudioSource = null;
                break;
        }
        return currentAudioSource;
    }

}
