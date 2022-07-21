using UnityEngine;
using UnityEngine.UI;

public class UIVolumeSettings : MonoBehaviour
{
    [SerializeField] Slider _masterSlider = null;
    [SerializeField] Slider _musicSlider = null;
    [SerializeField] Slider _sfxSlider = null;
    private AudioManager _audioManager = null;

    public void Initialize()
    {
        _audioManager = ServiceLocator.Get<AudioManager>();

        _masterSlider.value = DBToSliderVolume(_audioManager.MasterVolume);
        _musicSlider.value = DBToSliderVolume(_audioManager.MusicVolume);
        _sfxSlider.value = DBToSliderVolume(_audioManager.SFXVolume);

        SetupListeners();
    }

    private void SetupListeners()
    {
        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void SetMasterVolume(float masterValue)
    {
        _audioManager.SetMasterVolume(40 * Mathf.Log10(masterValue));
    }

    private void SetMusicVolume(float musicValue)
    {
        _audioManager.SetMusicVolume(40 * Mathf.Log10(musicValue));
    }

    private void SetSFXVolume(float sfxValue)
    {
        _audioManager.SetSFXVolume(40 * Mathf.Log10(sfxValue));
    }

    private float DBToSliderVolume(float volume)
    {
        return Mathf.Pow(10.0f, volume * 0.025f);
    }
}