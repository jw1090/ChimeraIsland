using UnityEngine;

public class TempleUI : MonoBehaviour
{
    [SerializeField] private UIEssenceWallet _essenceWallet = null;
    [SerializeField] private UIFossilWallet _fossilWallet = null;

    public void Initialize(UIManager uiManager)
    {
        
    }

    public void InitializeWallets()
    {
        _essenceWallet.Initialize();
        _fossilWallet.Initialize();
    }

    public void UpdateEssenceWallets()
    {
        _essenceWallet.UpdateWallet();
    }

    public void UpdateFossilWallets()
    {
        _fossilWallet.UpdateWallet();
    }
}