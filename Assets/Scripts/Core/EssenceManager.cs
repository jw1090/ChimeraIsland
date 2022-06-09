using UnityEngine;

public class EssenceManager : MonoBehaviour
{
    private PersistentData _persistentData = null;
    private UIManager _uiManager = null;
    private bool _essenceLoaded = false;
    private int _currentEssence = 100;

    public int CurrentEssence { get => _currentEssence; }

    public void SetUIManager(UIManager uiManager) { _uiManager = uiManager; }

    public EssenceManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();
        LoadEssence();

        return this;
    }

    public void LoadEssence()
    {
        if (_persistentData != null && _essenceLoaded == false)
        {
            _currentEssence = _persistentData.EssenceData; // Craig's Note: be careful when using properties that implement 'get' calls that are not exception safe, like this one.
            _essenceLoaded = true;
        }
    }

    public void IncreaseEssence(int amount)
    {
        _currentEssence += amount;
        _uiManager.UpdateWallets();
    }

    public bool SpendEssence(int amount)
    {
        if (_currentEssence - amount < 0)
        {
            return false;
        }

        _currentEssence -= amount;

        if (_uiManager != null)
        {
            _uiManager.UpdateWallets();
        }

        return true;
    }

    public void UpdateEssence(int amount)
    {
        _currentEssence = amount;

        if (_uiManager != null)
        {
            _uiManager.UpdateWallets();
        }
    }
}