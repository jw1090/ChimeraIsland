using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager = null;
    [SerializeField] private CameraController _cameraController = null;
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private EssenceManager _essenceManager = null;
    [SerializeField] private Habitat _habitat = null;

    private HabitatManager _habitatManager = null;
    private PersistentData _persistentData = null;

    public bool IsInitialized { get; private set; }
    private void Awake()
    {
        GameLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        ServiceLocator.Register<LevelManager>(this, true);
        _persistentData = ServiceLocator.Get<PersistentData>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();

        if (_uiManager != null)
        {
            ServiceLocator.Register<UIManager>(_uiManager.Initialize(), true);
        }
        if (_cameraController != null)
        {
            ServiceLocator.Register<CameraController>(_cameraController.Initialize(), true);
        }
        if (_inputManager != null)
        {
            ServiceLocator.Register<InputManager>(_inputManager.Initialize(), true);
        }
        if (_essenceManager != null)
        {
            ServiceLocator.Register<EssenceManager>(_essenceManager.Initialize(), true);
            _persistentData.SetEssenceManager(_essenceManager);
        }
        if (_habitat != null)
        {
            ServiceLocator.Register<Habitat>(_habitat.Initialize(), true);
        }

        IsInitialized = true;

        LoadChimeras();
    }

    private void LoadChimeras()
    {
        var chimerasToSpawn = _habitatManager.GetChimerasForHabitat(_habitat.GetHabitatType());
        _habitat.SpawnChimeras(chimerasToSpawn);
    }
}
