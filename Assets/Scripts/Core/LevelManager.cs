using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private MenuManager _menuManager = null;
    [SerializeField] private Habitat _habitat = null;

    private IPersistentData _persistentData;
    private ISessionData _sessionData;

    private void Awake()
    {
        GameLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _persistentData = ServiceLocator.Get<IPersistentData>();
        _sessionData = ServiceLocator.Get<ISessionData>();

        if(_habitat != null)
        {
            _habitat.Initialize();
        }
        if (_menuManager != null)
        {
            _menuManager.Initialize();
        }
    }
}