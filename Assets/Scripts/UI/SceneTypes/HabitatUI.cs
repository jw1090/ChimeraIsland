using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HabitatUI : MonoBehaviour
{
    [Header("Scene Changing")]
    [SerializeField] private Button _mainMenuButton = null;
    [SerializeField] private Button _quitGameButotn = null;
    [SerializeField] private Button _worldMapButton = null;

    [Header("Elements")]
    [SerializeField] private Button _openDetailsButton = null;
    [SerializeField] private Button _closeDetailsButton = null;
    [SerializeField] private Button _marketplaceButton = null;
    [SerializeField] private Button _expeditionButton = null;
    [SerializeField] private GameObject _topLeftButtonsFolder = null;
    [SerializeField] private GameObject _waterfallFacilityShopIcon = null;
    [SerializeField] private GameObject _runeFacilityShopIcon = null;
    [SerializeField] private GameObject _caveFacilityShopIcon = null;
    [SerializeField] private GameObject _standardUI = null;
    [SerializeField] private GameObject _detailsButtons = null;
    [SerializeField] private UIExpedition _expeditionPanel = null;
    [SerializeField] private GameObject _settingsPanel = null;
    [SerializeField] private GameObject _detailsPanel = null;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder = null;
    [SerializeField] private Marketplace _marketplacePanel = null;
    [SerializeField] private TransferMap _transferMap = null;
    [SerializeField] private UIVolumeSettings _volumeSettings = null;
    [SerializeField] private ReleaseSlider _releaseSlider = null;
    [SerializeField] private UITraining _trainingPanel = null;
    [SerializeField] private List<UIEssenceWallet> _essenceWallets = new List<UIEssenceWallet>();
    [SerializeField] private List<UIFossilWallet> _fossilWallets = new List<UIFossilWallet>();

    private AudioManager _audioManager = null;

    public Button MainMenuButton { get => _mainMenuButton; }
    public Button QuitGameButton { get => _quitGameButotn; }
    public Button WorldMapButton { get => _worldMapButton; }
    public Button MarketplaceButton { get => _marketplaceButton; }
    public Button ExpeditionButton { get => _expeditionButton; }
    public Button WaterfallButton { get => _waterfallFacilityShopIcon.GetComponentInChildren<Button>(); }
    public ReleaseSlider ReleaseSlider { get => _releaseSlider; }
    public UITraining TrainingPanel { get => _trainingPanel; }
    public UIExpedition ExpeditionPanel { get => _expeditionPanel; }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionPanel.SetExpeditionManager(expeditionManager);
    }

    public void Initialize(UIManager uiManager)
    {
        InitializeWallets();
        _trainingPanel.Initialize(uiManager);
        _expeditionPanel.Initialize();
    }

    private void InitializeWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.Initialize();
        }

        foreach (var wallet in _fossilWallets)
        {
            wallet.Initialize();
        }
    }

    public void InitializeVolumeSettings(AudioManager audioManager)
    {
        _audioManager = audioManager;

        _volumeSettings.Initialize();
    }

    public void LoadHabitatSpecificUI()
    {
        _marketplacePanel.Initialize();
        _detailsFolder.Initialize();
        _transferMap.Initialize();

        ResetStandardUI();
    }

    public void EnableTutorialUIByType(TutorialUIElementType uiElementType)
    {
        switch (uiElementType)
        {
            case TutorialUIElementType.None:
                break;
            case TutorialUIElementType.All:
                _marketplaceButton.gameObject.SetActive(true);
                _detailsButtons.gameObject.SetActive(true);
                _openDetailsButton.gameObject.SetActive(true);
                _worldMapButton.gameObject.SetActive(true);
                _marketplacePanel.ChimeraTabSetActive(true);
                _runeFacilityShopIcon.gameObject.SetActive(true);
                _caveFacilityShopIcon.gameObject.SetActive(true);
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
            case TutorialUIElementType.OtherFacilityButtons:
                _runeFacilityShopIcon.gameObject.SetActive(true);
                _caveFacilityShopIcon.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError($"{uiElementType} is invalid. Please change!");
                break;
        }
    }

    // Removes the basic UI so it can slowly be revealed by the Tutorial.
    public void TutorialDisableUI()
    {
        _runeFacilityShopIcon.gameObject.SetActive(false);
        _caveFacilityShopIcon.gameObject.SetActive(false);
        _marketplaceButton.gameObject.SetActive(false);
        _openDetailsButton.gameObject.SetActive(false);
        _worldMapButton.gameObject.SetActive(false);
        _closeDetailsButton.gameObject.SetActive(false);
        _marketplaceButton.gameObject.SetActive(false);
        _detailsButtons.gameObject.SetActive(false);
        _marketplacePanel.ChimeraTabSetActive(false);
    }

    // Resets to the standard UI when nothing has been disabled.
    public void ResetStandardUI()
    {
        _openDetailsButton.gameObject.SetActive(true);
        _standardUI.gameObject.SetActive(true);

        // Audio
        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _closeDetailsButton.gameObject.SetActive(false);
        _detailsPanel.gameObject.SetActive(false);
        _marketplacePanel.gameObject.SetActive(false);
        _settingsPanel.gameObject.SetActive(false);
        _expeditionPanel.gameObject.SetActive(false);
        _transferMap.gameObject.SetActive(false);
    }

    private void OpenDetails(DetailsButtonType detailsButtonType)
    {
        _detailsFolder.CheckDetails();

        ResetStandardUI();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _detailsPanel.gameObject.SetActive(true);
        _detailsFolder.ToggleDetailsButtons(detailsButtonType);

        _openDetailsButton.gameObject.SetActive(false);
    }

    public void OpenStandardDetailsPanel()
    {
        OpenDetails(DetailsButtonType.Standard);
        _closeDetailsButton.gameObject.SetActive(true);
    }

    private void OpenExpeditionDetailsPanel()
    {
        OpenDetails(DetailsButtonType.Expedition);
    }

    public void OpenMarketplace()
    {
        ResetStandardUI();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _marketplacePanel.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
    }

    public void CloseMarketplace()
    {
        _marketplacePanel.gameObject.SetActive(false);
    }

    public void OpenTransferMap(Chimera chimera)
    {
        ResetStandardUI();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _transferMap.Open(chimera);
    }

    public void ToggleSettingsMenu()
    {
        if(_trainingPanel.gameObject.activeInHierarchy == true)
        {
            _trainingPanel.ResetTrainingUI();
            ResetStandardUI();
        }
        else if (_settingsPanel.activeInHierarchy == true ||
            _marketplacePanel.gameObject.activeInHierarchy == true ||
            _expeditionPanel.gameObject.activeInHierarchy == true ||
            _detailsPanel.gameObject.activeInHierarchy == true
            )
        {
            ResetStandardUI();
        }
        else
        {
            OpenSettingsMenu();
        }
    }

    public void OpenSettingsMenu()
    {
        ResetStandardUI();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _settingsPanel.gameObject.SetActive(true);
        _openDetailsButton.gameObject.SetActive(false);
        _standardUI.gameObject.SetActive(false);
    }

    public void OpenExpedition()
    {
        ResetStandardUI();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _expeditionPanel.SetupExpeditionUI();

        OpenExpeditionDetailsPanel();
        _expeditionPanel.gameObject.SetActive(true);
    }

    public void UpdateDetails()
    {
        _detailsFolder.UpdateDetailsList();
    }

    public void OpenTrainingPanel()
    {
        _trainingPanel.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
        _expeditionButton.gameObject.SetActive(false);
        _marketplaceButton.gameObject.SetActive(false);
        _worldMapButton.gameObject.SetActive(false);
        _topLeftButtonsFolder.gameObject.SetActive(false);
    }

    public void RevealElementsHiddenByTraining()
    {
        _openDetailsButton.gameObject.SetActive(true);
        _expeditionButton.gameObject.SetActive(true);
        _marketplaceButton.gameObject.SetActive(true);
        _worldMapButton.gameObject.SetActive(true);
        _topLeftButtonsFolder.gameObject.SetActive(true);
    }

    public void UpdateEssenceWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.UpdateWallet();
        }
    }

    public void UpdateFossilWallets()
    {
        foreach (var wallet in _fossilWallets)
        {
            wallet.UpdateWallet();
        }
    }

    public void UpdateShopUI()
    {
        _marketplacePanel.UpdateShopUI();
    }
}