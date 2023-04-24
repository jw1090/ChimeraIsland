using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : AsyncLoader
{
    [SerializeField] private SceneType _sceneType = SceneType.None;

    [Header("Standard")]
    [SerializeField] private CameraUtil _cameraUtil = null;

    [Header("Starter")]
    [SerializeField] private StarterEnvironment _starterEnvironment = null;

    [Header("Habitat")]
    [SerializeField] private Habitat _habitat = null;
    [SerializeField] private LightingManager _lightingManager = null;
    [SerializeField] private ExpeditionManager _expeditionManager = null;

    [Header("Temple")]
    [SerializeField] private Temple _templeEnvironment = null;

    [Header("Builder")]
    [SerializeField] private EvolutionBuilder _evolutionBuilder = null;

    private static LevelLoader _instance = null;
    private readonly static List<Action> _queuedCallbacks = new List<Action>();

    private HabitatManager _habitatManager = null;
    private InputManager _inputManager = null;
    private TutorialManager _tutorialManager = null;
    private UIManager _uiManager = null;
    private AudioManager _audioManager = null;
    private SceneChanger _sceneChanger = null;

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

        _sceneChanger.RecentSceneChange = false;
        LoadUIElements();
        _inputManager.DisableOutline(false);

        switch (_sceneType)
        {
            case SceneType.MainMenu:
            case SceneType.Starting:
                break;
            case SceneType.Habitat:
                HabitatSceneSetup();
                break;
            case SceneType.Temple:
                TempleSceneSetup();
                break;
            case SceneType.Builder:
                _evolutionBuilder.BuildAll();
                _uiManager.EvolutionBuilderUI.LoadBaseChimeras();
                break;
            default:
                Debug.LogError($"{_sceneType} is invalid, please change!.");
                break;
        }

        PlaySceneLoopingAudio();

        CallOnComplete(OnComplete);
    }

    private void Initialize()
    {
        ServiceLocator.Register<LevelLoader>(this, true);

        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _habitatManager.SetCurrentHabitat(_habitat);

        _inputManager = ServiceLocator.Get<InputManager>();
        _inputManager.SetCurrentScene(_sceneType);

        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
        _uiManager = ServiceLocator.Get<UIManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        if (_cameraUtil != null)
        {
            ServiceLocator.Register<CameraUtil>(_cameraUtil.Initialize(_sceneType), true);
            _inputManager.SetCameraUtil(_cameraUtil);
            _uiManager.SettingsUI.SetCameraUtil(_cameraUtil);
            _habitatManager.SetCamera(_cameraUtil);
        }

        if (_habitat != null)
        {
            _habitat.Initialize();
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
            _lightingManager.Initialize();
            _habitat.SetLightingManager(_lightingManager);
        }

        if (_evolutionBuilder != null)
        {
            _evolutionBuilder.Initialize();
            _uiManager.EvolutionBuilderUI.SetEvolutionBuilder(_evolutionBuilder);
        }

        if (_starterEnvironment != null)
        {
            _uiManager.StartingUI.SetCameraUtil(_cameraUtil);
            _cameraUtil.SetStarterEnvironment(_starterEnvironment);
            _inputManager.SetAudioManager(_audioManager);
        }

        if (_templeEnvironment != null)
        {
            ServiceLocator.Register<Temple>(_templeEnvironment.Initialize(), true);
            _inputManager.SetTemple(_templeEnvironment);
            _uiManager.TempleUI.SetCameraUtil(_cameraUtil);
            _cameraUtil.SetTempleEnvironment(_templeEnvironment);
        }
    }

    private void HabitatSceneSetup()
    {
        _cameraUtil.SceneSetup();

        TempleBuildCheck();
        _habitatManager.HabitatSetup();

        _habitatManager.CurrentHabitat.MoveChimerasToFacility();

        StartHabitatTickTimer();

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

    private void TempleSceneSetup()
    {
        _cameraUtil.SceneSetup();
        _templeEnvironment.SceneSetup();
        _uiManager.TempleUI.EnteringTempleTransition();
    }

    private void LoadUIElements()
    {
        if (_uiManager == null)
        {
            return;
        }

        switch (_sceneType)
        {
            case SceneType.MainMenu:
                break;
            case SceneType.Starting:
                _uiManager.StartingUI.OnSceneStart();
                break;
            case SceneType.Habitat:
                _uiManager.HabitatUI.LoadHabitatSpecificUI();
                break;
            case SceneType.Temple:
                _uiManager.TempleUI.ShowSharedUIState();
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
        StartCoroutine(_uiManager.FadeOutLoadingScreen());
    }

    public void PlaySceneLoopingAudio()
    {
        _audioManager.PlaySceneMusic(_sceneType);
        _audioManager.PlaySceneAmbience(_sceneType);
    }
}