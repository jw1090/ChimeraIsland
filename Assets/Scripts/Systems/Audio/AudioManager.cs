using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer _mixer = null;

    [Header("Source")]
    [SerializeField] private AudioSource _musicSource = null;
    [SerializeField] private AudioSource _sfxSource = null;

    [Header("Music")]
    [SerializeField] private AudioClip _mainMenuMusic = null;
    [SerializeField] private AudioClip _stonePlainsMusic = null;
    [SerializeField] private AudioClip _treeOfLifeMusic = null;
    [SerializeField] private AudioClip _ashlandsMusic = null;

    [Header("UI SFX")]
    [SerializeField] private AudioClip _standardClickSFX= null;
    [SerializeField] private AudioClip _confirmClickSFX= null;
    [SerializeField] private AudioClip _purchaseClickSFX= null;
    [SerializeField] private AudioClip _placeChimeraSFX = null;
    [SerializeField] private AudioClip _removeChimeraSFX= null;
    [SerializeField] private AudioClip _evolutionSFX= null;
    [SerializeField] private AudioClip _levelUpSFX= null;

    private UIManager _uIManager = null;
    private PersistentData _persistentData = null;
    private float _masterVolume = 0.0f;
    private float _musicVolume = 0.0f;
    private float _sfxVolume = 0.0f;

    public Vector3 Volumes { get => new Vector3(_masterVolume, _musicVolume, _sfxVolume); }
    public float MasterVolume { get => _masterVolume; }
    public float MusicVolume { get => _musicVolume; }
    public float SFXVolume { get => _sfxVolume; }

    public void SetMasterVolume(float masterVolume)
    {
        _masterVolume = masterVolume;
        _mixer.SetFloat(GameConsts.AudioMixerKeys.MASTER, _masterVolume);
    }

    public void SetMusicVolume(float musicVolume)
    {
        _musicVolume = musicVolume;
        _mixer.SetFloat(GameConsts.AudioMixerKeys.MUSIC, _musicVolume);
    }

    public void SetSFXVolume(float sfxVolume)
    {
        _sfxVolume = sfxVolume;
        _mixer.SetFloat(GameConsts.AudioMixerKeys.SFX, _sfxVolume);
    }

    public AudioManager Initialize()
    {
        _persistentData = ServiceLocator.Get<PersistentData>();
        _uIManager = ServiceLocator.Get<UIManager>();

        _masterVolume = _persistentData.Volumes.x;
        _musicVolume = _persistentData.Volumes.y;
        _sfxVolume = _persistentData.Volumes.z;

        _mixer.SetFloat(GameConsts.AudioMixerKeys.MASTER, _masterVolume);
        _mixer.SetFloat(GameConsts.AudioMixerKeys.MUSIC, _musicVolume);
        _mixer.SetFloat(GameConsts.AudioMixerKeys.SFX, _sfxVolume);

        SetupAudioListeners();

        return this;
    }

    public void SetupAudioListeners()
    {
        CreateButtonListener(_uIManager.MainMenuUI.NewGameButton, PlayClickSFX);
        CreateButtonListener(_uIManager.MainMenuUI.LoadGameButton, PlayClickSFX);
        CreateButtonListener(_uIManager.HabitatUI.MainMenuButton, PlayClickSFX);
        CreateButtonListener(_uIManager.HabitatUI.WorldMapButton, PlayClickSFX);
        CreateButtonListener(_uIManager.HabitatUI.QuitGameButton, PlayClickSFX);
        CreateButtonListener(_uIManager.WorldMapUI.StonePlainsButton, PlayClickSFX);
        CreateButtonListener(_uIManager.WorldMapUI.TreeOfLifeButton, PlayClickSFX);
        CreateButtonListener(_uIManager.HabitatUI.TrainingPanel.DecreaseButton, PlayClickSFX);
        CreateButtonListener(_uIManager.HabitatUI.TrainingPanel.IncreaseButton, PlayClickSFX);
        CreateButtonListener(_uIManager.HabitatUI.TrainingPanel.DeclineButton, PlayClickSFX);
        CreateButtonListener(_uIManager.HabitatUI.TrainingPanel.ConfirmButton, PlayConfirmSFX);
    }

    private void CreateButtonListener(Button button, Action action)
    {
        if (button != null)
        {
            button.onClick.AddListener
            (delegate
            {
                action?.Invoke();
            });
        }
        else
        {
            Debug.LogError($"{button} is null! Please Fix");
        }
    }

    public void PlayHabitatMusic(HabitatType habitatType)
    {
        switch (habitatType)
        {
            case HabitatType.StonePlains:
                _musicSource.clip = _stonePlainsMusic;
                _musicSource.Play();
                break;
            case HabitatType.Ashlands:
                _musicSource.clip = _ashlandsMusic;
                _musicSource.Play();
                break;
            case HabitatType.TreeOfLife:
                _musicSource.clip = _treeOfLifeMusic;
                _musicSource.Play();
                break;
            default:
                Debug.LogError($"{habitatType} is invalid. Please change!");
                break;
        }
    }

    public void PlaySceneMusic(SceneType sceneType)
    {
        switch(sceneType)
        {
            //We add more if we want. Hopefully!
            case SceneType.MainMenu:
                _musicSource.clip = _mainMenuMusic;
                _musicSource.Play();
                break;
            default:
                Debug.LogError($"{sceneType} is invalid. Please change!");
                break;
        }
    }

    public void PlayElementsSFX(ElementsSFX uIElementsSFX)
    {
        switch(uIElementsSFX)
        {
            case ElementsSFX.StandardClick:
                _sfxSource.clip = _standardClickSFX;
                _sfxSource.Play();
                break;
            case ElementsSFX.ConfirmClick:
                _sfxSource.clip = _confirmClickSFX;
                _sfxSource.Play();
                break;
            case ElementsSFX.PurchaseClick:
                _sfxSource.clip = _purchaseClickSFX;
                _sfxSource.Play();
                break;
            case ElementsSFX.PlaceChimera:
                _sfxSource.clip = _placeChimeraSFX;
                _sfxSource.Play();
                break;
            case ElementsSFX.RemoveChimera:
                _sfxSource.clip = _removeChimeraSFX;
                _sfxSource.Play();
                break;
            case ElementsSFX.Evolution:
                _sfxSource.clip = _evolutionSFX;
                _sfxSource.Play();
                break;
            case ElementsSFX.LevelUp:
                _sfxSource.clip = _levelUpSFX;
                _sfxSource.Play();
                break;
            default:
                Debug.LogError($"{uIElementsSFX} is invalid. Please change!");
                break;
        }
    }

    private void PlayClickSFX()
    {
        PlayElementsSFX(ElementsSFX.StandardClick);
    }
    private void PlayConfirmSFX()
    {
        PlayElementsSFX(ElementsSFX.ConfirmClick);
    }
}