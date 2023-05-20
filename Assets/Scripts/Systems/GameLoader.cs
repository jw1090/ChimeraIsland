﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : AsyncLoader
{
    [SerializeField] private int sceneIndexToLoad = 1;
    public List<Component> GameModules = new List<Component>();
    private static GameLoader _instance = null;
    private static int _sceneIndex = 1;

    public static Transform SystemsParent { get => _systemsParent; }
    private static Transform _systemsParent = null;

    private readonly static List<Action> _queuedCallbacks = new List<Action>();
    protected override void Awake()
    {
        Debug.Log("GameLoader Starting");

        // Saftey check
        if (_instance != null && _instance != this)
        {
            Debug.Log("A duplicate instance of the GameLoader was found, and will be ignored. Only one instance is permitted");
            Destroy(gameObject);
            return;
        }

        // Set reference to this instance
        _instance = this;

        // Make persistent
        DontDestroyOnLoad(gameObject);

        // Scene Index Check
        if (sceneIndexToLoad < 0 || sceneIndexToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Invalid Scene Index {sceneIndexToLoad} ... using default value of {_sceneIndex}");
        }
        else
        {
            Debug.Log($"Scene index to load is set to {sceneIndexToLoad}");
            _sceneIndex = sceneIndexToLoad;
        }

        // Setup System GameObject
        GameObject systemsGO = new GameObject("[Services]");
        _systemsParent = systemsGO.transform;
        DontDestroyOnLoad(systemsGO);

        // Queue up loading routines
        Enqueue(IntializeCoreSystems(_systemsParent), 1);
        Enqueue(InitializeModularSystems(_systemsParent), 2);

        // Check for any CallOnComplete callbacks that were queued through Awake before this instance was made
        ProcessQueuedCallbacks();

        // Because Unity can hold onto static values between sessions.
        ResetVariables();

        // Set completion callback
        GameLoader.CallOnComplete(OnComplete);
    }

    private IEnumerator IntializeCoreSystems(Transform systemsParent)
    {
        Debug.Log("Loading Core Systems");

        var resourceManagerGO = new GameObject("Resource Manager");
        resourceManagerGO.transform.SetParent(systemsParent);
        var resourceManagerComp = resourceManagerGO.AddComponent<ResourceManager>().Initialize();
        ServiceLocator.Register<ResourceManager>(resourceManagerComp);

        var monoUtilGO = new GameObject("Monobehaviour Utility");
        monoUtilGO.transform.SetParent(systemsParent);
        var monoUtilComp = monoUtilGO.AddComponent<MonoUtil>().Initialize();
        ServiceLocator.Register<MonoUtil>(monoUtilComp);

        var persistentDataGO = new GameObject("Persistent Data Manager");
        persistentDataGO.transform.SetParent(systemsParent);
        var persistentDataComp = persistentDataGO.AddComponent<PersistentData>().Initialize();
        ServiceLocator.Register<PersistentData>(persistentDataComp);

        var sessionDataGO = new GameObject("Session Data Manager");
        sessionDataGO.transform.SetParent(systemsParent);
        var sessionDataComp = sessionDataGO.AddComponent<SessionData>().Initialize();
        ServiceLocator.Register<ISessionData>(sessionDataComp);

        var inputManagerGO = Instantiate(resourceManagerComp.InputManager, systemsParent);
        inputManagerGO.name = "Input Manager";
        var inputManagerComp = inputManagerGO.GetComponent<InputManager>().Initialize();
        ServiceLocator.Register<InputManager>(inputManagerComp);

        var currencyManagerGO = new GameObject("Currency Manager");
        currencyManagerGO.transform.SetParent(systemsParent);
        var currencyManagerComp = currencyManagerGO.AddComponent<CurrencyManager>().Initialize();
        ServiceLocator.Register<CurrencyManager>(currencyManagerComp);
        persistentDataComp.SetCurrencyManager(currencyManagerComp);
        inputManagerComp.SetCurrencyManager(currencyManagerComp);

        var questManagerGo = Instantiate(resourceManagerComp.QuestManager, systemsParent);
        questManagerGo.name = "Quest Manager";
        var questManagerComp = questManagerGo.GetComponent<QuestManager>().Initialize();
        ServiceLocator.Register<QuestManager>(questManagerComp);

        var habitatManagerGO = new GameObject("Habitat Manager");
        habitatManagerGO.transform.SetParent(systemsParent);
        var habitatManagerComp = habitatManagerGO.AddComponent<HabitatManager>().Initialize();
        ServiceLocator.Register<HabitatManager>(habitatManagerComp);
        persistentDataComp.SetHabitatManager(habitatManagerComp);
        inputManagerComp.SetHabitatManager(habitatManagerComp);
        questManagerComp.SetHabitatManager(habitatManagerComp);
        questManagerComp.LoadActiveQuests();

        var tutorialGO = new GameObject("Tutorial Manager");
        tutorialGO.transform.SetParent(systemsParent);
        var tutorialComp = tutorialGO.AddComponent<TutorialManager>().Initialize();
        ServiceLocator.Register<TutorialManager>(tutorialComp);
        persistentDataComp.SetTutorialManager(tutorialComp);

        var chimeraCreatorGO = new GameObject("Chimera Creator");
        chimeraCreatorGO.transform.SetParent(systemsParent);
        var chimeraCreatorComp = chimeraCreatorGO.AddComponent<ChimeraCreator>().Initialize();
        ServiceLocator.Register<ChimeraCreator>(chimeraCreatorComp);

        var sceneChangerGO = new GameObject("Scene Changer");
        sceneChangerGO.transform.SetParent(systemsParent);
        var sceneChangerComp = sceneChangerGO.AddComponent<SceneChanger>().Initialize();
        ServiceLocator.Register<SceneChanger>(sceneChangerComp);
        inputManagerComp.SetSceneChanger(sceneChangerComp);

        var uiManagerGO = Instantiate(resourceManagerComp.UIManager, systemsParent);
        uiManagerGO.name = "UI Manager";
        var uiManagerComp = uiManagerGO.GetComponent<UIManager>().Initialize();
        ServiceLocator.Register<UIManager>(uiManagerComp);

        inputManagerComp.SetUIManager(uiManagerComp);
        inputManagerComp.SetQuestManager(questManagerComp);
        currencyManagerComp.SetUIManager(uiManagerComp);
        questManagerComp.SetUIManager(uiManagerComp);
        habitatManagerComp.SetHabitatUI(uiManagerComp);
        tutorialComp.SetHabitatUI(uiManagerComp);

        sceneChangerComp.SetupUIListeners();

        yield return null;
    }

    private IEnumerator InitializeModularSystems(Transform systemsParent)
    {
        // Setup Additional Systems as needed
        Debug.Log("Loading Modular Systems");
        foreach (Component c in GameModules)
        {
            if (c is IGameModule)
            {
                IGameModule module = (IGameModule)c;
                yield return module.LoadModule();
            }
        }

        yield return null;
    }

    private IEnumerator LoadInitialScene(int index)
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (index != activeSceneIndex)
        {
            Debug.Log($"GameLoader -> Starting Scene Load: {index}");
            yield return SceneManager.LoadSceneAsync(index);
        }
        else
        {
            // We are already have the desired scene loaded.
            Debug.Log("GameLoader -> Skipping Scene Load: Scene is already active");
            yield break;
        }
    }

    protected override void ResetVariables()
    {
        base.ResetVariables();
    }

    public static void CallOnComplete(Action callback)
    {
        if (!_instance)
        {
            _queuedCallbacks.Add(callback);
            return;
        }

        _instance.CallOnComplete_Internal(callback);
    }

    private void ProcessQueuedCallbacks()
    {
        foreach (var callback in _queuedCallbacks)
        {
            callback?.Invoke();
        }
        _queuedCallbacks.Clear();
    }

    // AsyncLoader completion callback
    private void OnComplete()
    {
        Debug.Log("GameLoader Finished Initializing");
        StartCoroutine(LoadInitialScene(_sceneIndex));
    }
}