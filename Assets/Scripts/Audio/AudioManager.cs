using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, IGameModule
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource source;
    //[SerializeField] List<AudioClip> soundFx = new List<AudioClip>();

    [Header("Habitat Music")]
    [SerializeField] private AudioClip _stonePlainsMusic = null;

    //Keys
    public const string MASTER_KEY = "Master";
    public const string MUSIC_KEY = "MusicVolume";
    public const string SFX_KEY = "SFXVolume";


    void LoadVolume() // Volume saved in VolumeSettings.cs
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        mixer.SetFloat(VolumeSettings.MIXER_MASTER, (-80 + masterVolume * 100));
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, (-80 + masterVolume * 100));
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume)*20);
    }

    public IEnumerator LoadModule()
    {
        ServiceLocator.Register<AudioManager>(this);
        yield return null;
    }

    public void SetMusicForHabitat(HabitatType habitat)
    {
        switch (habitat)
        {
            case HabitatType.StonePlains:
                source.clip = _stonePlainsMusic;
                source.Play();
                break;

        }
    }
}
