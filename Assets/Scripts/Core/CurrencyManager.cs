using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private UIManager _uiManager = null;
    private PersistentData _persistentData = null;
    private bool _currencyLoaded = false;
    private int _essence = 0;
    private int _fossils = 0;

    public int Essence { get => _essence; }
    public int Fossils { get => _fossils; }

    public void SetUIManager(UIManager uiManager) { _uiManager = uiManager; }

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
            _essence = _persistentData.EssenceData;
            _fossils = _persistentData.FossilData;
            _currencyLoaded = true;
        }
    }

    public void ResetCurrency()
    {
        _essence = 0;
        _fossils = 0;

        if (_uiManager != null)
        {
            _uiManager.UpdateEssenceWallets();
            _uiManager.UpdateFossilWallets();
        }
    }

    public void IncreaseEssence(int amount)
    {
        _essence += amount;
        _uiManager.UpdateEssenceWallets();
    }

    public void IncreaseFossils(int amount)
    {
        _fossils += amount;
        _uiManager.UpdateFossilWallets();
    }

    public bool SpendEssence(int amount)
    {
        if (_essence - amount < 0)
        {
            return false;
        }

        _essence -= amount;

        if (_uiManager != null)
        {
            _uiManager.UpdateEssenceWallets();
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

        if (_uiManager != null)
        {
            _uiManager.UpdateFossilWallets();
        }

        return true;
    }
}