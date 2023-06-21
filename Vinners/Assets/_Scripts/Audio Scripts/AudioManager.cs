using FishNet.Object;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : NetworkBehaviour
{
    public static AudioManager Instance;

    // You can define additional properties or variables as per your needs.

    private AudioSource backgroundMusicAudioSource;
    [SerializeField] private AudioClip[] bgmAudioClips;
    private AudioSource[] soundEffectAudioSources;
    [SerializeField] private AudioClip[] effectAudioClips;
    private int initialSoundEffectAudioSources = 5;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        // Create an audio source for background music
        backgroundMusicAudioSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicAudioSource.clip = bgmAudioClips[0]; // This is hardcoded, separate later.
        backgroundMusicAudioSource.Play();

        // Create an array of audio sources for sound effects
        soundEffectAudioSources = new AudioSource[initialSoundEffectAudioSources];
        for (int i = 0; i < soundEffectAudioSources.Length; i++)
        {
            soundEffectAudioSources[i] = gameObject.AddComponent<AudioSource>();
            soundEffectAudioSources[i].playOnAwake = false;
        }
    }

    [ServerRpc]
    public void ServerPlayBackgroundMusic(int audioClipIndex, bool loop = true, float volume = 1f, float pitch = 1f)
    {
        ObserversPlayBackgroundMusic(audioClipIndex);
    }

    [ServerRpc]
    public void ServerPlaySoundEffect(int audioClipIndex, float volume = 1f, float pitch = 1f)
    {
        ObserversPlaySoundEffect(audioClipIndex);
    }

    [ServerRpc]
    public void ServerStopBackgroundMusic()
    {
        ObserversStopBackgroundMusic();
    }


    [ObserversRpc]
    public void ObserversPlayBackgroundMusic(int audioClipIndex, bool loop = true, float volume = 1f, float pitch = 1f)
    {
        backgroundMusicAudioSource.clip = bgmAudioClips[audioClipIndex];
        backgroundMusicAudioSource.loop = loop;
        backgroundMusicAudioSource.volume = volume;
        backgroundMusicAudioSource.pitch = pitch;
        backgroundMusicAudioSource.Play();
    }

    public void ObserversPlayAudio(AudioClip audioClip, bool loop = true, float volume = 1f, float pitch = 1f)
    {
        int index = System.Array.IndexOf(bgmAudioClips, audioClip);
        if (index == -1)
        {
            Debug.Log("BGM Audio Clip chosen is missing.");
            return;
        }
        ObserversPlayBackgroundMusic(index, loop, volume, pitch);
    }

    [ObserversRpc]
    public void ObserversStopBackgroundMusic()
    {
        backgroundMusicAudioSource.Stop();
    }

    [ObserversRpc]
    public void ObserversPlaySoundEffect(int audioClipIndex, float volume = 1f, float pitch = 1f)
    {
        foreach (AudioSource audioSource in soundEffectAudioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = effectAudioClips[audioClipIndex];
                audioSource.volume = volume;
                audioSource.pitch = pitch;
                audioSource.Play();
                return;
            }
        }

        // If all audio sources are in use, create a new one and play the sound effect
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = effectAudioClips[audioClipIndex];
        newAudioSource.volume = volume;
        newAudioSource.Play();
    }

    public void ObserversPlaySoundEffect(AudioClip audioClip, float volume = 1f, float pitch = 1f)
    {
        int index = System.Array.IndexOf(effectAudioClips, audioClip);
        if (index == -1)
        {
            Debug.Log("Audio clip chosen is missing.");
            return;
        }
        ObserversPlaySoundEffect(index, volume, pitch);
    }
}