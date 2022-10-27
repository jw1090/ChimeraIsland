using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] private UIVolumeSettings _volumeSettings = null;
    [SerializeField] private Button _resumeButton = null;
    [SerializeField] private Button _mainMenuButton = null;
    [SerializeField] private Button _quitGameButton = null;
    [SerializeField] private Button _screenWideButton = null;
    [SerializeField] private Slider _cameraSensitivitySlider = null;
    [SerializeField] private Slider _chimeraSpinSpeedSlider = null;
    [SerializeField] private TextMeshProUGUI _resumeButtonText;
    private InputManager _inputManager = null;
    private CameraUtil _cameraUtil = null;
    private RectTransform _resumeRectTransform = null;
    private Image _backgroundImage = null;
    public Button MainMenuButton { get => _mainMenuButton; }
    public Button QuitGameButton { get => _quitGameButton; }
    public Button ResumeButton { get => _resumeButton; }
    public Button ScreenWideButton { get => _screenWideButton; }

    private UIManager _uiManager = null;

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        _inputManager = ServiceLocator.Get<InputManager>();

        _resumeRectTransform = _resumeButton.gameObject.GetComponent<RectTransform>();

        _backgroundImage = gameObject.GetComponent<Image>();

        _chimeraSpinSpeedSlider.value = _inputManager.RotationSpeed;

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
        _uiManager.CreateButtonListener(_resumeButton, _uiManager.CloseSettings);
        _uiManager.CreateButtonListener(_screenWideButton, _uiManager.CloseSettings);
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

    public void HideHabitatButtons(bool hide)
    {
        _mainMenuButton.gameObject.SetActive(!hide);
        _quitGameButton.gameObject.SetActive(!hide);
        _backgroundImage.enabled = hide;
        if (hide == true)
        {
            _resumeButtonText.text = "Back";
            _resumeRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _resumeRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _resumeRectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        else
        {
            _resumeButtonText.text = "Resume";
            _resumeRectTransform.anchorMin = new Vector2(0.5f, 1.0f);
            _resumeRectTransform.anchorMax = new Vector2(0.5f, 1.0f);
            _resumeRectTransform.pivot = new Vector2(0.5f, 1.0f);
        }
    }
}