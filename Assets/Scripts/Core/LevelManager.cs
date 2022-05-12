using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private CameraController _cameraController = null;
    [SerializeField] private EssenceManager _essenceManager = null;
    [SerializeField] private UIManager _uiManager = null;
    [SerializeField] private Habitat _habitat = null;

    private ISessionData _sessionData;

    public bool IsInitialized { get; private set; }
    private void Awake()
    {
        GameLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        ServiceLocator.Register<LevelManager>(this, true);
        _sessionData = ServiceLocator.Get<ISessionData>();

        if(_inputManager)
        {
            ServiceLocator.Register<InputManager>(_inputManager.Initialize(), true);
        }
        if (_cameraController != null)
        {
            ServiceLocator.Register<CameraController>(_cameraController.Initialize(), true);
        }
        if (_essenceManager != null)
        {
            ServiceLocator.Register<EssenceManager>(_essenceManager.Initialize(), true);
        }
        if (_uiManager != null)
        {
            ServiceLocator.Register<UIManager>(_uiManager.Initialize(), true);
        }
        if (_habitat != null)
        {
            ServiceLocator.Register<Habitat>(_habitat.Initialize(), true);
        }

        IsInitialized = true;
    }
}