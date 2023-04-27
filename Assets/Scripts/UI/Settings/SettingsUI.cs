using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private VolumeSettingsUI _volumeSettingsUI = null;
    [SerializeField] private InputSettingsUI _inputSettingsUI = null;
    [SerializeField] private Button _resumeButton = null;
    [SerializeField] private Button _screenWideButton = null;

    [Header("Bottom")]
    [SerializeField] private GameObject _bottomSection = null;
    [SerializeField] private Button _mainMenuButton = null;
    [SerializeField] private Button _quitGameButton = null;

    private UIManager _uiManager = null;
    private AudioManager _audioManager = null;

    public Button ScreenWideButton { get => _screenWideButton; }
    public Button MainMenuButton { get => _mainMenuButton; }
    public Button QuitGameButton { get => _quitGameButton; }
    public Button ResumeButton { get => _resumeButton; }

    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }
    public void SetCameraUtil(CameraUtil cameraUtil) { _inputSettingsUI.SetCameraUtil(cameraUtil); }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        gameObject.SetActive(false);

        _inputSettingsUI.Initialize();

        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        _uiManager.CreateButtonListener(_resumeButton, CloseSettingsButtonPressed);
        _uiManager.CreateButtonListener(_screenWideButton, CloseSettingsButtonPressed);
    }

    public void InitializeVolumeSettings()
    {
        _volumeSettingsUI.Initialize();
    }

    public void OpenSettingsUI()
    {
        _audioManager.PlayUISFX(SFXUIType.StandardClick);
        _bottomSection.SetActive(!_uiManager.InMainMenuState);

        gameObject.SetActive(true);
    }

    private void CloseSettingsButtonPressed()
    {
        _audioManager.PlayUISFX(SFXUIType.StandardClick);
        CloseSettingsUI();
    }

    public void CloseSettingsUI()
    {
        if (_uiManager.InHabitatState == true)
        {
            _uiManager.HabitatUI.ResetStandardUI();
        }
        else if (_uiManager.InTempleState == true)
        {
            _uiManager.TempleUI.CloseSettingsUI();
        }
        else if (_uiManager.InMainMenuState == true)
        {
            _uiManager.MainMenuUI.CloseSettingsUI();
        }

        gameObject.SetActive(false);
    }
}