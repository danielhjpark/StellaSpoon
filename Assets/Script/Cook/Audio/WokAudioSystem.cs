using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WokAudioSystem : MonoBehaviour
{
    [Header("Wok Frying Sound")]
    [SerializeField] AudioSource wokDefaultAudio;
    [SerializeField] AudioSource wokTossingAudio;

    [Header("Pouring Sauce Sound")]
    [SerializeField] AudioSource pouringSauceAudio;

    [Header("UI")]
    [SerializeField] AudioClip InherentMotionClickAudio;
    [SerializeField] AudioClip FireSlideMoveAudio;

    [SerializeField]AudioSource audioSource;

    public enum AudioType {
        WokDefault, WokTossing, 
        PouringSauce,
        InherentMotionClick, FireSlideMove
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartAudioSource(AudioType audioType) {
        switch(audioType) {
            case AudioType.WokDefault:
                wokDefaultAudio.Play();
                break;
            case AudioType.WokTossing:
                wokTossingAudio.Play();
                break;
            case AudioType.PouringSauce:
                pouringSauceAudio.Play();
                break;
            default:
            break;
        }
        
    }
    
    public void StopAudioSource() {
        if(audioSource.clip != null) {
            audioSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
