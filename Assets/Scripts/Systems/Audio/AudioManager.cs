using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer _mixer = null;

    [Header("Source")]
    [SerializeField] private AudioSource _musicSource = null;
    [SerializeField] private AudioSource _sfxSource = null;

    [Header("Music")]
    [SerializeField] private AudioManifest _musicManifest = null;

    [Header("UI SFX")]
    [SerializeField] private AudioManifest _uiSFXManifest = null;

    [Header("Ambient SFX")]
    [SerializeField] private AudioManifest _ambientSFXManifest = null;

    [Header("Facility SFX")]
    [SerializeField] private AudioManifest _facilitySFXManifest = null;

    private UIManager _uiManager = null;
    private PersistentData _persistentData = null;
    private FacilitySFX _facilitySFX = null;
    private float _masterVolume = 0.0f;
    private float _musicVolume = 0.0f;
    private float _sfxVolume = 0.0f;

    public Vector3 Volumes { get => new Vector3(_masterVolume, _musicVolume, _sfxVolume); }
    public float MasterVolume { get => _masterVolume; }
    public float MusicVolume { get => _musicVolume; }
    public float SFXVolume { get => _sfxVolume; }

    public AudioClip GetFacilityAmbient(FacilityType facilityType)
    {
        switch (facilityType)
        {
            case FacilityType.Cave:
                {
                    AudioClipItem item = _ambientSFXManifest.AudioItems.Where(c => c.Name == "Cave Ambient SFX").FirstOrDefault();
                    return item.Clip;
                }
            case FacilityType.RuneStone:
                {
                    AudioClipItem item = _ambientSFXManifest.AudioItems.Where(c => c.Name == "Rune Ambient SFX").FirstOrDefault();
                    return item.Clip;
                }
            case FacilityType.Waterfall:
                {
                    AudioClipItem item = _ambientSFXManifest.AudioItems.Where(c => c.Name == "Waterfall Ambient SFX").FirstOrDefault();
                    return item.Clip;
                }
            default:
                return null;
        }
    }

    public AudioClip GetFacilityTraining(FacilityType facilityType)
    {
        switch (facilityType)
        {
            case FacilityType.Cave:
                {
                    AudioClipItem item = _facilitySFXManifest.AudioItems.Where(c => c.Name == "Cave Training SFX").FirstOrDefault();
                    return item.Clip;
                }
            case FacilityType.RuneStone:
                {
                    AudioClipItem item = _facilitySFXManifest.AudioItems.Where(c => c.Name == "Rune Training SFX").FirstOrDefault();
                    return item.Clip;
                }
            case FacilityType.Waterfall:
                {
                    AudioClipItem item = _facilitySFXManifest.AudioItems.Where(c => c.Name == "Waterfall Training SFX").FirstOrDefault();
                    return item.Clip;
                }
            default:
                return null;
        }
    }

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
        _uiManager = ServiceLocator.Get<UIManager>();
        _facilitySFX = GetComponent<FacilitySFX>();

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
        MainMenuUI mainMenuUI = _uiManager.MainMenuUI;
        HabitatUI habitatUI = _uiManager.HabitatUI;
        WorldMapUI worldMapUI = _uiManager.WorldMapUI;

        _uiManager.CreateButtonListener(mainMenuUI.NewGameButton, PlayClickSFX);
        _uiManager.CreateButtonListener(mainMenuUI.LoadGameButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.MainMenuButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.WorldMapButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.QuitGameButton, PlayClickSFX);
        _uiManager.CreateButtonListener(worldMapUI.StonePlainsButton, PlayClickSFX);
        _uiManager.CreateButtonListener(worldMapUI.TreeOfLifeButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.DecreaseButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.IncreaseButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.DeclineButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.ConfirmButton, PlayConfirmSFX);
    }

    public void PlayHabitatMusic(HabitatType habitatType)
    {
        switch (habitatType)
        {
            case HabitatType.StonePlains:
                {
                    AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "StonePlains").FirstOrDefault();
                    _musicSource.clip = item.Clip;
                    _musicSource.Play();
                }
                break;
            case HabitatType.TreeOfLife:
                {
                    AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "TreeOfLife").FirstOrDefault();
                    _musicSource.clip = item.Clip;
                    _musicSource.Play();
                }
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
            case SceneType.MainMenu:
                {
                    AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "MainMenuMusic").FirstOrDefault();
                    _musicSource.clip = item.Clip;
                    _musicSource.Play();
                }
                break;
            case SceneType.Starting:
                {
                    AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "StarterSceneMusic").FirstOrDefault();
                    _musicSource.clip = item.Clip;
                    _musicSource.Play();
                }
                break;
            default:
                Debug.LogError($"{sceneType} is invalid. Please change!");
                break;
        }
    }

    public void PlayUISFX(SFXUIType uIElementsSFX)
    {
        switch(uIElementsSFX)
        {
            case SFXUIType.StandardClick:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Standard Click SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.Play();
                }
                break;
            case SFXUIType.ConfirmClick:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Confirm Click SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.Play();
                }
                break;
            case SFXUIType.PurchaseClick:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Purchase Click SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.Play();
                }
                break;
            case SFXUIType.PlaceChimera:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Place Chimera SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.Play();
                }
                break;
            case SFXUIType.RemoveChimera:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Remove Chimera SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.Play();
                }
                break;
            case SFXUIType.Evolution:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Evolution SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.Play();
                }
                break;
            case SFXUIType.LevelUp:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Level Up SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.Play();
                }
                break; 
            case SFXUIType.ErrorClick:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Error SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.Play();
                }
                break;
            default:
                Debug.LogError($"{uIElementsSFX} is invalid. Please change!");
                break;
        }
    }
    private void PlayClickSFX()
    {
        PlayUISFX(SFXUIType.StandardClick);
    }
    private void PlayConfirmSFX()
    {
        PlayUISFX(SFXUIType.ConfirmClick);
    }

}