using UnityEngine;

public class LevelManager : AsyncLoader
{
    [SerializeField] private UIManager _uiManager = null;
    [SerializeField] private CameraController _cameraController = null;
    [SerializeField] private EssenceManager _essenceManager = null;
    [SerializeField] private Habitat _habitat = null;

    private PersistentData _persistentData = null;
    private InputManager _inputManager = null;
    private HabitatManager _habitatManager = null;

    protected override void Awake()
    {
        GameLoader.CallOnComplete(LevelSetup);
    }

    private void LevelSetup()
    {
        Initialize();
        LoadUI();
        LoadChimeras();
        StartHabitatTickTimer();

        LevelManager.ResetStaticVariables();
        LevelManager.CallOnComplete(OnComplete);
    }

    private void Initialize()
    {
        ServiceLocator.Register<LevelManager>(this, true);
        _persistentData = ServiceLocator.Get<PersistentData>();
        _inputManager = ServiceLocator.Get<InputManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();

        if (_uiManager != null)
        {
            ServiceLocator.Register<UIManager>(_uiManager.Initialize(), true);
            _inputManager.SetReleaseSlider(_uiManager.GetReleaseSlider());
        }
        if (_cameraController != null)
        {
            ServiceLocator.Register<CameraController>(_cameraController.Initialize(), true);
            _inputManager.SetCameraMain(_cameraController.CameraCO);
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
    }

    private void LoadUI()
    {
        if (_uiManager == null)
        {
            return;
        }

        _uiManager.LoadDetails(_habitat);
        _uiManager.LoadMarketplace(_habitat);
    }

    private void LoadChimeras()
    {
        var chimerasToSpawn = _habitatManager.GetChimerasForHabitat(_habitat.Type);
        _habitat.SpawnChimeras(chimerasToSpawn);
    }

    private void StartHabitatTickTimer()
    {
        _habitat.StartTickTimer();
    }

    private void OnComplete()
    {
        Debug.Log("LevelManager Finished Setup");
    }
}