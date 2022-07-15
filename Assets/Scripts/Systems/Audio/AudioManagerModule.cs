using System.Collections;
using UnityEngine;

public class AudioManagerModule : MonoBehaviour, IGameModule
{
    [SerializeField] private GameObject _audioManagerPrefab = null;

    public IEnumerator LoadModule()
    {
        Debug.Log($"<color=Yellow> Loading {this.GetType()}.</color>");

        GameObject sysObj = Instantiate(_audioManagerPrefab, GameLoader.SystemsParent);
        sysObj.name = "Audio Manager";
        AudioManager audioManagerComp = sysObj.GetComponent<AudioManager>();
        ServiceLocator.Register<AudioManager>(audioManagerComp.Initialize());

        ServiceLocator.Get<PersistentData>().SetAudioManager(audioManagerComp);
        ServiceLocator.Get<HabitatManager>().SetAudioManager(audioManagerComp);
        ServiceLocator.Get<UIManager>().HabitatUI.InitializeVolumeSettings();

        yield return null;
    }
}