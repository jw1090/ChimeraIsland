using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Scene Types")]
    [SerializeField] private GameObject _habitatUI = null;
    [SerializeField] private GameObject _mainMenuUI = null;
    [SerializeField] private StartingUI _startingUI = null;
    [SerializeField] private GameObject _worldMapUI = null;

    [Header("Scene Change Buttons")]
    [SerializeField] private Button _newGameButton = null;
    [SerializeField] private Button _loadGameButton = null;
    [SerializeField] private Button _mainMenuButton = null;
    [SerializeField] private Button _worldMapButton = null;
    [SerializeField] private Button _stonePlainsButton = null;
    [SerializeField] private Button _treeOfLifeButton = null;
    [SerializeField] private Button _ashLandsButton = null;

    [Header("Standard Elements")]
    [SerializeField] private Button _closeDetailsButton = null;
    [SerializeField] private Button _expeditionButton = null;
    [SerializeField] private Button _openDetailsButton = null;
    [SerializeField] private Button _marketplaceButton = null;
    [SerializeField] private Button _settingsButton = null;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder = null;
    [SerializeField] private GameObject _buttonFolder = null;
    [SerializeField] private GameObject _expeditionPanel = null;
    [SerializeField] private GameObject _settingsPanel = null;
    [SerializeField] private Marketplace _marketplacePanel = null;
    [SerializeField] private TransferMap _transferMap = null;
    [SerializeField] private ReleaseSlider _releaseSlider = null;
    [SerializeField] private UITutorialOverlay _tutorialOverlay = null;
    [SerializeField] private List<UIWallet> _essenceWallets = new List<UIWallet>();

    public Button AshLandsButton { get => _ashLandsButton; }
    public Button NewGameButton { get => _newGameButton; }
    public Button MainMenuButton { get => _mainMenuButton; }
    public Button LoadGameButton { get => _loadGameButton; }
    public Button StonePlainsButton { get => _stonePlainsButton; }
    public Button TreeOfLifeButton { get => _treeOfLifeButton; }
    public Button WorldMapButton { get => _worldMapButton; }
    public ReleaseSlider ReleaseSlider { get => _releaseSlider; }
    public UITutorialOverlay TutorialOverlay { get => _tutorialOverlay; }

    public UIManager Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _startingUI.Initialize();

        DisableAllSceneTypeUI();

        return this;
    }

    public void InitializeHabitatUI()
    {
        InitializeWallets();
        InitializeTutorialOverlay();
        _marketplacePanel.Initialize();
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

    private void InitializeTutorialOverlay()
    {
        if (_tutorialOverlay != null)
        {
            _tutorialOverlay.Initialize();
        }
    }

    public void ShowUIByScene(SceneType uiSceneType)
    {
        Debug.Log($"<color=Cyan> Show {uiSceneType} UI.</color>");

        switch (uiSceneType)
        {
            case SceneType.None:
                DisableAllSceneTypeUI();
                break;
            case SceneType.Habitat:
                _habitatUI.gameObject.SetActive(true);
                break;
            case SceneType.MainMenu:
                _mainMenuUI.gameObject.SetActive(true);
                break;
            case SceneType.Starting:
                _startingUI.gameObject.SetActive(true);
                break;
            case SceneType.WorldMap:
                _worldMapUI.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError($"{uiSceneType} is invalid. Please change!");
                break;
        }
    }

    public void DisableAllSceneTypeUI()
    {
        _habitatUI.gameObject.SetActive(false);
        _mainMenuUI.gameObject.SetActive(false);
        _startingUI.gameObject.SetActive(false);
        _worldMapUI.gameObject.SetActive(false);
    }

    public void EnableUIByType(UIElementType uiElementType)
    {
        switch (uiElementType)
        {
            case UIElementType.All:
                _essenceWallets[0].gameObject.SetActive(true);
                _expeditionButton.gameObject.SetActive(true);
                _marketplaceButton.gameObject.SetActive(true);
                _openDetailsButton.gameObject.SetActive(true);
                _settingsButton.gameObject.SetActive(true);
                _worldMapButton.gameObject.SetActive(true);
                break;
            case UIElementType.EssenceWallet:
                _essenceWallets[0].gameObject.SetActive(true);
                break;
            case UIElementType.ExpeditionButton:
                _expeditionButton.gameObject.SetActive(true);
                break;
            case UIElementType.MarketplaceButton:
                _marketplaceButton.gameObject.SetActive(true);
                break;
            case UIElementType.OpenDetailsButton:
                _openDetailsButton.gameObject.SetActive(true);
                break;
            case UIElementType.SettingsButton:
                _settingsButton.gameObject.SetActive(true);
                break;
            case UIElementType.WorldMapIcon:
                _worldMapButton.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError($"{uiElementType} is invalid. Please change!");
                break;
        }
    }

    public void DisableHabitatUI()
    {
        _essenceWallets[0].gameObject.SetActive(false);
        _expeditionButton.gameObject.SetActive(false);
        _marketplaceButton.gameObject.SetActive(false);
        _openDetailsButton.gameObject.SetActive(false);
        _settingsButton.gameObject.SetActive(false);
        _worldMapButton.gameObject.SetActive(false);
    }

    public void ResetHabitatUI()
    {
        _openDetailsButton.gameObject.SetActive(true);
        _buttonFolder.gameObject.SetActive(true);
        _marketplaceButton.gameObject.SetActive(true);

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

        ResetHabitatUI();
        _closeDetailsButton.gameObject.SetActive(true);
        _detailsFolder.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
    }

    public void OpenMarketplace()
    {
        ResetHabitatUI();
        _marketplacePanel.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
    }

    public void OpenTransferMap(Chimera chimera)
    {
        ResetHabitatUI();
        _transferMap.Open(chimera);
    }

    public void ToggleSettingsMenu()
    {
        if (_settingsPanel.activeInHierarchy == true ||
            _marketplacePanel.gameObject.activeInHierarchy == true ||
            _expeditionPanel.gameObject.activeInHierarchy == true)
        {
            ResetHabitatUI();
        }
        else
        {
            OpenSettingsMenu();
        }
    }

    public void OpenSettingsMenu()
    {
        ResetHabitatUI();
        _settingsPanel.gameObject.SetActive(true);

        _openDetailsButton.gameObject.SetActive(false);
        _buttonFolder.gameObject.SetActive(false);
        _marketplaceButton.gameObject.SetActive(false);
    }

    public void OpenExpedition()
    {
        ResetHabitatUI();
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

    public void StartTutorial(TutorialSteps tutorialSteps)
    {
        _tutorialOverlay.gameObject.SetActive(true);
        _tutorialOverlay.ShowOverlay(tutorialSteps);
    }

    public void EndTutorial()
    {
        _tutorialOverlay.gameObject.SetActive(false);
        ServiceLocator.Get<TutorialManager>().SaveTutorialProgress();
    }
}