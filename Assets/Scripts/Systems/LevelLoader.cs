using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : AsyncLoader
{
    [SerializeField] private SceneType _sceneType = SceneType.None;

    [Header("Standard References")]
    [SerializeField] private CameraUtil _cameraUtil = null;
    [SerializeField] private Habitat _habitat = null;
    [SerializeField] private ExpeditionManager _expeditionManager = null;
    [SerializeField] private LightingManager _lightingManager = null;

    [Header("Builder References")]
    [SerializeField] private EvolutionBuilder _evolutionBuilder = null;

    private static LevelLoader _instance = null;
    private readonly static List<Action> _queuedCallbacks = new List<Action>();

    private HabitatManager _habitatManager = null;
    private InputManager _inputManager = null;
    private PersistentData _persistentData = null;
    private TutorialManager _tutorialManager = null;
    private UIManager _uiManager = null;
    private AudioManager _audioManager = null;

    protected override void Awake()
    {
        _instance = this;
        GameLoader.CallOnComplete(LevelSetup);
    }

    private void OnDestroy()
    {
        ResetVariables();
    }

    private void LevelSetup()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        Initialize();
        ProcessQueuedCallbacks();

        LoadUIElements();

        switch (_sceneType)
        {
            case SceneType.Habitat:
                HabitatSceneSetup();
                break;
            case SceneType.MainMenu:
            case SceneType.Starting:
                PlayCurrentSceneMusic();
                break;
            case SceneType.Builder:
                _evolutionBuilder.BuildAll();
                _uiManager.EvolutionBuilderUI.LoadBaseChimeras();
                PlayCurrentSceneMusic();
                break;
            default:
                Debug.LogError($"{_sceneType} is invalid, please change!.");
                break;
        }

        CallOnComplete(OnComplete);
    }

    private void Initialize()
    {
        ServiceLocator.Register<LevelLoader>(this, true);

        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _inputManager = ServiceLocator.Get<InputManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _uiManager = ServiceLocator.Get<UIManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        if (_cameraUtil != null)
        {
            ServiceLocator.Register<CameraUtil>(_cameraUtil.Initialize(_sceneType), true);
            _inputManager.SetCameraUtil(_cameraUtil);
        }

        if (_habitat != null)
        {
            _habitat.Initialize();
            _habitatManager.SetCurrentHabitat(_habitat);
        }

        if (_expeditionManager != null)
        {
            ServiceLocator.Register<ExpeditionManager>(_expeditionManager.Initialize(), true);
            _uiManager.HabitatUI.SetExpeditionManager(_expeditionManager);
            _habitat.SetExpeditionManager(_expeditionManager);
            _inputManager.SetExpeditionManager(_expeditionManager);
        }

        if (_lightingManager != null)
        {
            ServiceLocator.Register<LightingManager>(_lightingManager.Initialize(), true);
            _habitat.SetLightingManager(_lightingManager);
        }

        if (_evolutionBuilder != null)
        {
            _evolutionBuilder.Initialize();
            _uiManager.EvolutionBuilderUI.SetEvolutionBuilder(_evolutionBuilder);
        }
    }

    private void HabitatSceneSetup()
    {
        if (LastSessionHabitatCheck() == false) // Return false when there is no need to change habitat.
        {
            TempleBuildCheck();

            _habitatManager.PlayCurrentHabitatMusic();
            _habitatManager.BuildFacilitiesForHabitat();
            _habitatManager.SpawnChimerasForHabitat();

            _habitatManager.CurrentHabitat.MoveChimerasToFacility();

            StartHabitatTickTimer();
        }

        _tutorialManager.TutorialStageCheck();
    }

    private void TempleBuildCheck()
    {
        if (_expeditionManager.CurrentFossilProgress == 0)
        {
            _habitatManager.CurrentHabitat.Temple.ResetTemple();
        }
        else
        {
            _habitatManager.CurrentHabitat.Temple.Build();
        }
    }

    private bool LastSessionHabitatCheck()
    {
        HabitatType lastSessionHabitat = _persistentData.LastSessionHabitat;

        switch (lastSessionHabitat)
        {
            case HabitatType.StonePlains:
            case HabitatType.TreeOfLife:
                if (LoadLastSessionScene(lastSessionHabitat) == true) // Return false when there is no need to change habitat.
                {
                    return true;
                }
                return false;
            default:
                Debug.Log($"Invalid case: {lastSessionHabitat}. Staying in current Habitat.");
                return false;
        }
    }

    private bool LoadLastSessionScene(HabitatType habitatType)
    {
        if (habitatType == _habitatManager.CurrentHabitat.Type)
        {
            Debug.Log($"Habitat is already {habitatType}. No need to move.");
            return false;
        }

        Debug.Log($"Moving to LastSessionHabitat: {habitatType}");

        _persistentData.ResetLastSessionHabitat();

        int loadNum = (int)habitatType + 4;
        SceneManager.LoadSceneAsync(loadNum);

        return true;
    }

    private void LoadUIElements()
    {
        if (_uiManager == null)
        {
            return;
        }

        switch (_sceneType)
        {
            case SceneType.Habitat:
                _uiManager.HabitatUI.LoadHabitatSpecificUI();
                break;
            case SceneType.MainMenu:
                break;
            case SceneType.Starting:
                _uiManager.StartingUI.SetupStartingButtons();
                break;
            case SceneType.Builder:
                break;
            default:
                Debug.LogWarning($"Scene Type: {_sceneType} is invalid.");
                break;
        }

        _uiManager.ShowUIByScene(_sceneType);
    }

    private void StartHabitatTickTimer()
    {
        if (_habitat == null)
        {
            return;
        }

        _habitat.StartTickTimer();
    }

    private void ProcessQueuedCallbacks()
    {
        foreach (var callback in _queuedCallbacks)
        {
            callback?.Invoke();
        }
    }

    protected override void ResetVariables()
    {
        base.ResetVariables();
        _queuedCallbacks.Clear();
    }

    public static void CallOnComplete(Action callback)
    {
        if (_instance == null)
        {
            _queuedCallbacks.Add(callback);
            return;
        }

        _instance.CallOnComplete_Internal(callback);
    }

    private void OnComplete()
    {
        Debug.Log($"<color=Lime> {this.GetType()} finished setup. </color>");
    }

    public void PlayCurrentSceneMusic()
    {
        _audioManager.PlaySceneMusic(_sceneType);
    }
}