using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotAudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource mainIngredientDropAudio;
    [SerializeField] AudioSource subIngredientDropAudio;
    [SerializeField] AudioSource rotaitionPotAudio;
    [SerializeField] AudioSource putPotLidAudio;

    [SerializeField] AudioSource pouringSauceAudio;
    [Header("UI")]
    [SerializeField] AudioSource view; 

    public enum AudioType {
        MainIngredientDrop, SubIngredientDrop, RotaitionPot, PutPotLid, PouringSauce
    }

    public void StartAudioSource(AudioType audioType)
    {
        switch(audioType) {
            case AudioType.MainIngredientDrop:
                mainIngredientDropAudio.Play();
                break;
            case AudioType.SubIngredientDrop:
                subIngredientDropAudio.Play();
                break;
            case AudioType.RotaitionPot:
                rotaitionPotAudio.Play();
                break;
            case AudioType.PutPotLid:
                putPotLidAudio.Play();
                break;
            case AudioType.PouringSauce:
                pouringSauceAudio.Play();
                break;
        }
    }
    public void StopAudioSource(AudioType audioType) {
        switch(audioType) {
            case AudioType.MainIngredientDrop:
                mainIngredientDropAudio.Stop();
                break;
            case AudioType.SubIngredientDrop:
                subIngredientDropAudio.Stop();
                break;
            case AudioType.RotaitionPot:
                rotaitionPotAudio.Stop();
                break;
            case AudioType.PutPotLid:
                putPotLidAudio.Stop();
                break;
            case AudioType.PouringSauce:
                pouringSauceAudio.Stop();
                break;
        }
    }

    public void PauseAudioSource(AudioType audioType) {
        switch(audioType) {
            case AudioType.MainIngredientDrop:
                mainIngredientDropAudio.Pause();
                break;
            case AudioType.SubIngredientDrop:
                subIngredientDropAudio.Pause();
                break;
            case AudioType.RotaitionPot:
                rotaitionPotAudio.Pause();
                break;
            case AudioType.PutPotLid:
                putPotLidAudio.Pause();
                break;
            case AudioType.PouringSauce:
                pouringSauceAudio.Pause();
                break;
        }
    }

    public void UnPauseAudioSource(AudioType audioType) {
        switch(audioType) {
            case AudioType.MainIngredientDrop:
                mainIngredientDropAudio.UnPause();
                break;
            case AudioType.SubIngredientDrop:
                subIngredientDropAudio.UnPause();
                break;
            case AudioType.RotaitionPot:
                rotaitionPotAudio.UnPause();
                break;
            case AudioType.PutPotLid:
                putPotLidAudio.UnPause();
                break;
            case AudioType.PouringSauce:
                pouringSauceAudio.UnPause();
                break;
        }
    }
}
