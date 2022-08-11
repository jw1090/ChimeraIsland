using UnityEngine;
using UnityEngine.UI;

public class TrainingFacilityIcon : MonoBehaviour
{
    [SerializeField] private Image _icon = null;
    [SerializeField] private Slider _slider = null;
    private Camera _camera = null;
    private bool _initialized = false;

    public void SetIcon(Sprite sprite) { _icon.sprite = sprite; }

    public void SetSliderAttributes(int starting, int ending)
    {
        _slider.minValue = starting;
        _slider.value = starting;
        _slider.maxValue = ending;
    }

    public void Initialize()
    {
        _camera = ServiceLocator.Get<CameraUtil>().CameraCO;

        _initialized = true;
    }

    private void Update()
    {
        if(_initialized == false)
        {
            return;
        }

        transform.LookAt(_camera.transform);
    }

    public void UpdateSlider(int currentExperience)
    {
        _slider.value += currentExperience;
    }

    public void ResetIcon()
    {
        _icon.sprite = null;
        _slider.value = _slider.minValue;
    }
}