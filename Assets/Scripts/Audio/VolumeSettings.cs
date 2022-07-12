using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    public const string MIXER_MASTER = "Master";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, masterSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
    }

    void SetMasterVolume(float masterValue)
    {
        mixer.SetFloat(MIXER_MASTER, (-80 + masterValue * 100));
    }

    void SetMusicVolume(float musicValue)
    {
        mixer.SetFloat(MIXER_MUSIC, (-80 + musicValue * 100));
    }

    void SetSFXVolume(float sfxValue)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(sfxValue) * 20);
    }
    
}
