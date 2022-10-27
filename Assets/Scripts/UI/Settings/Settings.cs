using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private UIVolumeSettings _volumeSettings = null;
    [SerializeField] private Button _resumeButton = null;
    [SerializeField] private Button _mainMenuButton = null;
    [SerializeField] private Button _quitGameButton = null;
    [SerializeField] private Button _screenWideButton = null;
    [SerializeField] Slider _cameraSensitivitySlider = null;
    [SerializeField] Slider _chimeraSpinSpeedSlider = null;
    private InputManager _inputManager = null;
    private CameraUtil _cameraUtil = null;
    public Button MainMenuButton { get => _mainMenuButton; }
    public Button QuitGameButton { get => _quitGameButton; }
    public Button ResumeButton { get => _resumeButton; }
    public Button ScreenWideButton { get => _screenWideButton; }

    private UIManager _uiManager = null;

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        _inputManager = ServiceLocator.Get<InputManager>();

        _chimeraSpinSpeedSlider.value = 0.8f;
        _cameraSensitivitySlider.value = 20.0f;

        SetupButtonListeners();
    }
    
    public void SetCameraUtil(CameraUtil camera)
    {
        _cameraUtil = camera;
        _cameraSensitivitySlider.value = camera.Speed;
    }

    private void SetupButtonListeners()
    {
        _cameraSensitivitySlider.onValueChanged.AddListener(SetCameraSpeed);
        _chimeraSpinSpeedSlider.onValueChanged.AddListener(SetChimeraRotationSpeed);
        _uiManager.CreateButtonListener(_resumeButton, _uiManager.HabitatUI.ResetStandardUI);
        _uiManager.CreateButtonListener(_screenWideButton, _uiManager.HabitatUI.ResetStandardUI);
    }

    public void InitializeVolumeSettings()
    {
        _volumeSettings.Initialize();
    }

    private void SetChimeraRotationSpeed(float speedValue)
    {
        _inputManager.SetChimeraRotationSpeed(speedValue);
    }

    private void SetCameraSpeed(float speed)
    {
        _cameraUtil.SetSpeed(speed);
    }
}