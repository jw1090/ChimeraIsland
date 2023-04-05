using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsUI : MonoBehaviour
{
    [SerializeField] VolumeSliderUI _masterSlider = null;
    [SerializeField] VolumeSliderUI _musicSlider = null;
    [SerializeField] VolumeSliderUI _sfxSlider = null;
    [SerializeField] VolumeSliderUI _ambientSlider = null;
    [SerializeField] VolumeSliderUI _uiSfxSlider = null;
    private AudioManager _audioManager = null;

    public void Initialize()
    {
        _audioManager = ServiceLocator.Get<AudioManager>();

        _masterSlider.Slider.value = DBToSliderVolume(_audioManager.MasterVolume);
        _masterSlider.Text.text = SliderToTextVal(_masterSlider.Slider);

        _musicSlider.Slider.value = DBToSliderVolume(_audioManager.MusicVolume);
        _musicSlider.Text.text = SliderToTextVal(_musicSlider.Slider);

        _sfxSlider.Slider.value = DBToSliderVolume(_audioManager.SFXVolume);
        _sfxSlider.Text.text = SliderToTextVal(_sfxSlider.Slider);

        _ambientSlider.Slider.value = DBToSliderVolume(_audioManager.AmbientVolume);
        _ambientSlider.Text.text = SliderToTextVal(_ambientSlider.Slider);

        _uiSfxSlider.Slider.value = DBToSliderVolume(_audioManager.UISFXVolume);
        _uiSfxSlider.Text.text = SliderToTextVal(_uiSfxSlider.Slider);

        SetupListeners();
    }

    private void SetupListeners()
    {
        _masterSlider.Slider.onValueChanged.AddListener(SetMasterVolume);
        _musicSlider.Slider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.Slider.onValueChanged.AddListener(SetSFXVolume);
        _ambientSlider.Slider.onValueChanged.AddListener(SetAmbientVolume);
        _uiSfxSlider.Slider.onValueChanged.AddListener(SetUISFXVolume);
    }

    private void SetMasterVolume(float masterValue)
    {
        _audioManager.SetMasterVolume(40 * Mathf.Log10(masterValue));
        _masterSlider.Text.text = SliderToTextVal(_masterSlider.Slider);
    }

    private void SetMusicVolume(float musicValue)
    {
        _audioManager.SetMusicVolume(40 * Mathf.Log10(musicValue));
        _musicSlider.Text.text = SliderToTextVal(_musicSlider.Slider);
    }

    private void SetSFXVolume(float sfxValue)
    {
        _audioManager.SetSFXVolume(40 * Mathf.Log10(sfxValue));
        _sfxSlider.Text.text = SliderToTextVal(_sfxSlider.Slider);
    }

    private void SetAmbientVolume(float ambientValue)
    {
        _audioManager.SetAmbientVolume(40 * Mathf.Log10(ambientValue));
        _ambientSlider.Text.text = SliderToTextVal(_ambientSlider.Slider);
    }

    private void SetUISFXVolume(float uiSfxValue)
    {
        _audioManager.SetUISFXVolume(40 * Mathf.Log10(uiSfxValue));
        _uiSfxSlider.Text.text = SliderToTextVal(_uiSfxSlider.Slider);
    }

    private float DBToSliderVolume(float volume)
    {
        return Mathf.Pow(10.0f, volume * 0.025f);
    }

    public string SliderToTextVal(Slider slider)
    {
        int num = (int)((slider.value - slider.minValue) / (slider.maxValue - slider.minValue) * 100);
        return num.ToString();
    }
}