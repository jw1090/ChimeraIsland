using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [Header("Light Attributes")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _time = 0.0f;
    [SerializeField] private float _fullDayLength = 0.0f;
    [SerializeField] private float _startTime = 0.0f;
    [SerializeField] private DayType _dayType = DayType.None;
    [SerializeField] private AnimationCurve _lightingIntensityMultiplier = null;
    [SerializeField] private AnimationCurve _reflectionIntensityMultiplier = null;

    [Header("Day Light")]
    [SerializeField] private Light _dayLight = null;
    [SerializeField] private Gradient _dayLightColor = null;
    [SerializeField] private AnimationCurve _dayLightIntensity = null;

    [Header("Night Light")]
    [SerializeField] private Light _nightLight = null;
    [SerializeField] private Gradient _nightLightColor = null;
    [SerializeField] private AnimationCurve _nightLightIntensity = null;

    Vector3 _dayPosition = Vector3.zero;
    Vector3 _nightPosition = Vector3.zero;
    private bool _initialized = false;
    private float _timeRate = 0.0f;
    private float _noon = 90.0f;
    private float _speed = 0.0f;

    public DayType DayType { get => _dayType; }

    public LightingManager Initialize()
    {
        _dayPosition = _dayLight.transform.eulerAngles;
        _nightPosition = _nightLight.transform.eulerAngles;

        _timeRate = 1.0f / _fullDayLength;
        _time = _startTime;

        _dayType = DayType.DayTime;

        _initialized = true;

        return this;
    }

    private void Update()
    {
        if (_initialized == false)
        {
            return;
        }

        TimeEvaluate();
        LightRotation();

        IntensityManipulation();
        ColorManipulation();

        DaylightToggle();
        NightLightToggle();
        Debug.Log(_dayLight.intensity);

        RenderSettings.ambientIntensity = _lightingIntensityMultiplier.Evaluate(_time);
        RenderSettings.reflectionIntensity = _reflectionIntensityMultiplier.Evaluate(_time);
    }

    private void TimeEvaluate()
    {
        _time += _timeRate * _speed * Time.deltaTime;

        if (_time >= 1.0f)
        {
            _time = 0.0f;
        }

        switch (_dayType)
        {
            case DayType.DayTime:
                _speed = 1;
                break;
            case DayType.NightTime:
                _speed = 2;
                break;
            default:
                Debug.LogWarning($"DayType is not valid [{_dayType}] please change!");
                break;
        }
    }

    // Lights are opposite directions.
    private void LightRotation()
    {
        _dayPosition.x = (_time - 0.25f) * _noon * 4.0f;
        _nightPosition.x = (_time - 0.75f) * _noon * 4.0f;

        _dayLight.transform.eulerAngles = _dayPosition;
        _nightLight.transform.eulerAngles = _nightPosition;
    }

    private void IntensityManipulation()
    {
        _dayLight.intensity = _dayLightIntensity.Evaluate(_time);
        _nightLight.intensity = _nightLightIntensity.Evaluate(_time);
    }

    private void ColorManipulation()
    {
        _dayLight.color = _dayLightColor.Evaluate(_time);
        _nightLight.color = _nightLightColor.Evaluate(_time);
    }

    private void DaylightToggle()
    {
        if (_dayLight.intensity == 0.1 && _dayLight.gameObject.activeInHierarchy)
        {
            _dayLight.gameObject.SetActive(false);
        }
        else if (_dayLight.intensity > 0.1 && !_dayLight.gameObject.activeInHierarchy)
        {
            _dayLight.gameObject.SetActive(true);
            _dayType = DayType.DayTime;
        }
    }

    private void NightLightToggle()
    {
        if (_nightLight.intensity == 0 && _nightLight.gameObject.activeInHierarchy)
        {
            _nightLight.gameObject.SetActive(false);
        }
        else if (_nightLight.intensity > 0 && !_nightLight.gameObject.activeInHierarchy)
        {
            _nightLight.gameObject.SetActive(true);
            _dayType = DayType.NightTime;
        }
    }
}