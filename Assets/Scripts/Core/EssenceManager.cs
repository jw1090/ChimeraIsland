using UnityEngine;

public class EssenceManager : MonoBehaviour
{
    private UIManager _uIManager = null;
    private bool _isInitialized = false;
    private PersistentData _persistentData = null;

    public int CurrentEssence { get; private set; } = 100;
    public bool IsInitialized { get => _isInitialized; }

    public EssenceManager Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");
        LoadEssence();

        _uIManager = ServiceLocator.Get<UIManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();
        if (_uIManager != null)
        {
            _uIManager.UpdateWallets();
        }

        _isInitialized = true;
        return this;
    }

    public void IncreaseEssence(int amount)
    {
        CurrentEssence += amount;
        _uIManager.UpdateWallets();
    }

    // Spends Essence and detects if you can afford it. Return false if you cannot afford and return true if you can.
    public bool SpendEssence(int amount)
    {
        if (CurrentEssence - amount < 0)
        {
            return false;
        }

        CurrentEssence -= amount;
        _uIManager.UpdateWallets();

        return true;
    }

    public void SaveEssence()
    {
        GlobalSaveData data = new GlobalSaveData(CurrentEssence);
        FileHandler.SaveToJSON(data, GameConsts.JsonSaveKeys.GLOBAL_SAVE_DATA_FILE);
    }

    public void LoadEssence()
    {
        CurrentEssence = _persistentData.GetEssenceData();
        Debug.Log($"Loaded Essense: {CurrentEssence}");
    }
}