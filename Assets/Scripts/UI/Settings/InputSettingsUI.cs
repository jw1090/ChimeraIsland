using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider _cameraSensitivitySlider = null;
    [SerializeField] private Slider _chimeraSpinSpeedSlider = null;
    [SerializeField] private TextMeshProUGUI _cameraSensitivityText = null;
    [SerializeField] private TextMeshProUGUI _chimeraSpinSpeedText = null;
    private PersistentData _persistentData = null;
    private CameraUtil _cameraUtil = null;
    private InputManager _inputManager = null;

    public void SetCameraUtil(CameraUtil cameraUtil)
    {
        _cameraUtil = cameraUtil;
        _cameraSensitivitySlider.value = cameraUtil.Speed;
    }

    public void Initialize()
    {
        _inputManager = ServiceLocator.Get<InputManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();

        _chimeraSpinSpeedSlider.value = _inputManager.RotationSpeed;
        _cameraSensitivitySlider.value = 20.0f;

        SetupListeners();
    }

    private void SetupListeners()
    {
        _cameraSensitivitySlider.onValueChanged.AddListener(SetCameraSpeed);
        _cameraSensitivityText.text = SliderToTextVal(_cameraSensitivitySlider);
        _chimeraSpinSpeedSlider.onValueChanged.AddListener(SetChimeraRotationSpeed);
        _chimeraSpinSpeedText.text = SliderToTextVal(_chimeraSpinSpeedSlider);
    }

    private void SetChimeraRotationSpeed(float speedValue)
    {
        _inputManager.SetChimeraRotationSpeed(speedValue);
        _chimeraSpinSpeedText.text = SliderToTextVal(_chimeraSpinSpeedSlider);
    }

    private void SetCameraSpeed(float speed)
    {
        if(_cameraUtil != null)
        {
            _cameraUtil.SetSpeed(speed);
        }
        else
        {
            _persistentData.SettingsData.cameraSpeed = speed;
        }
        _cameraSensitivityText.text = SliderToTextVal(_cameraSensitivitySlider);
    }

    public string SliderToTextVal(Slider slider)
    {
        int num = (int)((slider.value - slider.minValue) / (slider.maxValue - slider.minValue) * 100);
        return num.ToString();
    }
}