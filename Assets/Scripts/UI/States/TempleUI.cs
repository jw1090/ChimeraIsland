using UnityEngine;

public class TempleUI : MonoBehaviour
{
    [SerializeField] MarketplaceUI _marketplaceUI = null;

    public void Initialize(UIManager uiManager)
    {
        _marketplaceUI.Initialize(uiManager);
    }
}