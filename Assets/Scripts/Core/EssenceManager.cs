using UnityEngine;

public class EssenceManager : MonoBehaviour
{
    private PersistentData _persistentData = null;
    private UIManager _uIManager = null;
    private bool _isInitialized = false;
    private int _currentEssence = 100;

    public int CurrentEssence { get => _currentEssence; }
    public bool IsInitialized { get => _isInitialized; }

    public EssenceManager Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();
        _uIManager = ServiceLocator.Get<UIManager>();

        LoadEssence();

        if (_uIManager != null)
        {
            _uIManager.UpdateWallets();
        }

        _isInitialized = true;

        return this;
    }

    public void IncreaseEssence(int amount)
    {
        _currentEssence += amount;
        _uIManager.UpdateWallets();
    }

    // Spends Essence and detects if you can afford it. Return false if you cannot afford and return true if you can.
    public bool SpendEssence(int amount)
    {
        if (_currentEssence - amount < 0)
        {
            return false;
        }

        _currentEssence -= amount;
        _uIManager.UpdateWallets();

        return true;
    }

    public void LoadEssence()
    {
        _currentEssence = _persistentData.EssenceData;
        Debug.Log($"Loaded Essense: {_currentEssence}");
    }
}