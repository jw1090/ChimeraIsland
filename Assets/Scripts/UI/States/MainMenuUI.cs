using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private StatefulObject _statefulObject = null;

    [Header("Main Buttons")]
    [SerializeField] private Button _newGameButton = null;
    [SerializeField] private Button _loadGameButton = null;
    [SerializeField] private Button _openCreditsButton = null;
    [SerializeField] private Button _closeCreditsButton = null;
    [SerializeField] private Button _quitGameButton = null;
    [SerializeField] private Button _settingsButton = null;

    [Header("Warning Panel")]
    [SerializeField] private Button _warningYesButton = null;
    [SerializeField] private Button _warningNoButton = null;
    [SerializeField] private Button _screenWideWarningButton = null;

    private UIManager _uiManager;
    private PersistentData _persistentData;

    public Button WarningYesButton { get => _warningYesButton; }
    public Button WarningNoButton { get => _warningNoButton; }
    public Button NewGameButton { get => _newGameButton; }
    public Button LoadGameButton { get => _loadGameButton; }
    public Button OpenCreditsButton { get => _openCreditsButton; }
    public Button CloseCreditsButton { get => _closeCreditsButton; }
    public Button QuitGameButton { get => _quitGameButton; }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _persistentData = ServiceLocator.Get<PersistentData>();

        SetupButtonsListeners();
        ResetToStandard();
        CheckShowLoadGameButton();
    }

    public void CheckShowLoadGameButton()
    {
        LoadGameButton.gameObject.SetActive(CheckHasSave());
    }

    public bool CheckHasSave()
    {
        return _persistentData.ChimeraData != null && _persistentData.ChimeraData.Count != 0;
    }

    private void SetupButtonsListeners()
    {
        _uiManager.CreateButtonListener(_openCreditsButton, OpenCredits);
        _uiManager.CreateButtonListener(_closeCreditsButton, ResetToStandard);
        _uiManager.CreateButtonListener(_settingsButton, _uiManager.SettingsUI.OpenSettingsPanel);
        _uiManager.CreateButtonListener(_warningNoButton, ResetToStandard);
        _uiManager.CreateButtonListener(_screenWideWarningButton, ResetToStandard);
    }

    private void OpenCredits()
    {
        _statefulObject.SetState("Credits Panel", true);
    }

    public void OpenWarningPanel()
    {
        _statefulObject.SetState("Warning Panel", true);
    }

    public void ResetToStandard()
    {
        _statefulObject.SetState("Main Panel", true);
    }
}