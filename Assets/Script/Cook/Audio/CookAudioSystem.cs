using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookAudioSystem : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClipList;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartAudioSource() {
        audioSource.clip = audioClipList[0];
        if(audioSource.clip != null) {
            audioSource.Play();
        }
    }

}
