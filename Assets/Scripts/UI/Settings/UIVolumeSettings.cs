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

        _masterSlider.value = _audioManager.MasterVolume;
        _musicSlider.value = _audioManager.MusicVolume;
        _sfxSlider.value = _audioManager.SFXVolume;

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
        _audioManager.SetMasterVolume(Mathf.Log10(masterValue) * 20);
    }

    private void SetMusicVolume(float musicValue)
    {
        _audioManager.SetMusicVolume(Mathf.Log10(musicValue) * 20);
    }

    private void SetSFXVolume(float sfxValue)
    {
        _audioManager.SetSFXVolume(Mathf.Log10(sfxValue) * 20);
    }
}