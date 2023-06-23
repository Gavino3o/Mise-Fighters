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

    [Range(0f , 1f)]
    public float backgroundMusicVolume = 1.0f;
    [Range(0f, 1f)]
    public float gameVolume = 1.0f;

    [Range(0.1f, 3f)]
    public float backgroundMusicPitch = 1.0f;
    [Range(0.1f, 3f)]
    public float gameVolumePitch = 1.0f;

    public AudioMixerGroup audioMixer;

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
        backgroundMusicAudioSource.volume = backgroundMusicVolume;
        backgroundMusicAudioSource.outputAudioMixerGroup = audioMixer;
        backgroundMusicAudioSource.clip = bgmAudioClips[1]; // This is hardcoded, separate later.


        // Create an array of audio sources for sound effects
        soundEffectAudioSources = new AudioSource[initialSoundEffectAudioSources];
        for (int i = 0; i < soundEffectAudioSources.Length; i++)
        {
            soundEffectAudioSources[i] = gameObject.AddComponent<AudioSource>();
            soundEffectAudioSources[i].playOnAwake = false;
            soundEffectAudioSources[i].volume = gameVolume;
            soundEffectAudioSources[i].outputAudioMixerGroup = audioMixer;
        }
    }

    //This is called in character select.
    private void Start()
    {
        backgroundMusicAudioSource.Play();
        Debug.Log("BGM audio source function called");
        Debug.Log(backgroundMusicAudioSource.isPlaying.ToString());
    }

    [ServerRpc]
    public void ServerPlayBackgroundMusic(int audioClipIndex, bool loop = true)
    {
        ObserversPlayBackgroundMusic(audioClipIndex);
    }

    [ServerRpc]
    public void ServerPlaySoundEffect(int audioClipIndex)
    {
        ObserversPlaySoundEffect(audioClipIndex);
    }

    [ServerRpc]
    public void ServerStopBackgroundMusic()
    {
        ObserversStopBackgroundMusic();
    }


    [ObserversRpc]
    public void ObserversPlayBackgroundMusic(int audioClipIndex, bool loop = true)
    {
        /*if (backgroundMusicAudioSource.isPlaying)
        {
            ObserversStopBackgroundMusic();
        }*/

        backgroundMusicAudioSource.clip = bgmAudioClips[audioClipIndex];
        backgroundMusicAudioSource.loop = loop;
        backgroundMusicAudioSource.Play();
    }

    public void ObserversPlayBackgroundMusic(AudioClip audioClip, bool loop = true)
    {
        int index = System.Array.IndexOf(bgmAudioClips, audioClip);
        if (index == -1)
        {
            Debug.Log("BGM Audio Clip chosen is missing.");
            return;
        }
        ObserversPlayBackgroundMusic(index, loop);
    }

    [ObserversRpc]
    public void ObserversStopBackgroundMusic()
    {
        backgroundMusicAudioSource.Stop();
    }

    [ObserversRpc]
    public void ObserversPlaySoundEffect(int audioClipIndex)
    {
        foreach (AudioSource audioSource in soundEffectAudioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = effectAudioClips[audioClipIndex];
                audioSource.Play();
                return;
            }
        }

        // If all audio sources are in use, create a new one and play the sound effect
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = effectAudioClips[audioClipIndex];
        newAudioSource.playOnAwake = false;
        newAudioSource.volume = gameVolume;
        newAudioSource.outputAudioMixerGroup = audioMixer;
        newAudioSource.Play();
    }

    public void ObserversPlaySoundEffect(AudioClip audioClip)
    {
        int index = System.Array.IndexOf(effectAudioClips, audioClip);
        if (index == -1)
        {
            Debug.Log("Audio clip chosen is missing.");
            return;
        }
        ObserversPlaySoundEffect(index);
    }

}