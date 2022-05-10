using UnityEngine;

public class EssenceManager : MonoBehaviour
{
    public int CurrentEssence { get; private set; } = 0;
    public bool IsInitialized { get; set; } = false;

    public EssenceManager Initialize()
    {
        Debug.Log("<color=Orange> Initializing Essence Manager ... </color>");
        LoadEssence();
        
        IsInitialized = true;
        return this;
    }

    public void IncreaseEssence(int amount)
    {
        CurrentEssence += amount;
        ServiceLocator.Get<UIManager>().UpdateWallets();
        GlobalSaveData data = new GlobalSaveData(CurrentEssence);
        FileHandler.SaveToJSON(data, GameConsts.JsonSaveKeys.GLOBAL_SAVE_DATA_FILE);
    }

    // Spends Essence and detects if you can afford it. Return false if you cannot afford and return true if you can.
    public bool SpendEssence(int amount)
    {
        if (CurrentEssence - amount < 0)
        {
            return false;
        }

        CurrentEssence -= amount;
        ServiceLocator.Get<UIManager>().UpdateWallets();

        return true;
    }

    // Loads essence from 
    public void LoadEssence()
    {
        GlobalSaveData temp = FileHandler.ReadFromJSON<GlobalSaveData>(GameConsts.JsonSaveKeys.GLOBAL_SAVE_DATA_FILE);
        CurrentEssence = temp == null ? 0 : temp.currentEssence;
        Debug.Log($"Loaded Essense: {CurrentEssence}");
    }
}