using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingAudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource IngredientScanAudio;
    [SerializeField] AudioSource KnifeMotionAudio;


    public enum AudioType {
        IngredientScan, KnifeMotion
    }

    public void StartAudioSource(AudioType audioType)
    {
        switch(audioType) {
            case AudioType.IngredientScan:
                IngredientScanAudio.Play();
                break;
            case AudioType.KnifeMotion:
                KnifeMotionAudio.Play();
                break;
        }
    }
    public void StopAudioSource(AudioType audioType) {
        switch(audioType) {
            case AudioType.IngredientScan:
                IngredientScanAudio.Stop();
                break;
            case AudioType.KnifeMotion:
                KnifeMotionAudio.Stop();
                break;
        }
    }

    public void PauseAudioSource(AudioType audioType) {
        switch(audioType) {
            case AudioType.IngredientScan:
                IngredientScanAudio.Stop();
                break;
            case AudioType.KnifeMotion:
                KnifeMotionAudio.Stop();
                break;
        }
    }
}
