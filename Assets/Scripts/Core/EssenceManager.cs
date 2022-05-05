using System;
using UnityEngine;

public class EssenceManager : MonoBehaviour
{
    public int CurrentEssence { get; private set; } = 0;
    private IPersistentData _persistentData = null;
    public bool IsInitialized { get; set; } = false;

    public EssenceManager Initialize()
    {
        Debug.Log("<color=Orange> Initializing Essence Manager ... </color>");
        _persistentData = ServiceLocator.Get<IPersistentData>();
        IsInitialized = true;
        return this;
    }

    public void IncreaseEssence(int amount)
    {
        CurrentEssence += amount;
        ServiceLocator.Get<UIManager>().UpdateWallets();
        if (_persistentData != null)
        {
            // Debug.Log("<color=lime>Saving Essence</color>");
            _persistentData.SaveData(GameConsts.GameSaveKeys.ESSENCE, CurrentEssence);
        }
        else if (_persistentData == null)
        {
            _persistentData = ServiceLocator.Get<IPersistentData>() as PersistentData;
            // Debug.Log("<color=lime>PERSISTANT DATA IS NULL AAAAAAA</color>");
        }
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
        if (_persistentData != null)
        {
            CurrentEssence = _persistentData.LoadDataInt(GameConsts.GameSaveKeys.ESSENCE);
            Debug.Log($"Loaded Essense: {CurrentEssence}");
        }
    }
}