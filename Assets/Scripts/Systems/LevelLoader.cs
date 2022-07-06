using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : AsyncLoader
{
    [SerializeField] private UIManager _uiManager = null;
    [SerializeField] private CameraController _cameraController = null;
    [SerializeField] private Habitat _habitat = null;

    private static LevelLoader _instance = null;
    private readonly static List<Action> _queuedCallbacks = new List<Action>();

    private EssenceManager _essenceManager = null;
    private HabitatManager _habitatManager = null;
    private InputManager _inputManager = null;
    private PersistentData _persistentData = null;
    private TutorialManager _tutorialManager = null;

    protected override void Awake()
    {
        // TODO: Craig is not happy with this <.< (It's all his fault though)
        _instance = this;
        GameLoader.CallOnComplete(LevelSetup);
    }

    private void OnDestroy()
    {
        ResetVariables();
    }

    private void LevelSetup()
    {
        Initialize();
        ProcessQueuedCallbacks();

        if (LastSessionHabitatCheck() == false) // Return false when there is no need to change habitat.
        {
            InitializeUIElements();

            if(_habitat != null)
            {
                LoadFacilities();
                LoadChimeras();
                StartHabitatTickTimer();
            }

            CallOnComplete(OnComplete);
        }

        _tutorialManager.SetupTutorial();
    }

    private void Initialize()
    {
        ServiceLocator.Register<LevelLoader>(this, true);

        _essenceManager = ServiceLocator.Get<EssenceManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _inputManager = ServiceLocator.Get<InputManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        if (_uiManager != null)
        {
            ServiceLocator.Register<UIManager>(_uiManager.Initialize(), true);
            _essenceManager.SetUIManager(_uiManager);
            _inputManager.SetUIManager(_uiManager);
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

    private void InitializeUIElements()
    {
        if (_uiManager == null)
        {
            return;
        }

        _uiManager.InitializeUIElements();
        _uiManager.ShowHabitatUI();
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

    private void ProcessQueuedCallbacks()
    {
        foreach(var callback in _queuedCallbacks)
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
        if(_instance == null)
        {
            _queuedCallbacks.Add(callback);
            return;
        }

        _instance.CallOnComplete_Internal(callback);
    }

    private void OnComplete()
    {
        Debug.Log($"{this.GetType()} finished setup.");
    }
}