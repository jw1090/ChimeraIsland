using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PatrolNodes patrolNodes = null;

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

        patrolNodes.Initialize();
    }
}