using UnityEngine;
using UnityEngine.UI;

public class InputSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider _cameraSensitivitySlider = null;
    [SerializeField] private Slider _chimeraSpinSpeedSlider = null;
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

        _chimeraSpinSpeedSlider.value = _inputManager.RotationSpeed;
        _cameraSensitivitySlider.value = 20.0f;

        SetupListeners();
    }

    private void SetupListeners()
    {
        _cameraSensitivitySlider.onValueChanged.AddListener(SetCameraSpeed);
        _chimeraSpinSpeedSlider.onValueChanged.AddListener(SetChimeraRotationSpeed);
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