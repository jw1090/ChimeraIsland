using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private UIVolumeSettings _volumeSettings = null;
    [SerializeField] private Button _resumeButton = null;
    [SerializeField] private Button _mainMenuButton = null;
    [SerializeField] private Button _quitGameButton = null;
    [SerializeField] private Button _screenWideButton = null;

    public Button MainMenuButton { get => _mainMenuButton; }
    public Button QuitGameButton { get => _quitGameButton; }
    public Button ResumeButton { get => _resumeButton; }
    public Button ScreenWideButton { get => _screenWideButton; }

    private UIManager _uiManager = null;

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        _uiManager.CreateButtonListener(_resumeButton, _uiManager.HabitatUI.ResetStandardUI);
        _uiManager.CreateButtonListener(_screenWideButton, _uiManager.HabitatUI.ResetStandardUI);
    }

    public void InitializeVolumeSettings()
    {
        _volumeSettings.Initialize();
    }
}