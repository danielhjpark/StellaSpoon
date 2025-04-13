using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPanAudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource FryingAudio;
    [SerializeField] AudioSource PouringSauceAudio;

    public enum AudioType {
        Frying, PouringSauce
    }

    public void StartAudioSource(AudioType audioType)
    {
        switch(audioType) {
            case AudioType.Frying:
                FryingAudio.Play();
                break;
            case AudioType.PouringSauce:
                PouringSauceAudio.Play();
                break;
        }
    }
    public void StopAudioSource(AudioType audioType) {
        switch(audioType) {
            case AudioType.Frying:
                FryingAudio.Stop();
                break;
            case AudioType.PouringSauce:
                PouringSauceAudio.Stop();
                break;
        }
    }

    public void PauseAudioSource(AudioType audioType) {
        switch(audioType) {
            case AudioType.Frying:
                FryingAudio.Pause();
                break;
        }
    }
    public void UnPauseAudioSource(AudioType audioType) {
        switch(audioType) {
            case AudioType.Frying:
                FryingAudio.UnPause();
                break;
        }
    }

}
