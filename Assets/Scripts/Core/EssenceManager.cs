using UnityEngine;

public class EssenceManager : MonoBehaviour
{
    private HabitatUI _habitatUI = null;
    private PersistentData _persistentData = null;
    private bool _essenceLoaded = false;
    private int _currentEssence = 100;

    public int CurrentEssence { get => _currentEssence; }

    public void SetHabitatUI(HabitatUI habiatUI) { _habitatUI = habiatUI; }

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

    public void ResetEssence()
    {
        _currentEssence = 100;
        if (_habitatUI != null)
        {
            _habitatUI.UpdateWallets();
        }
    }

    public void IncreaseEssence(int amount)
    {
        _currentEssence += amount;
        _habitatUI.UpdateWallets();
    }

    public bool SpendEssence(int amount)
    {
        if (_currentEssence - amount < 0)
        {
            return false;
        }

        _currentEssence -= amount;

        if (_habitatUI != null)
        {
            _habitatUI.UpdateWallets();
        }

        return true;
    }
}