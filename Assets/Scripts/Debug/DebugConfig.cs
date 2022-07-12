using System.Collections;
using System;
using UnityEngine;

public class DebugConfig : MonoBehaviour, IGameModule
{
    public static event Action DebugConfigLoaded = null;

    [SerializeField] private bool _enableTutorials = true;
    public bool TutorialsEnabled { get { return _enableTutorials; } }

    public IEnumerator LoadModule()
    {
        Debug.Log("Loading DebugConfig Module");
        ServiceLocator.Register<DebugConfig>(this);
        DebugConfigLoaded?.Invoke();
        yield return null;
    }
}
