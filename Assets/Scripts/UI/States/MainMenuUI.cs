using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _newGameButton = null;
    [SerializeField] private Button _loadGameButton = null;
    [SerializeField] private Button _openCreditsButton = null;
    [SerializeField] private Button _closeCreditsButton = null;
    [SerializeField] private GameObject _menuPanel = null;
    [SerializeField] private GameObject _creditsPanel = null;
    private UIManager _uiManager;
    private StatefulObject _statefulObject;

    public Button NewGameButton { get => _newGameButton; }
    public Button LoadGameButton { get => _loadGameButton; }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _statefulObject = GetComponent<StatefulObject>();

        SetupButtons();
        CloseCredits();
    }

    private void SetupButtons()
    {
        _uiManager.CreateButtonListener(_openCreditsButton, OpenCredits);
        _uiManager.CreateButtonListener(_closeCreditsButton, CloseCredits);
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