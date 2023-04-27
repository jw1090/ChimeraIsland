using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer _mixer = null;

    [Header("Source")]
    [SerializeField] private AudioSource _musicSource = null;
    [SerializeField] private AudioSource _sfxSource = null;
    [SerializeField] private AudioSource _ambientSource = null;
    [SerializeField] private AudioSource _uiSource = null;

    [Header("Manifests")]
    [SerializeField] private AudioManifest _musicManifest = null;
    [SerializeField] private AudioManifest _chimeraSFXManifest = null;
    [SerializeField] private AudioManifest _chimeraHappySFXManifest = null;
    [SerializeField] private AudioManifest _chimeraSadSFXManifest = null;
    [SerializeField] private AudioManifest _environmentSFXManifest = null;
    [SerializeField] private AudioManifest _facilityAmbientManifest = null;
    [SerializeField] private AudioManifest _facilityTrainingManifest = null;
    [SerializeField] private AudioManifest _habitatAmbientManifest = null;
    [SerializeField] private AudioManifest _uiSFXManifest = null;

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
                    AudioClipItem item = _facilityAmbientManifest.AudioItems.Where(c => c.Name == "Cave Ambient SFX").FirstOrDefault();
                    return item.Clip;
                }
            case FacilityType.RuneStone:
                {
                    AudioClipItem item = _facilityAmbientManifest.AudioItems.Where(c => c.Name == "Rune Ambient SFX").FirstOrDefault();
                    return item.Clip;
                }
            case FacilityType.Waterfall:
                {
                    AudioClipItem item = _facilityAmbientManifest.AudioItems.Where(c => c.Name == "Waterfall Ambient SFX").FirstOrDefault();
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
                    AudioClipItem item = _facilityTrainingManifest.AudioItems.Where(c => c.Name == "Cave Training SFX").FirstOrDefault();
                    return item.Clip;
                }
            case FacilityType.RuneStone:
                {
                    AudioClipItem item = _facilityTrainingManifest.AudioItems.Where(c => c.Name == "Rune Training SFX").FirstOrDefault();
                    return item.Clip;
                }
            case FacilityType.Waterfall:
                {
                    AudioClipItem item = _facilityTrainingManifest.AudioItems.Where(c => c.Name == "Waterfall Training SFX").FirstOrDefault();
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
        _persistentData.SetVolume(Volumes);
    }

    public void SetMusicVolume(float musicVolume)
    {
        _musicVolume = musicVolume;
        _mixer.SetFloat(GameConsts.AudioMixerKeys.MUSIC, _musicVolume);
        _persistentData.SetVolume(Volumes);
    }

    public void SetSFXVolume(float sfxVolume)
    {
        _sfxVolume = sfxVolume;
        _mixer.SetFloat(GameConsts.AudioMixerKeys.SFX, _sfxVolume);
        _persistentData.SetVolume(Volumes);
    }

    public void SetAmbientVolume(float sfxVolume)
    {
        _ambientVolume = sfxVolume;
        _mixer.SetFloat(GameConsts.AudioMixerKeys.AMBIENT, _ambientVolume);
        _persistentData.SetVolume(Volumes);
    }

    public void SetUISFXVolume(float sfxVolume)
    {
        _uiSfxVolume = sfxVolume;
        _mixer.SetFloat(GameConsts.AudioMixerKeys.UISFX, _uiSfxVolume);
        _persistentData.SetVolume(Volumes);
    }

    public void SetHabitat(Habitat habitat) { _habitat = habitat; }

    public AudioManager Initialize()
    {
        _persistentData = ServiceLocator.Get<PersistentData>();
        _uiManager = ServiceLocator.Get<UIManager>();

        _masterVolume = _persistentData.SettingsData.masterVolume;
        _musicVolume = _persistentData.SettingsData.musicVolume;
        _sfxVolume = _persistentData.SettingsData.sfxVolume;
        _ambientVolume = _persistentData.SettingsData.ambientVolume;
        _uiSfxVolume = _persistentData.SettingsData.uiSfxVolume;

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
        StartingUI startingUI = _uiManager.StartingUI;
        HabitatUI habitatUI = _uiManager.HabitatUI;
        TempleUI templeUI = _uiManager.TempleUI;

        _uiManager.CreateButtonListener(_uiManager.SettingsUI.MainMenuButton, PlayClickSFX);
        _uiManager.CreateButtonListener(_uiManager.SettingsUI.QuitGameButton, PlayClickSFX);
        _uiManager.CreateButtonListener(_uiManager.SettingsUI.ResumeButton, PlayClickSFX);
        _uiManager.CreateButtonListener(_uiManager.SettingsUI.ScreenWideButton, PlayClickSFX);

        _uiManager.CreateButtonListener(mainMenuUI.NewGameButton, PlayClickSFX);
        _uiManager.CreateButtonListener(mainMenuUI.WarningNoButton, PlayClickSFX);
        _uiManager.CreateButtonListener(mainMenuUI.WarningYesButton, PlayClickSFX);
        _uiManager.CreateButtonListener(mainMenuUI.LoadGameButton, PlayClickSFX);
        _uiManager.CreateButtonListener(mainMenuUI.OpenCreditsButton, PlayClickSFX);
        _uiManager.CreateButtonListener(mainMenuUI.CloseCreditsButton, PlayClickSFX);

        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.DecreaseButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.IncreaseButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.DeclineButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.TrainingPanel.ConfirmButton, PlayConfirmSFX);
        _uiManager.CreateButtonListener(habitatUI.CloseDetailsButton, PlayClickSFX);
        _uiManager.CreateButtonListener(habitatUI.OpenDetailsButton, PlayClickSFX);

        foreach(Button closeButton in habitatUI.ExpeditionPanel.CloseButtons)
        {
            _uiManager.CreateButtonListener(closeButton, PlayClickSFX);
        }

        _uiManager.CreateButtonListener(startingUI.AcceptButton, PlayConfirmSFX);
        _uiManager.CreateButtonListener(startingUI.DeclineButton, PlayClickSFX);

        _uiManager.CreateButtonListener(_uiManager.TempleUI.BackToHabitatButton, PlayClickSFX);
        _uiManager.CreateButtonListener(_uiManager.TempleUI.GoLeftButton, PlayWhooshSFX);
        _uiManager.CreateButtonListener(_uiManager.TempleUI.GoRightButton, PlayWhooshSFX);
    }

    public void PlayHabitatMusic()
    {
        _musicSource.Stop();

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

    public void PlayHabitatAmbient()
    {
        _ambientSource.Stop();

        if (_habitat.CurrentTier == 1)
        {
            AudioClipItem item = _habitatAmbientManifest.AudioItems.Where(c => c.Name == "StonePlainsAmbient").FirstOrDefault();
            _ambientSource.clip = item.Clip;
            _ambientSource.Play();
        }
        else if (_habitat.CurrentTier == 2)
        {
            AudioClipItem item = _habitatAmbientManifest.AudioItems.Where(c => c.Name == "StonePlainsAmbient2").FirstOrDefault();
            _ambientSource.clip = item.Clip;

        }
        else if (_habitat.CurrentTier == 3)
        {
            AudioClipItem item = _habitatAmbientManifest.AudioItems.Where(c => c.Name == "StonePlainsAmbient3").FirstOrDefault();
            _ambientSource.clip = item.Clip;
        }

        _ambientSource.Play();
    }

    public void PlaySceneMusic(SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.MainMenu:
                {
                    _musicSource.Stop();

                    AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "MainMenuMusic").FirstOrDefault();
                    _musicSource.clip = item.Clip;

                    _musicSource.Play();
                }
                break;
            case SceneType.Habitat:
                {
                    PlayHabitatMusic();
                }
                break;
            case SceneType.Starting:
                {
                    _musicSource.Stop();

                    AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "StarterSceneMusic").FirstOrDefault();
                    _musicSource.clip = item.Clip;

                    _musicSource.Play();
                }
                break;
            case SceneType.Temple:
                {
                    _musicSource.Stop();

                    AudioClipItem item = _musicManifest.AudioItems.Where(c => c.Name == "TempleMusic").FirstOrDefault();
                    _musicSource.clip = item.Clip;

                    _musicSource.Play();
                }
                break;
            default:
                Debug.LogError($"{sceneType} is invalid. Please change!");
                break;
        }
    }

    public void PlaySceneAmbience(SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.MainMenu:
            case SceneType.Starting:
                _ambientSource.Stop();
                break;
            case SceneType.Habitat:
                PlayHabitatAmbient();
                break;
            case SceneType.Temple:
                {
                    _ambientSource.Stop();

                    AudioClipItem item = _habitatAmbientManifest.AudioItems.Where(c => c.Name == "Temple Ambient").FirstOrDefault();
                    _ambientSource.clip = item.Clip;

                    _ambientSource.Play();
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
            case SFXUIType.ErrorClick:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Error SFX").FirstOrDefault();
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
            case SFXUIType.StoneDrag:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Stone Drag SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case SFXUIType.Whoosh:
                {
                    AudioClipItem item = _uiSFXManifest.AudioItems.Where(c => c.Name == "Whoosh SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            default:
                Debug.LogError($"{uIElementsSFX} is invalid. Please change!");
                break;
        }
    }

    public void PlaySFX(EnvironmentSFXType environmentSFXType)
    {
        switch (environmentSFXType)
        {
            case EnvironmentSFXType.Evolution:
                {
                    AudioClipItem item = _environmentSFXManifest.AudioItems.Where(c => c.Name == "Evolution SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case EnvironmentSFXType.LevelUp:
                {
                    AudioClipItem item = _environmentSFXManifest.AudioItems.Where(c => c.Name == "Level Up SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case EnvironmentSFXType.PortalClick:
                {
                    AudioClipItem item = _environmentSFXManifest.AudioItems.Where(c => c.Name == "Portal Click SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case EnvironmentSFXType.MiningTap:
                {
                    AudioClipItem item = _environmentSFXManifest.AudioItems.Where(c => c.Name == "Mining Tap SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case EnvironmentSFXType.MiningHarvest:
                {
                    AudioClipItem item = _environmentSFXManifest.AudioItems.Where(c => c.Name == "Mining Harvest SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case EnvironmentSFXType.WaterHit:
                {
                    AudioClipItem item = _environmentSFXManifest.AudioItems.Where(c => c.Name == "Water Hit SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case EnvironmentSFXType.StoneHit:
                {
                    AudioClipItem item = _environmentSFXManifest.AudioItems.Where(c => c.Name == "Stone Hit SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case EnvironmentSFXType.DirtHit:
                {
                    AudioClipItem item = _environmentSFXManifest.AudioItems.Where(c => c.Name == "Dirt Hit SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case EnvironmentSFXType.TreeHit:
                {
                    AudioClipItem item = _environmentSFXManifest.AudioItems.Where(c => c.Name == "Tree Hit SFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            default:
                Debug.LogError($"{environmentSFXType} is invalid. Please change!");
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

    public void PlayHappyChimeraSFX(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera A HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.A1:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera A1 HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.A2:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera A2 HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.A3:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera A3 HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                    break;
                }
            case ChimeraType.B:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera B HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.B1:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera B1 HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.B2:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera B2 HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.B3:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera B3 HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera C HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C1:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera C1 HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C2:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera C2 HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C3:
                {
                    AudioClipItem item = _chimeraHappySFXManifest.AudioItems.Where(c => c.Name == "Chimera C3 HappySFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            default:
                Debug.LogError($"{chimeraType} is invalid. Please change!");
                break;
        }
    }

    public void PlaySadChimeraSFX(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera A SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.A1:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera A1 SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.A2:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera A2 SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.A3:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera A3 SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                    break;
                }
            case ChimeraType.B:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera B SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.B1:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera B1 SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.B2:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera B2 SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.B3:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera B3 SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera C SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C1:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera C1 SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C2:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera C2 SadSFX").FirstOrDefault();
                    _sfxSource.clip = item.Clip;
                    _sfxSource.PlayOneShot(_sfxSource.clip);
                }
                break;
            case ChimeraType.C3:
                {
                    AudioClipItem item = _chimeraSadSFXManifest.AudioItems.Where(c => c.Name == "Chimera C3 SadSFX").FirstOrDefault();
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

    private void PlayWhooshSFX()
    {
        PlayUISFX(SFXUIType.Whoosh);
    }
}