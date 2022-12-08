using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsUI : MonoBehaviour
{
    [SerializeField] Slider _masterSlider = null;
    [SerializeField] Slider _musicSlider = null;
    [SerializeField] Slider _sfxSlider = null;
    [SerializeField] Slider _ambientSlider = null;
    [SerializeField] Slider _uiSfxSlider = null;
    [SerializeField] TextMeshProUGUI _masterText = null;
    [SerializeField] TextMeshProUGUI _musicText = null;
    [SerializeField] TextMeshProUGUI _sfxText = null;
    [SerializeField] TextMeshProUGUI _ambientText = null;
    [SerializeField] TextMeshProUGUI _uiSfxText = null;
    private AudioManager _audioManager = null;

    public void Initialize()
    {
        _audioManager = ServiceLocator.Get<AudioManager>();

        _masterSlider.value = DBToSliderVolume(_audioManager.MasterVolume);
        _masterText.text = SliderToTextVal(_masterSlider);
        _musicSlider.value = DBToSliderVolume(_audioManager.MusicVolume);
        _musicText.text = SliderToTextVal(_musicSlider);
        _sfxSlider.value = DBToSliderVolume(_audioManager.SFXVolume);
        _sfxText.text = SliderToTextVal(_sfxSlider);
        _ambientSlider.value = DBToSliderVolume(_audioManager.AmbientVolume);
        _ambientText.text = SliderToTextVal(_ambientSlider);
        _uiSfxSlider.value = DBToSliderVolume(_audioManager.UISFXVolume);
        _uiSfxText.text = SliderToTextVal(_uiSfxSlider);

        SetupListeners();
    }

    private void SetupListeners()
    {
        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
        _uiSfxSlider.onValueChanged.AddListener(SetUISFXVolume);
    }

    private void SetMasterVolume(float masterValue)
    {
        _audioManager.SetMasterVolume(40 * Mathf.Log10(masterValue));
        _masterText.text = SliderToTextVal(_masterSlider);
    }

    private void SetMusicVolume(float musicValue)
    {
        _audioManager.SetMusicVolume(40 * Mathf.Log10(musicValue));
        _musicText.text = SliderToTextVal(_musicSlider);
    }

    private void SetSFXVolume(float sfxValue)
    {
        _audioManager.SetSFXVolume(40 * Mathf.Log10(sfxValue));
        _sfxText.text = SliderToTextVal(_sfxSlider);
    }

    private void SetAmbientVolume(float ambientValue)
    {
        _audioManager.SetAmbientVolume(40 * Mathf.Log10(ambientValue));
        _ambientText.text = SliderToTextVal(_ambientSlider);
    }

    private void SetUISFXVolume(float uiSfxValue)
    {
        _audioManager.SetUISFXVolume(40 * Mathf.Log10(uiSfxValue));
        _uiSfxText.text = SliderToTextVal(_uiSfxSlider);
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