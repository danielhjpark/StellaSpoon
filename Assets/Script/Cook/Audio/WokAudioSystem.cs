using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WokAudioSystem : CookAudioSystem<WokAudioSystem.AudioType>
{
    [Header("Wok Frying Sound")]
    [SerializeField] AudioSource wokDefaultAudio;
    [SerializeField] AudioSource wokTossingAudio;

    [Header("Pouring Sauce Sound")]
    [SerializeField] AudioSource pouringSauceAudio;

    [Header("UI")]
    [SerializeField] AudioSource InherentMotionClickAudio;
    [SerializeField] AudioSource FireSlideMoveAudio;

    public enum AudioType
    {
        WokDefault, WokTossing,
        PouringSauce,
        InherentMotionClick, FireSlideMove
    }

    protected override AudioSource CallAudioSource(AudioType audioType)
    {

        AudioSource currentAudioSource;
        switch (audioType)
        {
            case AudioType.WokDefault:
                currentAudioSource = wokDefaultAudio;
                break;
            case AudioType.WokTossing:
                currentAudioSource = wokTossingAudio;
                break;
            case AudioType.PouringSauce:
                currentAudioSource = pouringSauceAudio;
                break;
            default:
                currentAudioSource = null;
                break;
        }
        return currentAudioSource;
    }
}
