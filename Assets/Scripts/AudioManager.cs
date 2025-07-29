using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    
    [Header("Sound Effects")]
    public AudioClip wheelSpinSound;
    public AudioClip correctMatchSound;
    public AudioClip wrongMatchSound;
    public AudioClip gameOverSound;
    public AudioClip backgroundMusic;
    
    [Header("Settings")]
    public float sfxVolume = 0.7f;
    public float musicVolume = 0.3f;
    
    private static AudioManager instance;
    
    public static AudioManager Instance
    {
        get { return instance; }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        SetupAudio();
        PlayBackgroundMusic();
    }
    
    void SetupAudio()
    {
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
        
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
            musicSource.loop = true;
        }
    }
    
    public void PlayWheelSpin()
    {
        PlaySFX(wheelSpinSound);
    }
    
    public void PlayCorrectMatch()
    {
        PlaySFX(correctMatchSound);
    }
    
    public void PlayWrongMatch()
    {
        PlaySFX(wrongMatchSound);
    }
    
    public void PlayGameOver()
    {
        PlaySFX(gameOverSound);
    }
    
    public void PlayBackgroundMusic()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }
    
    void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }
}