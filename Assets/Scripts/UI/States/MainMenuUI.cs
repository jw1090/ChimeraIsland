using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _newGameButton = null;
    [SerializeField] private Button _loadGameButton = null;
    [SerializeField] private Button _openCreditsButton = null;
    [SerializeField] private Button _closeCreditsButton = null;
    [SerializeField] private Button _quitGameButton = null;
    [SerializeField] private Button _settingsButton = null;
    [SerializeField] private GameObject _warningPanel = null;
    [SerializeField] private Button _warningYesButton = null;
    [SerializeField] private Button _warningNoButton = null;

    private UIManager _uiManager;
    private StatefulObject _statefulObject;
    private PersistentData _persistentData;

    public GameObject WarningPanel { get => _warningPanel; }
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
        _statefulObject = GetComponent<StatefulObject>();

        SetupButtonsListeners();
        CloseCredits();
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
        _uiManager.CreateButtonListener(_closeCreditsButton, CloseCredits);
        _uiManager.CreateButtonListener(_settingsButton, _uiManager.SettingsUI.OpenSettingsPanel);
    }

    private void OpenCredits()
    {
        _statefulObject.SetState("Credits Panel", true);
    }

    private void CloseCredits()
    {
        _statefulObject.SetState("Main Panel", true);
    }
}