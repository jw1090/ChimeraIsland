using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HabitatUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Button _openDetailsButton = null;
    [SerializeField] private Button _closeDetailsButton = null;
    [SerializeField] private Button _settingsButton = null;
    [SerializeField] private GameObject _standardUI = null;
    [SerializeField] private GameObject _detailsButtons = null;
    [SerializeField] private ExpeditionUI _expeditionPanel = null;
    [SerializeField] private GameObject _detailsPanel = null;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder = null;
    [SerializeField] private ReleaseSlider _releaseSlider = null;
    [SerializeField] private TrainingUI _trainingPanel = null;
    [SerializeField] private UITutorialOverlay _tutorialOverlay = null;
    [SerializeField] private List<UIEssenceWallet> _essenceWallets = new List<UIEssenceWallet>();
    [SerializeField] private List<UIFossilWallet> _fossilWallets = new List<UIFossilWallet>();

    private UIManager _uiManager = null;
    private AudioManager _audioManager = null;
    private TutorialManager _tutorialManager = null;
    private bool _menuOpen = false;
    private bool _tutorialOpen = false;
    private bool _uiActive = true;

    public ChimeraDetailsFolder DetailsPanel { get => _detailsFolder; }
    public ReleaseSlider ReleaseSlider { get => _releaseSlider; }
    public TrainingUI TrainingPanel { get => _trainingPanel; }
    public ExpeditionUI ExpeditionPanel { get => _expeditionPanel; }
    public Button CloseDetailsButton { get => _closeDetailsButton; }
    public bool MenuOpen { get => _menuOpen; }
    public bool TutorialOpen { get => _tutorialOpen; }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _detailsFolder.SetExpeditionManager(expeditionManager);
        _expeditionPanel.SetExpeditionManager(expeditionManager);
    }

    public void SetAudioManager(AudioManager audioManager)
    {
        _audioManager = audioManager;

        _expeditionPanel.SetAudioManager(audioManager);
    }

    public void MenuClosed() { _menuOpen = false; }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        SetupButtonListeners();

        InitializeWallets();
        _trainingPanel.Initialize(uiManager);
        _expeditionPanel.Initialize(uiManager);
        _detailsFolder.Initialize(uiManager);
        _tutorialOverlay.Initialize(this);
    }

    private void SetupButtonListeners()
    {
        _uiManager.CreateButtonListener(_settingsButton, OpenHabitatSettings);
        _uiManager.CreateButtonListener(_openDetailsButton, OpenStandardDetails);
        _uiManager.CreateButtonListener(_closeDetailsButton, ResetStandardUI);
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

    public void LoadHabitatSpecificUI()
    {
        _detailsFolder.HabitatDetailsSetup();

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

    public void StartTutorial(TutorialStageData tutorialSteps, TutorialStageType tutorialType)
    {
        _tutorialOverlay.gameObject.SetActive(true);
        _tutorialOverlay.ShowOverlay(tutorialSteps, tutorialType);

        _standardUI.gameObject.SetActive(false);

        _tutorialOpen = true;
    }

    public void EndTutorial()
    {
        _tutorialOverlay.gameObject.SetActive(false);
        _tutorialManager.SaveTutorialProgress();

        _standardUI.gameObject.SetActive(true);

        _tutorialOpen = false;
    }

    public void EnableUIElementByType(UIElementType uiElementType)
    {
        switch (uiElementType)
        {
            case UIElementType.None:
            case UIElementType.MarketplaceChimeraTab:
            case UIElementType.WorldMapButton:
            case UIElementType.OtherFacilityButtons:
                break;
            case UIElementType.All:
                _detailsButtons.gameObject.SetActive(true);
                _openDetailsButton.gameObject.SetActive(true);
                break;
            case UIElementType.OpenDetailsButton:
                _detailsButtons.gameObject.SetActive(true);
                _openDetailsButton.gameObject.SetActive(true);
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
        if (_uiActive == false)
        {
            _openDetailsButton.gameObject.SetActive(false);
            _standardUI.gameObject.SetActive(false);
        }
        else
        {
            _openDetailsButton.gameObject.SetActive(true);

            if (_tutorialOpen == false)
            {
                _standardUI.gameObject.SetActive(true);
            }
        }

        _closeDetailsButton.gameObject.SetActive(false);
        _detailsPanel.gameObject.SetActive(false);
        _uiManager.SettingsUI.gameObject.SetActive(false);
        _expeditionPanel.CloseExpeditionUI();

        _menuOpen = false;
    }

    private void OpenDetails()
    {
        _detailsFolder.CheckDetails();

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

    public void OpenExpedtionSelectionDetails()
    {
        ResetStandardUI();
        OpenDetails();
    }

    public void OpenMarketplace()
    {
        if (_uiActive == false)
        {
            return;
        }

        ResetStandardUI();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _openDetailsButton.gameObject.SetActive(false);

        _menuOpen = true;
    }

    public void ToggleSettingsMenu()
    {
        if (isActiveAndEnabled == false)
        {
            return;
        }

        if (_tutorialOpen == true)
        {
            return;
        }

        if (_trainingPanel.gameObject.activeInHierarchy == true)
        {
            _trainingPanel.ResetTrainingUI();
            ResetStandardUI();

            _audioManager.PlayUISFX(SFXUIType.StandardClick);
        }
        else if (_uiManager.SettingsUI.gameObject.activeInHierarchy == true ||
            _expeditionPanel.gameObject.activeInHierarchy == true ||
            _detailsPanel.gameObject.activeInHierarchy == true)
        {
            ResetStandardUI();

            _audioManager.PlayUISFX(SFXUIType.StandardClick);
        }
        else
        {
            OpenHabitatSettings();
        }
    }

    private void OpenHabitatSettings()
    {
        ResetStandardUI();

        _openDetailsButton.gameObject.SetActive(false);
        _uiManager.SettingsUI.gameObject.SetActive(false);

        _uiManager.SettingsUI.OpenSettingsPanel();

        _menuOpen = true;
    }

    public void OpenExpedition()
    {
        if (_uiActive == false)
        {
            return;
        }

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
        _settingsButton.gameObject.SetActive(false);

        _menuOpen = true;
    }

    public void RevealElementsHiddenByTraining()
    {
        _openDetailsButton.gameObject.SetActive(true);
        _settingsButton.gameObject.SetActive(true);

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
}