using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private AudioMixer _mixer = null;
    private AudioSource _habitatSource = null;
    private AudioSource _caveSource = null;
    private AudioSource _runeStoneSource = null;
    private AudioSource _waterfallSource = null;
    private AudioClip _stonePlainsMusic = null;
    private AudioClip _ashlandsMusic = null;
    private AudioClip _treeOfLifeMusic = null;
    private AudioClip _waterfallSFX = null;
    private ResourceManager _resourceManager = null;
    private float _masterVolume = 0.0f;
    private float _musicVolume = 0.0f;
    private float _sfxVolume = 0.0f;

    public float MasterVolume { get => _masterVolume; }
    public float MusicVolume { get => _musicVolume; }
    public float SFXVolume { get => _sfxVolume; }

    public void SetHabitatAudioSources(HabitatSources habitatSources)
    {
        _habitatSource = habitatSources.HabitatSource;
        _caveSource = habitatSources.CaveSource;
        _runeStoneSource = habitatSources.RuneStoneSource;
        _waterfallSource = habitatSources.WaterfallSource;
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
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _resourceManager = ServiceLocator.Get<ResourceManager>();

        _mixer = _resourceManager.AudioMixer;
        _stonePlainsMusic = _resourceManager.GetHabitatMusic(HabitatType.StonePlains);
        _treeOfLifeMusic = _resourceManager.GetHabitatMusic(HabitatType.StonePlains);
        _ashlandsMusic = _resourceManager.GetHabitatMusic(HabitatType.StonePlains);
        _waterfallSFX = _resourceManager.GetFacilitySFX(FacilityType.Waterfall);

        _masterVolume = PlayerPrefs.GetFloat(GameConsts.AudioMixerKeys.MASTER, 0.0f);
        _musicVolume = PlayerPrefs.GetFloat(GameConsts.AudioMixerKeys.MUSIC, 0.0f);
        _sfxVolume = PlayerPrefs.GetFloat(GameConsts.AudioMixerKeys.SFX, 0.0f);

        return this;
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(GameConsts.AudioMixerKeys.MASTER, _masterVolume);
        PlayerPrefs.SetFloat(GameConsts.AudioMixerKeys.MUSIC, _musicVolume);
        PlayerPrefs.SetFloat(GameConsts.AudioMixerKeys.SFX, _sfxVolume);
    }

    public void PlayHabitatMusic(HabitatType habitatType)
    {
        switch (habitatType)
        {
            case HabitatType.StonePlains:
                _habitatSource.clip = _stonePlainsMusic;
                _habitatSource.Play();
                break;
            case HabitatType.Ashlands:
                _habitatSource.clip = _ashlandsMusic;
                _habitatSource.Play();
                break;
            case HabitatType.TreeOfLife:
                _habitatSource.clip = _treeOfLifeMusic;
                _habitatSource.Play();
                break;
            default:
                Debug.LogError($"{habitatType} is invalid. Please change!");
                break;
        }
    }

    public void PlayFacilitySFX(FacilityType facilityType)
    {
        switch (facilityType)
        {
            case FacilityType.Cave:
                _caveSource.clip = _waterfallSFX; // TODO: Change to cave.
                _caveSource.Play();
                break;
            case FacilityType.RuneStone:
                _runeStoneSource.clip = _waterfallSFX; // TODO: Change to rune stone.
                _runeStoneSource.Play();
                break;
            case FacilityType.Waterfall:
                _waterfallSource.clip = _waterfallSFX;
                _waterfallSource.Play();
                break;
            default:
                Debug.LogError($"{facilityType} is invalid. Please change!");
                break;
        }
    }
}