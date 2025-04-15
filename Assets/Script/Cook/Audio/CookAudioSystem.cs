using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CookAudioSystem<T> : MonoBehaviour where T : Enum
{
    protected abstract AudioSource CallAudioSource(T audioType);

    public void StartAudioSource(T audioType)
    {
        CallAudioSource(audioType).Play();
    }

    public void StopAudioSource(T audioType)
    {
        CallAudioSource(audioType).Stop();
    }

    public void PauseAudioSource(T audioType)
    {
        CallAudioSource(audioType).Pause();
    }

    public void UnPauseAudioSource(T audioType)
    {
        CallAudioSource(audioType).UnPause();
    }
}
