using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotAudioSystem : CookAudioSystem<PotAudioSystem.AudioType>
{
    [Header("Main Sound")]
    [SerializeField] AudioSource mainIngredientDropAudio;
    [SerializeField] AudioSource subIngredientDropAudio;
    [SerializeField] AudioSource rotaitionPotAudio;
    [SerializeField] AudioSource putPotLidAudio;

    [SerializeField] AudioSource pouringSauceAudio;
    [Header("UI")]
    [SerializeField] AudioSource view;

    public enum AudioType
    {
        MainIngredientDrop, SubIngredientDrop, RotaitionPot, PutPotLid, PouringSauce
    }

    protected override AudioSource CallAudioSource(AudioType audioType)
    {
        AudioSource currentAudioSource;
        switch (audioType)
        {
            case AudioType.MainIngredientDrop:
                currentAudioSource = mainIngredientDropAudio;
                break;
            case AudioType.SubIngredientDrop:
                currentAudioSource = subIngredientDropAudio;
                break;
            case AudioType.RotaitionPot:
                currentAudioSource = rotaitionPotAudio;
                break;
            case AudioType.PutPotLid:
                currentAudioSource = putPotLidAudio;
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
