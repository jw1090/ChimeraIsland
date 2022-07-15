using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer _mixer = null;

    [Header("Source")]
    [SerializeField] private AudioSource _musicSource = null;

    [Header("Music")]
    [SerializeField] private AudioClip _stonePlainsMusic = null;
    [SerializeField] private AudioClip _treeOfLifeMusic = null;
    [SerializeField] private AudioClip _ashlandsMusic = null;

    [Header("SFX")]
    [SerializeField] private AudioClip _waterfallSFX = null;
    private AudioSource _caveSource = null;
    private AudioSource _runeStoneSource = null;
    private AudioSource _waterfallSource = null;
    private PersistentData _persistentData = null;
    private float _masterVolume = 0.0f;
    private float _musicVolume = 0.0f;
    private float _sfxVolume = 0.0f;

    public Vector3 Volumes { get => new Vector3(_masterVolume, _musicVolume, _sfxVolume); }
    public float MasterVolume { get => _masterVolume; }
    public float MusicVolume { get => _musicVolume; }
    public float SFXVolume { get => _sfxVolume; }

    public void SetHabitatSFXSources(HabitatSources habitatSources)
    {
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
        _persistentData = ServiceLocator.Get<PersistentData>();

        _masterVolume = _persistentData.Volumes.x;
        _musicVolume = _persistentData.Volumes.y;
        _sfxVolume = _persistentData.Volumes.z;

        _mixer.SetFloat(GameConsts.AudioMixerKeys.MASTER, _masterVolume);
        _mixer.SetFloat(GameConsts.AudioMixerKeys.MUSIC, _musicVolume);
        _mixer.SetFloat(GameConsts.AudioMixerKeys.SFX, _sfxVolume);

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