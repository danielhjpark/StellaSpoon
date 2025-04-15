using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPanAudioSystem : CookAudioSystem<FryingPanAudioSystem.AudioType>
{
    [SerializeField] AudioSource FryingAudio;
    [SerializeField] AudioSource PouringSauceAudio;

    public enum AudioType
    {
        Frying, PouringSauce
    }

    protected override AudioSource CallAudioSource(AudioType audioType)
    {
        AudioSource currentAudioSource;
        switch (audioType)
        {
            case AudioType.Frying:
                currentAudioSource = FryingAudio;
                break;
            case AudioType.PouringSauce:
                currentAudioSource = PouringSauceAudio;
                break;
            default:
                currentAudioSource = null;
                break;
        }
        return currentAudioSource;
    }

}
