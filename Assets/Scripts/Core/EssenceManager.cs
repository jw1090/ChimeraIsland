using UnityEngine;

public class EssenceManager : MonoBehaviour
{
    public int CurrentEssence { get; private set; } = 0;
    //private IPersistentData _persistentData = null;

    public EssenceManager Initialize()
    {
        LoadEssence();
        Debug.Log("<color=Orange> Initializing Essence Manager ... </color>");
        //_persistentData = ServiceLocator.Get<IPersistentData>();

        return this;
    }

    public void IncreaseEssence(int amount)
    {
        CurrentEssence += amount;
        ServiceLocator.Get<UIManager>().UpdateWallets();
        GlobalSaveData data = new GlobalSaveData(CurrentEssence);
        FileHandler.SaveToJSON(data, GameConsts.JsonSaveKeys.GLOBAL_SAVE_DATA_FILE);
        /*
        if (_persistentData != null)
        {
            // Debug.Log("<color=lime>Saving Essence</color>");
        }
        else if (_persistentData == null)
        {
            _persistentData = ServiceLocator.Get<IPersistentData>() as PersistentData;
            // Debug.Log("<color=lime>PERSISTANT DATA IS NULL AAAAAAA</color>");
        }
        */
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