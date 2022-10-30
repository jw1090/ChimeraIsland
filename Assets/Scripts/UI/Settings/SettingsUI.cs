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

    public Button ScreenWideButton { get => _screenWideButton; }
    public Button MainMenuButton { get => _mainMenuButton; }
    public Button QuitGameButton { get => _quitGameButton; }
    public Button ResumeButton { get => _resumeButton; }

    public void SetCameraUtil(CameraUtil cameraUtil) { _inputSettingsUI.SetCameraUtil(cameraUtil); }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        _inputSettingsUI.Initialize();

        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        _uiManager.CreateButtonListener(_resumeButton, _uiManager.CloseSettings);
        _uiManager.CreateButtonListener(_screenWideButton, _uiManager.CloseSettings);
    }

    public void InitializeVolumeSettings()
    {
        _volumeSettingsUI.Initialize();
    }

    public void HideHabitatButtons(bool hide)
    {
        _mainMenuButton.gameObject.SetActive(!hide);
        _quitGameButton.gameObject.SetActive(!hide);

        if (hide == true)
        {
            _resumeButtonText.text = "Back";
        }
        else
        {
            _resumeButtonText.text = "Resume";
        }
    }
}