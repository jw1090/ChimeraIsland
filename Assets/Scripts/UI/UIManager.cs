using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _openDetailsButton = null;
    [SerializeField] private Button _closeDetailsButton = null;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder = null;
    [SerializeField] private Button _marketplaceButton = null;
    [SerializeField] private Marketplace _marketplace = null;
    [SerializeField] private GameObject _settingsMenu = null;
    [SerializeField] private TransferMap _transferMap = null;
    [SerializeField] private ReleaseSlider _releaseSlider = null;
    [SerializeField] private UITutorialOverlay _tutorialOverlay = null;
    [SerializeField] private UIWallet[] _essenceWallets = null;
    private UISceneChange _sceneChange = null;

    public ReleaseSlider ReleaseSlider { get => _releaseSlider; }
    public UITutorialOverlay TutorialOverlay { get => _tutorialOverlay; }

    public UIManager Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        // Setup the Tutorial Overlay
        if(_tutorialOverlay != null)
        {
            _tutorialOverlay.Initialize();
        }
        else
        {
            Debug.Log($"UITutorialOverlay is null. Check inspector reference");
        }

        _sceneChange = GetComponent<UISceneChange>();
        ResetUI();
        _sceneChange.Initialize();

        return this;
    }

    public void InitializeUIElements()
    {
        InitializeWallets();
        _marketplace.Initialize();
        _detailsFolder.Initialize();
        _transferMap.Initialize();
    }

    private void InitializeWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.Initialize();
        }
    }

    public void ResetUI()
    {
        _openDetailsButton.gameObject.SetActive(true);
        _marketplaceButton.gameObject.SetActive(true);

        _closeDetailsButton.gameObject.SetActive(false);
        _detailsFolder.gameObject.SetActive(false);
        _marketplace.gameObject.SetActive(false);
        _settingsMenu.gameObject.SetActive(false);
        _transferMap.gameObject.SetActive(false);
    }

    public void OpenDetailsPanel()
    {
        _detailsFolder.CheckDetails();

        ResetUI();
        _openDetailsButton.gameObject.SetActive(false);
        _closeDetailsButton.gameObject.SetActive(true);
        _detailsFolder.gameObject.SetActive(true);
    }

    public void OpenMarketplace()
    {
        ResetUI();
        _openDetailsButton.gameObject.SetActive(false);
        _marketplace.gameObject.SetActive(true);
    }

    public void OpenTransferMap(Chimera chimera)
    {
        ResetUI();
        _transferMap.Open(chimera);
    }

    public void ToggleSettingsMenu()
    {
        if (_settingsMenu.activeInHierarchy == true)
        {
            ResetUI();
        }
        else
        {
            OpenSettingsMenu();
        }
    }

    public void OpenSettingsMenu()
    {
        ResetUI();
        _settingsMenu.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
        _marketplaceButton.gameObject.SetActive(false);
    }

    public void UpdateDetails()
    {
        _detailsFolder.UpdateDetailsList();
    }

    public void UpdateWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.UpdateWallet();
        }
    }

    public void StartTutorial(TutorialSteps tutorialSteps)
    {
        _tutorialOverlay.gameObject.SetActive(true);
        _tutorialOverlay.ShowOverlay(tutorialSteps);
    }
}