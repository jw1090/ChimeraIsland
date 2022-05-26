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
    private TutorialManager _tutorialManager = null;

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

        _tutorialManager.Initialize();
        _tutorialManager.ShowTutorial();
    }

    private void Initialize()
    {
        ServiceLocator.Register<LevelManager>(this, true);
        _persistentData = ServiceLocator.Get<PersistentData>();
        _inputManager = ServiceLocator.Get<InputManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

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

        _habitat.Initialize();
        _habitatManager.SetCurrentHabitat(_habitat);
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
        _habitatManager.SpawnChimerasForHabitat();
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