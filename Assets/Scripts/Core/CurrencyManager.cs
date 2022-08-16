using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private HabitatUI _habitatUI = null;
    private PersistentData _persistentData = null;
    private bool _currencyLoaded = false;
    private int _essence = 100;
    private int _fossils = 0;

    public int Essence { get => _essence; }
    public int Fossils { get => _fossils; }

    public void SetHabitatUI(HabitatUI habiatUI) { _habitatUI = habiatUI; }

    public CurrencyManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();
        LoadCurrency();

        return this;
    }

    public void LoadCurrency()
    {
        if (_persistentData != null && _currencyLoaded == false)
        {
            // Craig's Note: be careful when using properties that implement 'get' calls that are not exception safe, like this one.
            _essence = _persistentData.EssenceData;
            _fossils = _persistentData.FossilData;
            _currencyLoaded = true;
        }
    }

    public void ResetCurrency()
    {
        _essence = 100;
        _fossils = 0;

        if (_habitatUI != null)
        {
            _habitatUI.UpdateEssenceWallets();
            _habitatUI.UpdateFossilWallets();
        }
    }

    public void IncreaseEssence(int amount)
    {
        _essence += amount;
        _habitatUI.UpdateEssenceWallets();
    }

    public void IncreaseFossils(int amount)
    {
        _fossils += amount;
        _habitatUI.UpdateFossilWallets();
    }

    public bool SpendEssence(int amount)
    {
        if (_essence - amount < 0)
        {
            return false;
        }

        _essence -= amount;

        if (_habitatUI != null)
        {
            _habitatUI.UpdateEssenceWallets();
        }

        return true;
    }

    public bool SpendFossils(int amount)
    {
        if (_fossils - amount < 0)
        {
            return false;
        }

        _fossils -= amount;

        if (_habitatUI != null)
        {
            _habitatUI.UpdateFossilWallets();
        }

        return true;
    }
}