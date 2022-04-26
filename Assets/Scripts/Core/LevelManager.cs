using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private CameraController _cameraController = null;
    [SerializeField] private EssenceManager _essenceManager = null;
    [SerializeField] private UIManager _uiManager = null;
    [SerializeField] private Habitat _habitat = null;
    [SerializeField] private FileManager _fileManager = null;

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

        if(_inputManager)
        {
            ServiceLocator.Register<InputManager>(_inputManager.Initialize());
        }
        if (_cameraController != null)
        {
            ServiceLocator.Register<CameraController>(_cameraController.Initialize());
        }
        if(_essenceManager != null)
        {
            ServiceLocator.Register<EssenceManager>(_essenceManager.Initialize());
        }
        if (_uiManager != null)
        {
            ServiceLocator.Register<UIManager>(_uiManager.Initialize());
        }
        if (_habitat != null)
        {
            ServiceLocator.Register<Habitat>(_habitat.Initialize());
        }
        if (_fileManager)
        {
            ServiceLocator.Register<FileManager>(_fileManager.Initialize());
        }
    }
}