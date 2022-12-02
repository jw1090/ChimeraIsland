using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private VolumeSettingsUI _volumeSettingsUI = null;
    [SerializeField] private InputSettingsUI _inputSettingsUI = null;
    [SerializeField] private Button _resumeButton = null;
    [SerializeField] private Button _mainMenuButton = null;
    [SerializeField] private Button _quitGameButton = null;
    [SerializeField] private Button _screenWideButton = null;
    [SerializeField] private TextMeshProUGUI _resumeButtonText;
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

        _inputSettingsUI.Initialize();

        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        _uiManager.CreateButtonListener(_resumeButton, CloseSettingsPanel);
        _uiManager.CreateButtonListener(_screenWideButton, CloseSettingsPanel);
    }

    public void InitializeVolumeSettings()
    {
        _volumeSettingsUI.Initialize();
    }

    public void OpenSettingsPanel()
    {
        _mainMenuButton.gameObject.SetActive(_uiManager.InHabitatState);
        _quitGameButton.gameObject.SetActive(_uiManager.InHabitatState);

        if (_uiManager.InHabitatState == true)
        {
            _resumeButtonText.text = "Back";
        }
        else
        {
            _resumeButtonText.text = "Resume";
        }

        gameObject.SetActive(true);
        _audioManager.PlayUISFX(SFXUIType.StandardClick);
    }

    private void CloseSettingsPanel()
    {
        if(_uiManager.InHabitatState == true)
        {
            _uiManager.HabitatUI.MenuClosed();
            _uiManager.HabitatUI.ResetStandardUI();
        }

        gameObject.SetActive(false);
        _audioManager.PlayUISFX(SFXUIType.StandardClick);
    }
}