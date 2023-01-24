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
    [SerializeField] private DetailsManager _detailsManager = null;
    [SerializeField] private TrainingUI _trainingPanel = null;
    [SerializeField] private UITutorialOverlay _tutorialOverlay = null;
    [SerializeField] private UIEssenceWallet _essenceWallet = null;
    [SerializeField] private UIFossilWallet _fossilWallet = null;
    [SerializeField] private ChimeraPopUp _chimeraPopUp = null;

    private UIManager _uiManager = null;
    private AudioManager _audioManager = null;
    private TutorialManager _tutorialManager = null;
    private bool _menuOpen = false;
    private bool _tutorialOpen = false;
    private bool _uiActive = true;

    public UIManager UIManager { get => _uiManager; }
    public DetailsManager DetailsManager { get => _detailsManager; }
    public TrainingUI TrainingPanel { get => _trainingPanel; }
    public ExpeditionUI ExpeditionPanel { get => _expeditionPanel; }
    public Button CloseDetailsButton { get => _closeDetailsButton; }
    public bool MenuOpen { get => _menuOpen; }
    public bool TutorialOpen { get => _tutorialOpen; }

    public void ActivateChimeraPopUp(Chimera chimera)
    {
        _chimeraPopUp.SetChimera(chimera);
        _chimeraPopUp.gameObject.SetActive(true);
    }

    public void DeactivateChimeraPopUp()
    {
        _chimeraPopUp.gameObject.SetActive(false);
    }

    public void MenuClosed() { _menuOpen = false; }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _detailsManager.SetExpeditionManager(expeditionManager);
        _expeditionPanel.SetExpeditionManager(expeditionManager);
    }
    public void SetAudioManager(AudioManager audioManager)
    {
        _audioManager = audioManager;

        _expeditionPanel.SetAudioManager(audioManager);
    }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        SetupButtonListeners();

        _trainingPanel.Initialize(uiManager);
        _expeditionPanel.Initialize(uiManager);
        _detailsManager.Initialize(uiManager);
        _chimeraPopUp.Initialize();

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
        if (_expeditionPanel.expeditionManager.CurrentHabitatProgress == 0)
        {
            _essenceWallet.gameObject.SetActive(false);
        }

        if (_expeditionPanel.expeditionManager.CurrentFossilProgress == 0)
        {
            _fossilWallet.gameObject.SetActive(false);
        }
    }

    public void InitializeWallets()
    {
        _essenceWallet.Initialize();
        _fossilWallet.Initialize();
    }

    public void LoadHabitatSpecificUI()
    {
        _detailsManager.HabitatDetailsSetup();

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
                _fossilWallet.gameObject.SetActive(true);
                break;
            case UIElementType.EssenceWallets:
                _essenceWallet.gameObject.SetActive(true);
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
        _detailsManager.CloseDetails();
        _uiManager.SettingsUI.gameObject.SetActive(false);
        _expeditionPanel.CloseExpeditionUI();

        _menuOpen = false;
    }

    public void HideButtonsForExpeditions()
    {
        _openDetailsButton.gameObject.SetActive(false);
        _settingsButton.gameObject.SetActive(false);
        _fossilWallet.gameObject.SetActive(false);
        _essenceWallet.gameObject.SetActive(false);
    }

    public void RevealButtonsForExpeditions()
    {
        _openDetailsButton.gameObject.SetActive(true);
        _settingsButton.gameObject.SetActive(true);
        _fossilWallet.gameObject.SetActive(true);
        _essenceWallet.gameObject.SetActive(true);

        LoadCurrentUIProgress(); // Check for tutuorial steps
    }

    public void OpenStandardDetails()
    {
        ResetStandardUI();

        _menuOpen = true;

        _detailsManager.OpenStandardDetails();
        _closeDetailsButton.gameObject.SetActive(true);
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
            _detailsManager.IsOpen == true)
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

        _audioManager.PlaySFX(EnvironmentSFXType.PortalClick);

        _expeditionPanel.OpenExpeditionUI();

        _menuOpen = true;
    }

    public void UpdateHabitatUI()
    {
        _detailsManager.UpdateDetailsList();
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
        _essenceWallet.UpdateWallet();
    }

    public void UpdateFossilWallets()
    {
        _fossilWallet.UpdateWallet();
    }
}