using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HabitatUI : MonoBehaviour
{
    [Header("Scene Changing")]
    [SerializeField] private Button _mainMenuButton = null;
    [SerializeField] private Button _worldMapButton = null;

    [Header("Elements")]
    [SerializeField] private Button _openDetailsButton = null;
    [SerializeField] private Button _closeDetailsButton = null;
    [SerializeField] private Button _expeditionButton = null;
    [SerializeField] private Button _marketplaceButton = null;
    [SerializeField] private GameObject _standardUI = null;
    [SerializeField] private GameObject _detailsButtons = null;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder = null;
    [SerializeField] private GameObject _expeditionPanel = null;
    [SerializeField] private GameObject _settingsPanel = null;
    [SerializeField] private Marketplace _marketplacePanel = null;
    [SerializeField] private TransferMap _transferMap = null;
    [SerializeField] private UITutorialOverlay _tutorialOverlay = null;
    [SerializeField] private ReleaseSlider _releaseSlider = null;
    [SerializeField] private List<UIWallet> _essenceWallets = new List<UIWallet>();

    private TutorialManager _tutorialManager = null;

    public Button MainMenuButton { get => _mainMenuButton; }
    public Button WorldMapButton { get => _worldMapButton; }
    public ReleaseSlider ReleaseSlider { get => _releaseSlider; }

    public void Initialize()
    {
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        InitializeWallets();
        InitializeTutorialOverlay();
    }

    private void InitializeWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.Initialize();
        }
    }

    private void InitializeTutorialOverlay()
    {
        if (_tutorialOverlay != null)
        {
            _tutorialOverlay.Initialize(this);
        }
    }

    public void LoadHabitatSpecificUI()
    {
        _marketplacePanel.Initialize();
        _detailsFolder.Initialize();
        _transferMap.Initialize();

        ResetUI();
    }

    public void EnableTutorialUIByType(TutorialUIElementType uiElementType)
    {
        switch (uiElementType)
        {
            case TutorialUIElementType.None:
                break;
            case TutorialUIElementType.All:
                _expeditionButton.gameObject.SetActive(true);
                _marketplaceButton.gameObject.SetActive(true);
                _detailsButtons.gameObject.SetActive(true);
                _openDetailsButton.gameObject.SetActive(true);
                _worldMapButton.gameObject.SetActive(true);
                _marketplacePanel.ChimeraTabSetActive(true);
                break;
            case TutorialUIElementType.ExpeditionButton:
                _expeditionButton.gameObject.SetActive(true);
                break;
            case TutorialUIElementType.MarketplaceButton:
                _marketplaceButton.gameObject.SetActive(true);
                break;
            case TutorialUIElementType.MarketplaceChimeraTab:
                _marketplacePanel.ChimeraTabSetActive(true);
                break;
            case TutorialUIElementType.OpenDetailsButton:
                _detailsButtons.gameObject.SetActive(true);
                _openDetailsButton.gameObject.SetActive(true);
                break;
            case TutorialUIElementType.WorldMapButton:
                _worldMapButton.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError($"{uiElementType} is invalid. Please change!");
                break;
        }
    }

    public void DisableUI()
    {
        _expeditionButton.gameObject.SetActive(false);
        _marketplaceButton.gameObject.SetActive(false);
        _openDetailsButton.gameObject.SetActive(false);
        _worldMapButton.gameObject.SetActive(false);
        _closeDetailsButton.gameObject.SetActive(false);
        _marketplaceButton.gameObject.SetActive(false);
        _detailsButtons.gameObject.SetActive(false);
        _marketplacePanel.ChimeraTabSetActive(false);
    }

    // Resetting to the standard UI when nothing has been disabled.
    public void ResetUI()
    {
        _openDetailsButton.gameObject.SetActive(true);
        _standardUI.gameObject.SetActive(true);

        _closeDetailsButton.gameObject.SetActive(false);
        _detailsFolder.gameObject.SetActive(false);
        _marketplacePanel.gameObject.SetActive(false);
        _settingsPanel.gameObject.SetActive(false);
        _expeditionPanel.gameObject.SetActive(false);
        _transferMap.gameObject.SetActive(false);
    }

    public void OpenDetailsPanel()
    {
        _detailsFolder.CheckDetails();

        ResetUI();
        _closeDetailsButton.gameObject.SetActive(true);
        _detailsFolder.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
    }

    public void OpenMarketplace()
    {
        ResetUI();
        _marketplacePanel.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
    }

    public void OpenTransferMap(Chimera chimera)
    {
        ResetUI();
        _transferMap.Open(chimera);
    }

    public void ToggleSettingsMenu()
    {
        if (_settingsPanel.activeInHierarchy == true ||
            _marketplacePanel.gameObject.activeInHierarchy == true ||
            _expeditionPanel.gameObject.activeInHierarchy == true)
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
        _settingsPanel.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
        _standardUI.gameObject.SetActive(false);
    }

    public void OpenExpedition()
    {
        ResetUI();
        _expeditionPanel.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
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

    public void StartTutorial(TutorialStageData tutorialSteps)
    {
        _tutorialOverlay.gameObject.SetActive(true);
        _tutorialOverlay.ShowOverlay(tutorialSteps);
    }

    public void EndTutorial()
    {
        _tutorialOverlay.gameObject.SetActive(false);
        _tutorialManager.SaveTutorialProgress();
    }
}