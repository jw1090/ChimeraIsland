using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetMusicVolume(float musicValue)
    {
        mixer.SetFloat(MIXER_MUSIC,Mathf.Log10(musicValue)*20);
    }

    void SetSFXVolume(float sfxValue)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(sfxValue) * 20);
    }
}
