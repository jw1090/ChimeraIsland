using UnityEngine;

public class EssenceManager : MonoBehaviour
{
    private UIManager _uIManager = null;

    public int CurrentEssence { get; private set; } = 50;
    public bool IsInitialized { get; set; } = false;

    public EssenceManager Initialize()
    {
        Debug.Log($"<color=Orange> {this.GetType()} Initialized!</color>");
        LoadEssence();

        _uIManager = ServiceLocator.Get<UIManager>();
        if(_uIManager != null)
        {
            _uIManager.UpdateWallets();
        }

        IsInitialized = true;
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
        GlobalSaveData temp = FileHandler.ReadFromJSON<GlobalSaveData>(GameConsts.JsonSaveKeys.GLOBAL_SAVE_DATA_FILE);
        CurrentEssence = temp == null ? CurrentEssence : temp.currentEssence;
        Debug.Log($"Loaded Essense: {CurrentEssence}");
    }
}
