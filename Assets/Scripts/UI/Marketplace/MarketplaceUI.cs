using UnityEngine;
using UnityEngine.UI;

public class MarketplaceUI : MonoBehaviour
{
    [SerializeField] private TabGroup _tabGroup = null;
    [SerializeField] private ChimeraShop _chimeraShop = null;
    [SerializeField] private Button _closeButton = null;
    private UIManager _uiManager = null;

    public Button CloseButton { get => _closeButton; } 

    public void Initialize(UIManager uiManager)
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        _uiManager = uiManager;

        _tabGroup.Initialize();
        _chimeraShop.Initialize();

        SetupListeners();
    }

    private void SetupListeners()
    {
        _uiManager.CreateButtonListener(_closeButton, _uiManager.HabitatUI.ResetStandardUI);
    }
}