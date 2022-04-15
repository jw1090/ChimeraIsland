using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : AsyncLoader
{
    [SerializeField] private int sceneIndexToLoad = 1;
    public List<Component> GameModules;

    private static int _sceneIndex = 1;
    private static GameLoader _instance;
    
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
            Debug.Log($"Scene index to load is set to {_sceneIndex}");
            _sceneIndex = sceneIndexToLoad;
        }

		// Setup System GameObject
		GameObject systemsGO = new GameObject("[Services]");
		Transform systemsParent = systemsGO.transform;
		DontDestroyOnLoad(systemsGO);

        // Queue up loading routines
        Enqueue(IntializeCoreSystems(systemsParent), 1);
		Enqueue(InitializeModularSystems(systemsParent), 2);

		// Set completion callback
		CallOnComplete(OnComplete);
	}

	private IEnumerator IntializeCoreSystems(Transform systemsParent)
	{
        Debug.Log("Loading Core Systems");

        var persistentDataGO = new GameObject("Persistent Data Manager");
        persistentDataGO.transform.SetParent(systemsParent);
        var persistentDataComp = persistentDataGO.AddComponent<PersistentData>().Initialize();
        ServiceLocator.Register<IPersistentData>(persistentDataComp);

        var sessionDataGO = new GameObject("Session Data Manager");
        sessionDataGO.transform.SetParent(systemsParent);
        var sessionDataComp = sessionDataGO.AddComponent<SessionData>().Initialize();
        ServiceLocator.Register<ISessionData>(sessionDataComp);

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
        Debug.Log($"GameLoader -> Starting Scene Load: {index}");        
        yield return SceneManager.LoadSceneAsync(index);
    }

	// AsyncLoader completion callback
	private void OnComplete()
	{
		Debug.Log("GameLoader Finished Initializing");
        StartCoroutine(LoadInitialScene(_sceneIndex));
    }
}