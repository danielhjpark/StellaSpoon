using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [SerializeField] AudioClip interactAudio;

    public void PlayAudio()
    {
        InteractUIManger.instance.interactAudioSource.clip = interactAudio;
        InteractUIManger.instance.interactAudioSource.Play();
    }

    public void StopAudio()
    {
        InteractUIManger.instance.interactAudioSource.Stop();
    }


}
