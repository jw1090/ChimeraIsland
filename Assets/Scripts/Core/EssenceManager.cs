using UnityEngine;

public class EssenceManager : MonoBehaviour
{
    public int CurrentEssence { get; private set; } = 0;
    //private IPersistentData _persistentData = null;

    public EssenceManager Initialize()
    {
        Debug.Log("<color=Orange> Initializing Essence Manager ... </color>");

        //_persistentData = ServiceLocator.Get<IPersistentData>();

        return this;
    }

    public void IncreaseEssence(int amount)
    {
        CurrentEssence += amount;
        ServiceLocator.Get<UIManager>().UpdateWallets();
        FileHandler.SaveToJSON(CurrentEssence, GameConsts.GameSaveKeys.ESSENCE.ToString());
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

        CurrentEssence = FileHandler.ReadFromJSON<int>(GameConsts.GameSaveKeys.ESSENCE.ToString());
        /*
        if (_persistentData != null)
        {
            CurrentEssence = _persistentData.LoadDataInt(GameConsts.GameSaveKeys.ESSENCE);
        }
        */
        Debug.Log($"Loaded Essense: {CurrentEssence}");
    }
}