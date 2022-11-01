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
    private UIManager _uiManager;
    private StatefulObject _statefulObject;
    private PersistentData _persistentData;

    public Button NewGameButton { get => _newGameButton; }
    public Button LoadGameButton { get => _loadGameButton; }
    public Button OpenCreditsButton { get => _openCreditsButton; }
    public Button CloseCreditsButton { get => _closeCreditsButton; }
    public Button QuitGameButton { get => _quitGameButton; }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _statefulObject = GetComponent<StatefulObject>();
        _persistentData = ServiceLocator.Get<PersistentData>();

        SetupButtons();
        CloseCredits();
        CheckHasSave();
    }

    public void CheckHasSave()
    {
        if (_persistentData.ChimeraData == null || _persistentData.ChimeraData.Count == 0)
        {
            LoadGameButton.gameObject.SetActive(false);
        }
        else
        {
            LoadGameButton.gameObject.SetActive(true);
        }
    }

    private void SetupButtons()
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