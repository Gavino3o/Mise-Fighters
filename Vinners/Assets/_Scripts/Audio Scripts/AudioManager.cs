using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System;

public class AudioManager : NetworkBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    public AudioClip[] audioClips;

    private void Awake()
    {
        instance = this;
    }

    [ObserversRpc]
    public void ObserverPlayAudio(int audioIndex)
    {
        if (audioIndex < 0 || audioIndex >= audioClips.Length)
        {
            Debug.Log("Audio index out of bounds");
            return;
        }
        audioSource.PlayOneShot(audioClips[audioIndex]);
    }

    public void ObserversPlayAudio(AudioClip audioClip)
    {
        int index = System.Array.IndexOf(audioClips, audioClip);
        if (index == -1)
        {
            Debug.Log("Audio clip chosen is missing.");
            return;
        }
        ObserverPlayAudio(index);
    }
}