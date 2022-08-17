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
    [SerializeField] private GameObject _bottomRightButtonsFolder = null;
    [SerializeField] private ExpeditionButton _expeditionButton = null;
    [SerializeField] private GameObject _topLeftButtonsFolder = null;
    [SerializeField] private GameObject _waterfallFacilityShopIcon = null;
    [SerializeField] private GameObject _runeFacilityShopIcon = null;
    [SerializeField] private GameObject _caveFacilityShopIcon = null;
    [SerializeField] private GameObject _standardUI = null;
    [SerializeField] private GameObject _detailsButtons = null;
    [SerializeField] private ExpeditionUI _expeditionPanel = null;
    [SerializeField] private GameObject _settingsPanel = null;
    [SerializeField] private GameObject _detailsPanel = null;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder = null;
    [SerializeField] private Marketplace _marketplacePanel = null;
    [SerializeField] private TransferMap _transferMap = null;
    [SerializeField] private UIVolumeSettings _volumeSettings = null;
    [SerializeField] private ReleaseSlider _releaseSlider = null;
    [SerializeField] private TrainingUI _trainingPanel = null;
    [SerializeField] private List<UIEssenceWallet> _essenceWallets = new List<UIEssenceWallet>();
    [SerializeField] private List<UIFossilWallet> _fossilWallets = new List<UIFossilWallet>();

    private UIManager _uiManager = null;
    private AudioManager _audioManager = null;
    private TutorialManager _tutorialManager = null;
    private bool _menuOpen = false;

    public Marketplace Marketplace { get => _marketplacePanel; }
    public Button MainMenuButton { get => _mainMenuButton; }
    public Button QuitGameButton { get => _quitGameButotn; }
    public Button WorldMapButton { get => _worldMapButton; }
    public Button MarketplaceButton { get => _marketplaceButton; }
    public ExpeditionButton ExpeditionButton { get => _expeditionButton; }
    public Button WaterfallButton { get => _waterfallFacilityShopIcon.GetComponentInChildren<Button>(); }
    public ChimeraDetailsFolder DetailsPanel { get => _detailsFolder; }
    public ReleaseSlider ReleaseSlider { get => _releaseSlider; }
    public TrainingUI TrainingPanel { get => _trainingPanel; }
    public ExpeditionUI ExpeditionPanel { get => _expeditionPanel; }
    public bool MenuOpen { get => _menuOpen; }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionPanel.SetExpeditionManager(expeditionManager);
    }
    public void SetAudioManager(AudioManager audioManager)
    {
        _expeditionPanel.SetAudioManager(audioManager);
    }
    public void Initialize(UIManager uiManager)
    {
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        _uiManager = uiManager;

        InitializeWallets();
        _trainingPanel.Initialize(uiManager);
        _expeditionPanel.Initialize(uiManager);
        _detailsFolder.Initialize(uiManager);

        _expeditionButton.ActivateNotification(false);

        SetupUIListeners();
    }

    public void LoadCurrentUIProgress()
    {
        if (_expeditionPanel.expeditionManager.CurrentFossilProgress == 0)
        {
            _marketplaceButton.gameObject.SetActive(false);
            foreach (var wallet in _fossilWallets)
            {
                wallet.gameObject.SetActive(false);
            }
        }
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
        _uiManager.CreateButtonListener(_openDetailsButton, OpenStandardDetails);

        _marketplacePanel.Initialize();
        _detailsFolder.HabitatDetailsSetup();
        _transferMap.Initialize();
    }

    private void SetupUIListeners()
    {
        _detailsFolder.SetupButtonListeners();
    }

    public void EnableTutorialUIByType(UIElementType uiElementType)
    {
        switch (uiElementType)
        {
            case UIElementType.None:
                break;
            case UIElementType.All:
                _bottomRightButtonsFolder.gameObject.SetActive(true);
                _detailsButtons.gameObject.SetActive(true);
                _openDetailsButton.gameObject.SetActive(true);
                _worldMapButton.gameObject.SetActive(true);
                _marketplacePanel.ChimeraTabSetActive(true);
                _runeFacilityShopIcon.gameObject.SetActive(true);
                _caveFacilityShopIcon.gameObject.SetActive(true);
                break;
            case UIElementType.MarketplaceButton:
                _marketplaceButton.gameObject.SetActive(true);
                break;
            case UIElementType.MarketplaceChimeraTab:
                _marketplacePanel.ChimeraTabSetActive(true);
                break;
            case UIElementType.OpenDetailsButton:
                _detailsButtons.gameObject.SetActive(true);
                _openDetailsButton.gameObject.SetActive(true);
                break;
            case UIElementType.WorldMapButton:
                _worldMapButton.gameObject.SetActive(true);
                break;
            case UIElementType.OtherFacilityButtons:
                _runeFacilityShopIcon.gameObject.SetActive(true);
                _caveFacilityShopIcon.gameObject.SetActive(true);
                break;
            case UIElementType.FossilButtons:
                foreach(UIFossilWallet uiFossil in _fossilWallets)
                {
                    uiFossil.gameObject.SetActive(true);
                }
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
        _waterfallFacilityShopIcon.gameObject.SetActive(false);
        _bottomRightButtonsFolder.gameObject.SetActive(false);
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

        _closeDetailsButton.gameObject.SetActive(false);
        _detailsPanel.gameObject.SetActive(false);
        _marketplacePanel.gameObject.SetActive(false);
        _settingsPanel.gameObject.SetActive(false);
        _expeditionPanel.CloseExpeditionUI();
        _transferMap.gameObject.SetActive(false);

        _menuOpen = false;

        _audioManager.PlayUISFX(SFXUIType.StandardClick);
    }

    private void OpenDetails(DetailsButtonType detailsButtonType)
    {
        _detailsFolder.CheckDetails();

        ResetStandardUI();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _detailsPanel.gameObject.SetActive(true);
        _detailsFolder.ToggleDetailsButtons(detailsButtonType);

        _openDetailsButton.gameObject.SetActive(false);

        if (_worldMapButton.gameObject.activeInHierarchy == true && detailsButtonType == DetailsButtonType.Standard)
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.Transfers);
        }
    }

    public void OpenStandardDetails()
    {
        OpenDetails(DetailsButtonType.Standard);
        _closeDetailsButton.gameObject.SetActive(true);
    }

    public void OpenExpedtionSelectionDetails()
    {
        OpenDetails(DetailsButtonType.Standard);
        _closeDetailsButton.gameObject.SetActive(false);
    }

    public void OpenMarketplace()
    {
        ResetStandardUI();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _marketplacePanel.gameObject.SetActive(true);
        if (_marketplacePanel.ChimeraTabIsActive() == true)
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.ChimeraShop);
        }
        _openDetailsButton.gameObject.SetActive(false);
        _marketplacePanel.ChimeraTabSetActive(true);
        _marketplacePanel.FacilityTabCheckActive();

        _menuOpen = true;
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

        _menuOpen = true;
    }

    public void ToggleSettingsMenu()
    {
        if (_trainingPanel.gameObject.activeInHierarchy == true)
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

        _menuOpen = true;
    }

    public void OpenExpedition()
    {
        ResetStandardUI();

        _expeditionButton.ActivateNotification(false);
        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _expeditionPanel.OpenExpeditionUI();

        _menuOpen = true;
    }

    public void UpdateHabitatUI()
    {
        _detailsFolder.UpdateDetailsList();
        _expeditionPanel.SetupUI.UpdateIcons();
    }

    public void OpenTrainingPanel()
    {
        _trainingPanel.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
        _expeditionButton.gameObject.SetActive(false);
        _bottomRightButtonsFolder.gameObject.SetActive(false);
        _topLeftButtonsFolder.gameObject.SetActive(false);

        _menuOpen = true;
    }

    public void RevealElementsHiddenByTraining()
    {
        _openDetailsButton.gameObject.SetActive(true);
        _expeditionButton.gameObject.SetActive(true);
        _bottomRightButtonsFolder.gameObject.SetActive(true);
        _topLeftButtonsFolder.gameObject.SetActive(true);

        _menuOpen = false;
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