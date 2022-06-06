using UnityEngine;

public class EssenceManager : MonoBehaviour
{
    private UIManager _uiManager = null;
    private int _currentEssence = 100;

    public int CurrentEssence { get => _currentEssence; }

    public void SetUIManager(UIManager uiManager) { _uiManager = uiManager; }

    public EssenceManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        return this;
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