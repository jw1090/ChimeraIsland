using UnityEngine;
using UnityEngine.UI;

public class InputSettingsUI : MonoBehaviour
{
    [SerializeField] private TextSliderUI _cameraSensitivitySlider = null;
    [SerializeField] private TextSliderUI _chimeraSpinSpeedSlider = null;
    private PersistentData _persistentData = null;
    private CameraUtil _cameraUtil = null;
    private InputManager _inputManager = null;

    public void SetCameraUtil(CameraUtil cameraUtil)
    {
        _cameraUtil = cameraUtil;
        SetCameraSpeed(_cameraSensitivitySlider.Slider.value);
    }

    public void Initialize()
    {
        _inputManager = ServiceLocator.Get<InputManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();

        _chimeraSpinSpeedSlider.Slider.value = _inputManager.RotationSpeed;
        if (_cameraUtil == null)
        {
            _cameraSensitivitySlider.Slider.value = _persistentData.SettingsData.cameraSpeed;
        }

        SetupListeners();
    }

    private void SetupListeners()
    {
        _cameraSensitivitySlider.Slider.onValueChanged.AddListener(SetCameraSpeed);
        _cameraSensitivitySlider.Text.text = SliderToTextVal(_cameraSensitivitySlider.Slider);
        _chimeraSpinSpeedSlider.Slider.onValueChanged.AddListener(SetChimeraRotationSpeed);
        _chimeraSpinSpeedSlider.Text.text = SliderToTextVal(_chimeraSpinSpeedSlider.Slider);
    }

    private void SetChimeraRotationSpeed(float speedValue)
    {
        _inputManager.SetChimeraRotationSpeed(speedValue);
        _chimeraSpinSpeedSlider.Text.text = SliderToTextVal(_chimeraSpinSpeedSlider.Slider);
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

        _cameraSensitivitySlider.Text.text = SliderToTextVal(_cameraSensitivitySlider.Slider);
    }

    public string SliderToTextVal(Slider slider)
    {
        int num = (int)((slider.value - slider.minValue) / (slider.maxValue - slider.minValue) * 100);
        return num.ToString();
    }
}