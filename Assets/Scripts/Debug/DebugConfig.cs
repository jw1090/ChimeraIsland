using System.Collections;
using System;
using UnityEngine;

public class DebugConfig : MonoBehaviour, IGameModule
{
    public static event Action DebugConfigLoaded = null;

    [SerializeField] private bool _enableTutorials = true;
    [SerializeField] private bool _enableDebugTutorialInput = false;

    public bool TutorialsEnabled { get => _enableTutorials; }
    public bool DebugTutorialInputEnabled { get => _enableDebugTutorialInput; }

    public IEnumerator LoadModule()
    {
        Debug.Log($"<color=Yellow> Loading {this.GetType()} Module </color>");
        ServiceLocator.Register<DebugConfig>(this);
        DebugConfigLoaded?.Invoke();
        yield return null;
    }
}