using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Configurações de Áudio")]
    public AudioMixer audioMixer;
    public AudioSource musicSource;
    
    [Header("UI Elements")]
    public Slider volumeSlider;
    public Toggle musicToggle;
    
    private const string MusicVolumeKey = "MusicVolume";
    private const string MusicEnabledKey = "MusicEnabled";
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.playOnAwake = true;
        }
        
        LoadAudioSettings();
    }
    
    void Start()
    {
        UpdateUI();
    }
    
    public void SetMusicVolume(float volume)
    {
        float mixerVolume = Mathf.Log10(volume) * 20;
        
        if (volume <= 0.001f) //  
            mixerVolume = -80f;
        
        if (audioMixer != null)
            audioMixer.SetFloat("MusicVolume", mixerVolume);
        
        if (musicSource != null)
            musicSource.volume = volume;
        
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }
    
    public void ToggleMusic(bool isOn)
    {
        if (musicSource != null)
        {
            if (isOn)
            {
                musicSource.Play();
               
                float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.7f);
                SetMusicVolume(savedVolume);
            }
            else
            {
                musicSource.Pause();
               
                PlayerPrefs.SetInt(MusicEnabledKey, 0);
            }
        }
        
        PlayerPrefs.SetInt(MusicEnabledKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void LoadAudioSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.7f);
        
       
        int musicEnabled = PlayerPrefs.GetInt(MusicEnabledKey, 1);
        bool isMusicEnabled = musicEnabled == 1;
        
        SetMusicVolume(savedVolume);
        
        if (musicSource != null)
        {
            if (isMusicEnabled && !musicSource.isPlaying)
                musicSource.Play();
            else if (!isMusicEnabled && musicSource.isPlaying)
                musicSource.Pause();
        }
    }
    
    private void UpdateUI()
    {
    
        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.7f);
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        
       
        if (musicToggle != null)
        {
            int musicEnabled = PlayerPrefs.GetInt(MusicEnabledKey, 1);
            musicToggle.isOn = musicEnabled == 1;
            musicToggle.onValueChanged.AddListener(ToggleMusic);
        }
    }
    
   
  
}