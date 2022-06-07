using UnityEngine;

public class LevelManager : AsyncLoader
{
    [SerializeField] private UIManager _uiManager = null;
    [SerializeField] private CameraController _cameraController = null;
    [SerializeField] private Habitat _habitat = null;

    private EssenceManager _essenceManager = null;
    private HabitatManager _habitatManager = null;
    private InputManager _inputManager = null;
    private PersistentData _persistentData = null;
    private TutorialManager _tutorialManager = null;

    protected override void Awake()
    {
        GameLoader.CallOnComplete(LevelSetup);
    }

    private void LevelSetup()
    {
        LevelManager.ResetStaticVariables();

        Initialize();
        
        if (LastSessionHabitatCheck() == false) // Return false when there is no need to change habitat.
        {
            _essenceManager.LoadEssence();
            InitializeUIElements();
            LoadFacilities();
            LoadChimeras();
            StartHabitatTickTimer();
            LevelManager.CallOnComplete(OnComplete);
        }
    }

    private void Initialize()
    {
        ServiceLocator.Register<LevelManager>(this, true);

        _essenceManager = ServiceLocator.Get<EssenceManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _inputManager = ServiceLocator.Get<InputManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        if (_uiManager != null)
        {
            ServiceLocator.Register<UIManager>(_uiManager.Initialize(), true);
            _inputManager.SetReleaseSlider(_uiManager.ReleaseSlider);
            _essenceManager.SetUIManager(_uiManager);
            _tutorialManager.SetUIManager(_uiManager);
        }
        if (_cameraController != null)
        {
            ServiceLocator.Register<CameraController>(_cameraController.Initialize(), true);
            _inputManager.SetCamera(_cameraController.CameraCO);
        }
        if (_habitat != null)
        {
            _habitat.Initialize();
            _habitatManager.SetCurrentHabitat(_habitat);
        }
    }

    private bool LastSessionHabitatCheck()
    {
        HabitatType lastSessionHabitat = _persistentData.LastSessionHabitat;

        switch (lastSessionHabitat)
        {
            case HabitatType.StonePlains:
            case HabitatType.TreeOfLife:
            case HabitatType.Ashlands:
                if(LoadLastSessionScene(lastSessionHabitat) == true) // Return false when there is no need to change habitat.
                {
                    return true;
                }

                return false;
            default:
                Debug.Log($"Invalid case: {lastSessionHabitat}. Staying in current Habitat");
                return false;
        }
    }

    private bool LoadLastSessionScene(HabitatType habitatType)
    {
        if (habitatType == _habitatManager.CurrentHabitat.Type)
        {
            return false;
        }

        Debug.Log($"Moving to LastSessionHabitat: {habitatType}");

        int loadNum = (int)habitatType + 4;
            
        return true;
    }

    private void InitializeUIElements()
    {
        if (_uiManager == null)
        {
            return;
        }

        _uiManager.InitializeUIElements();
    }

    private void LoadChimeras()
    {
        _habitatManager.SpawnChimerasForHabitat();
    }

    private void LoadFacilities()
    {
        _habitatManager.BuildFacilitiesForHabitat();
    }

    private void StartHabitatTickTimer()
    {
        if (_habitat == null)
        {
            return;
        }

        _habitat.StartTickTimer();
    }

    private void OnComplete()
    {
        Debug.Log("LevelManager Finished Setup");
    }
}