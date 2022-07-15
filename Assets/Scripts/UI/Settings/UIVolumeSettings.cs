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

    private float DBToSliderVolume(float volume) // TODO: Get a better solution.
    {
        if (volume <= -51.0f)
        {
            return 0.0f;
        }
        else if(volume <= -30.8f)
        {
            return 0.1f;
        }
        else if (volume <= -22.47f)
        {
            return 0.2f;
        }
        else if (volume <= -17.111f)
        {
            return 0.3f;
        }
        else if (volume <= -13.152)
        {
            return 0.4f;
        }
        else if (volume <= -10.015f)
        {
            return 0.5f;
        }
        else if (volume <= -7.415f)
        {
            return 0.6f;
        }
        else if (volume <= -4.991f)
        {
            return 0.7f;
        }
        else if (volume <= -3.26f)
        {
            return 0.8f;
        }
        else if (volume <= -1.381f)
        {
            return 0.9f;
        }
        else
        {
            return 1.0f;
        }
    }
}