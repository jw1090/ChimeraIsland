using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HabitatUI : MonoBehaviour
{
    [Header("Scene Changing")]
    [SerializeField] private Button _worldMapButton = null;

    [Header("Elements")]
    [SerializeField] private Button _openDetailsButton = null;
    [SerializeField] private Button _closeDetailsButton = null;
    [SerializeField] private GameObject _topLeftButtonsFolder = null;
    [SerializeField] private GameObject _standardUI = null;
    [SerializeField] private GameObject _detailsButtons = null;
    [SerializeField] private ExpeditionUI _expeditionPanel = null;
    [SerializeField] private Settings _settingsPanel = null;
    [SerializeField] private GameObject _detailsPanel = null;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder = null;
    [SerializeField] private Marketplace _marketplacePanel = null;
    [SerializeField] private TransferMap _transferMap = null;
    [SerializeField] private ReleaseSlider _releaseSlider = null;
    [SerializeField] private TrainingUI _trainingPanel = null;
    [SerializeField] private FacilityUpgradeMarketplace _facilityMarketplace = null;
    [SerializeField] private List<UIEssenceWallet> _essenceWallets = new List<UIEssenceWallet>();
    [SerializeField] private List<UIFossilWallet> _fossilWallets = new List<UIFossilWallet>();

    private HabitatManager _habitatManager = null;
    private UIManager _uiManager = null;
    private AudioManager _audioManager = null;
    private bool _menuOpen = false;
    private TutorialManager _tutorialManager = null;

    public FacilityUpgradeMarketplace FacilityMarketplace { get => _facilityMarketplace; }
    public Marketplace Marketplace { get => _marketplacePanel; }
    public Settings Settings { get => _settingsPanel; }
    public Button WorldMapButton { get => _worldMapButton; }
    public ChimeraDetailsFolder DetailsPanel { get => _detailsFolder; }
    public ReleaseSlider ReleaseSlider { get => _releaseSlider; }
    public TrainingUI TrainingPanel { get => _trainingPanel; }
    public ExpeditionUI ExpeditionPanel { get => _expeditionPanel; }
    public bool MenuOpen { get => _menuOpen; }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _detailsFolder.SetExpeditionManager(expeditionManager);
        _expeditionPanel.SetExpeditionManager(expeditionManager);
    }

    public void SetAudioManager(AudioManager audioManager)
    {
        _expeditionPanel.SetAudioManager(audioManager);
    }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        InitializeWallets();
        _trainingPanel.Initialize(uiManager);
        _expeditionPanel.Initialize(uiManager);
        _detailsFolder.Initialize(uiManager);
        _settingsPanel.Initialize(uiManager);

        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
    }

    public void LoadCurrentUIProgress()
    {
        if (_expeditionPanel.expeditionManager.CurrentFossilProgress == 0)
        {
            foreach (var wallet in _fossilWallets)
            {
                wallet.gameObject.SetActive(false);
            }
        }

        if (_expeditionPanel.expeditionManager.CurrentHabitatProgress == 0)
        {
            foreach (var wallet in _essenceWallets)
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

        _settingsPanel.InitializeVolumeSettings();
    }

    public void LoadHabitatSpecificUI()
    {
        _uiManager.CreateButtonListener(_openDetailsButton, OpenStandardDetails);

        _marketplacePanel.Initialize(_uiManager); 
        _facilityMarketplace.Initialize(_uiManager);
        _detailsFolder.HabitatDetailsSetup();
        _transferMap.Initialize();

        UIProgressCheck();
    }

    private void UIProgressCheck()
    {
        ExpeditionManager expeditionManager = ServiceLocator.Get<ExpeditionManager>();

        if (expeditionManager.CurrentHabitatProgress > 0)
        {
            EnableUIElementByType(UIElementType.EssenceWallets);
        }

        if (expeditionManager.CurrentFossilProgress > 0)
        {
            EnableUIElementByType(UIElementType.FossilsWallets);
        }
    }

    public void EnableUIElementByType(UIElementType uiElementType)
    {
        switch (uiElementType)
        {
            case UIElementType.None:
                break;
            case UIElementType.All:
                _detailsButtons.gameObject.SetActive(true);
                _openDetailsButton.gameObject.SetActive(true);
                _worldMapButton.gameObject.SetActive(true);
                _marketplacePanel.ChimeraTabSetActive(true);
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
                break;
            case UIElementType.FossilsWallets:
                foreach (UIFossilWallet fossilWallet in _fossilWallets)
                {
                    fossilWallet.gameObject.SetActive(true);
                }
                break;
            case UIElementType.EssenceWallets:
                foreach (UIEssenceWallet essenceWallet in _essenceWallets)
                {
                    essenceWallet.gameObject.SetActive(true);
                }
                break;
            default:
                Debug.LogError($"{uiElementType} is invalid. Please change!");
                break;
        }
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
        _facilityMarketplace.CloseUI();

        _menuOpen = false;

        _audioManager.PlayUISFX(SFXUIType.StandardClick);
    }

    private void OpenDetails()
    {
        _detailsFolder.CheckDetails();
        _detailsFolder.ToggleDetailsButtons();

        _detailsPanel.gameObject.SetActive(true);
        _openDetailsButton.gameObject.SetActive(false);

        _audioManager.PlayUISFX(SFXUIType.StandardClick);
    }

    public void OpenStandardDetails()
    {
        ResetStandardUI();
        _closeDetailsButton.gameObject.SetActive(true);
        OpenDetails();
    }

    public void OpenFacilityUpgradeMenu(FacilityType facilityType)
    {
        if (_facilityMarketplace.CheckActive() == false || _facilityMarketplace.IsFacilityUnlocked(facilityType) == false)
        {
            return;
        }
        ResetStandardUI();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _openDetailsButton.gameObject.SetActive(false);
        _facilityMarketplace.ShowShop(facilityType);

        _menuOpen = true;
    }

    public void OpenExpedtionSelectionDetails()
    {
        ResetStandardUI();
        OpenDetails();
    }

    public void OpenMarketplace()
    {
        ResetStandardUI();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _marketplacePanel.gameObject.SetActive(true);
        _openDetailsButton.gameObject.SetActive(false);
        _marketplacePanel.ChimeraTabSetActive(true);

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
        if (isActiveAndEnabled == false)
        {
            return;
        }

        if (_trainingPanel.gameObject.activeInHierarchy == true)
        {
            _trainingPanel.ResetTrainingUI();
            ResetStandardUI();
        }
        else if (_settingsPanel.gameObject.activeInHierarchy == true ||
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

        _tutorialManager.ShowTutorialStage(TutorialStageType.ExpeditionSelection);

        _audioManager.PlayUISFX(SFXUIType.PortalClick);

        _expeditionPanel.ExpeditionButtonClick();

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
        _topLeftButtonsFolder.gameObject.SetActive(false);

        _menuOpen = true;
    }

    public void RevealElementsHiddenByTraining()
    {
        _openDetailsButton.gameObject.SetActive(true);
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
        _facilityMarketplace.UpdateUI();
        _marketplacePanel.UpdateShopUI();
    }
}