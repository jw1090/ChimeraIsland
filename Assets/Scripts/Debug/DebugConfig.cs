using System;
using System.Collections;
using UnityEngine;

public class DebugConfig : MonoBehaviour, IGameModule
{
    public static event Action DebugConfigLoaded = null;

    [SerializeField] private bool _enableTutorials = true;
    [SerializeField] private bool _enableDebugCurrencyInput = true;
    [SerializeField] private bool _enableDebugHabitatUpgradeInput = true;
    [SerializeField] private bool _enableDebugViewInput = true;
    [SerializeField] private int _debugEssenceGain = 100;
    [SerializeField] private int _debugFossilGain = 1;

    public bool TutorialsEnabled { get => _enableTutorials; }
    public bool DebugCurrencyInputEnabled { get => _enableDebugCurrencyInput; }
    public bool DebugHabitatUpgradeInputEnabled { get => _enableDebugHabitatUpgradeInput; }
    public bool EnableDebugViewInput { get => _enableDebugViewInput; }
    public int DebugEssenceGain { get => _debugEssenceGain; }
    public int DebugFossilGain { get => _debugFossilGain; }

    public IEnumerator LoadModule()
    {
        Debug.Log($"<color=Yellow> Loading {this.GetType()} Module </color>");

        ServiceLocator.Register<DebugConfig>(this);
        DebugConfigLoaded?.Invoke();

        yield return null;
    }
}