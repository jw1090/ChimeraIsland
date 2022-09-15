using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer _mixer = null;

    [Header("Source")]
    [SerializeField] private AudioSource _musicSource = null;
    [SerializeField] private AudioSource _sfxSource = null;
    [SerializeField] private AudioSource _ambientSource = null;
    [SerializeField] private AudioSource _uiSource = null;

    [Header("Music")]
    [SerializeField] private AudioManifest _musicManifest = null;

    [Header("Ambient")]
    [SerializeField] private AudioManifest _ambientManifest = null;

    [Header("UI SFX")]
    [SerializeField] private AudioManifest _uiSFXManifest = null;

    [Header("Ambient SFX")]
    [SerializeField] private AudioManifest _ambientSFXManifest = null;

    [Header("Facility SFX")]
    [SerializeField] private AudioManifest _facilitySFXManifest = null;
    
    [Header("Chimeras SFX")]
    [SerializeField] private AudioManifest _chimeraSFXManifest = null;

    private UIManager _uiManager = null;
    private PersistentData _persistentData = null;
    private Habitat _habitat = null;
    private float _masterVolume = 0.0f;
    private float _musicVolume = 0.0f;
    private float _sfxVolume = 0.0f;
    private float _ambientVolume = 0.0f;
    private float _uiSfxVolume = 0.0f;

    public List<float> Volumes { get => new List<float> { _masterVolume, _musicVolume, _sfxVolume, _ambientVolume, _uiSfxVolume }; }
    public float MasterVolume { get => _masterVolume; }
    public float MusicVolume { get => _musicVolume; }
    public float SFXVolume { get => _sfxVolume; }
    public float AmbientVolume { get => _ambientVolume; }
    public float UISFXVolume { get => _uiSfxVolume; }

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

    public void SetAmbientVolume(float sfxVolume)
    {
        _ambientVolume = sfxVolume;
        _mixer.SetFloat(GameConsts.AudioMixerKeys.AMBIENT, _ambientVolume);
    }

    public void SetUISFXVolume(float sfxVolume)
    {
        _uiSfxVolume = sfxVolume;
        _mixer.SetFloat(GameConsts.AudioMixerKeys.UISFX, _uiSfxVolume);
    }

    public void SetHabitat(Habitat habitat) { _habitat = habitat; }

    public AudioManager Initialize()
    {
        _persistentData = ServiceLocator.Get<PersistentData>();
        _masterVolume = _persistentData.Volumes[0];
        _musicVolume = _persistentData.Volumes[1];
        _sfxVolume = _persistentData.Volumes[2];
        _ambientVolume = _persistentData.Volumes[3];
        _uiSfxVolume= _persistentData.Volumes[4];
        _uiManager = ServiceLocator.Get<UIManager>();

        _mixer.SetFloat(GameConsts.AudioMixerKeys.MASTER, _masterVolume);
        _mixer.SetFloat(GameConsts.AudioMixerKeys.MUSIC, _musicVolume);
        _mixer.SetFloat(GameConsts.AudioMixerKeys.SFX, _sfxVolume);
        _mixer.SetFloat(GameConsts.AudioMixerKeys.AMBIENT, _ambientVolume);
        _mixer.SetFloat(GameConsts.AudioMixerKeys.UISFX, _uiSfxVolume);

        SetupAudioListeners();

        return this;
    }

    public void SetupAudioListeners()
    {
        MainMenuUI mainMenuUI = _uiManager.MainMenuUI;
        HabitatUI habitatUI = _uiManager.HabitatUI;

        _uiManager.CreateButtonListener(mainMenuUI.NewGameButton, PlayClickSFX);
        _uiManager.CreateButtonListener(mainMenuUI.LoadGameButton, PlayClickSFX);
        _uiManager.CreateButtonListener(mainMenuUI.OpenCreditsButton, PlayClickSFX);
        _uiManager.CreateButtonListener(mainMenuUI.CloseCreditsButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.Settings.MainMenuButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.Settings.QuitGameButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.DecreaseButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.IncreaseButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.DeclineButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.ConfirmButton, PlayConfirmSFX);
        _uiManager.CreateButtonListener(habitatUI.Settings.ResumeButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.Settings.ScreenWideButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.ExpeditionPanel.CloseButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.Marketplace.CloseButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.CloseDetailsButton, PlayClickSFX);
    }

    public void PlayHabitatMusic(HabitatType habitatType)
    {
        switch (habitatType)
        {
            case HabitatType.StonePlains:
                {
                   PlayMusicOnTier();
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

    public void PlayHabitatAmbient(HabitatType habitatType)
    {
        switch (habitatType)
        {
            case HabitatType.StonePlains:
                {
                    PlayAmbientOnTier();
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
                    StopAmbientSource();
                    _musicSource.Play();
                }
                break;
            case SceneType.Starting:
            case SceneType.Builder:
                {
                    AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "StarterSceneMusic").FirstOrDefault();
                    _musicSource.clip = item.Clip;
                    StopAmbientSource();
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
                    _uiSource.clip = item.Clip;
                    _uiSource.Play();
                }
                break;
            case SFXUIType.ConfirmClick:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Confirm Click SFX").FirstOrDefault();
                    _uiSource.clip = item.Clip;
                    _uiSource.Play();
                }
                break;
            case SFXUIType.PurchaseClick:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Purchase Click SFX").FirstOrDefault();
                    _uiSource.clip = item.Clip;
                    _uiSource.Play();
                }
                break;
            case SFXUIType.PlaceChimera:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Place Chimera SFX").FirstOrDefault();
                    _uiSource.clip = item.Clip;
                    _uiSource.Play();
                }
                break;
            case SFXUIType.RemoveChimera:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Remove Chimera SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case SFXUIType.Evolution:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Evolution SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case SFXUIType.LevelUp:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Level Up SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break; 
            case SFXUIType.ErrorClick:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Error SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case SFXUIType.PortalClick:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Portal Click SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case SFXUIType.Completion:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Completion SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case SFXUIType.Failure:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Failure SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case SFXUIType.Hit:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Hit SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case SFXUIType.Harvest:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Harvest SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            default:
                Debug.LogError($"{uIElementsSFX} is invalid. Please change!");
                break;
        }
    }
    public void PlayHeldChimeraSFX(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "A").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.A1:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "A1").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.A2:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "A2").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.A3:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "A3").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                    break;
                }
            case ChimeraType.B:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "B").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.B1:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "B1").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.B2:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "B2").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.B3:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "B3").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "C").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C1:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "C1").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C2:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "C2").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C3:
                {
                    AudioClipItem item = _chimeraSFXManifest.AudioItems.Where(c => c.Name == "C3").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            default:
                Debug.LogError($"{chimeraType} is invalid. Please change!");
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

    private void StopAmbientSource()
    {
        _ambientSource.Stop();
    }

    private void PlayMusicOnTier()
    {
        if (_habitat.CurrentTier == 1)
        {
            AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "StonePlains").FirstOrDefault();
            _musicSource.clip = item.Clip;
        }
        if (_habitat.CurrentTier == 2)
        {
            AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "StonePlains2").FirstOrDefault();
            _musicSource.clip = item.Clip;
        }
        if (_habitat.CurrentTier == 3)
        {
            AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "StonePlains3").FirstOrDefault();
            _musicSource.clip = item.Clip;
        }
        _musicSource.Play();
    }

    private void PlayAmbientOnTier()
    {
        if (_habitat.CurrentTier == 1)
        {
             AudioClipItem item = _ambientManifest.AudioItems.Where(c => c.Name == "StonePlainsAmbient").FirstOrDefault();
            _ambientSource.clip = item.Clip;
            _ambientSource.Play();
        }
        if (_habitat.CurrentTier == 2)
        {
             AudioClipItem item = _ambientManifest.AudioItems.Where(c => c.Name == "StonePlainsAmbient2").FirstOrDefault();
            _ambientSource.clip = item.Clip;

        }
        if (_habitat.CurrentTier == 3)
        {
             AudioClipItem item = _ambientManifest.AudioItems.Where(c => c.Name == "StonePlainsAmbient3").FirstOrDefault();
            _ambientSource.clip = item.Clip;
        }
        _ambientSource.Play();
    }
}