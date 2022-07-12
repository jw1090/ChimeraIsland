using System.Collections;
using UnityEngine;

public class AudioManagerModule : MonoBehaviour, IGameModule
{
    [SerializeField] private GameObject _systemPrefab = null;

    public IEnumerator LoadModule()
    {
        GameObject sysObj = Instantiate(_systemPrefab, GameLoader.SystemsParent);
        sysObj.name = "Audio Manager";
        ServiceLocator.Register<AudioManager>(sysObj.GetComponent<AudioManager>());
        yield return null;
    }
}
