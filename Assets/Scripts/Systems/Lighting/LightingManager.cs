using System;
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

    Vector3 _dayRotation = Vector3.zero;
    Vector3 _nightRotation = Vector3.zero;
    private Habitat _habitat = null;
    private bool _initialized = false;
    private float _timeRate = 0.0f;
    private float _speed = 0.0f;

    public static event Action<DayType> DayTypeChanged = null;
    public DayType DayType { get => _dayType; }

    public LightingManager Initialize()
    {
        DayTypeChanged += OnDayTypeChanged;
        _dayRotation = _dayLight.transform.eulerAngles;
        _nightRotation = _nightLight.transform.eulerAngles;

        _timeRate = 1.0f / _fullDayLength;
        _time = _startTime;

        _dayType = DayType.DayTime;
        _speed = 1.0f;

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

        RenderSettings.ambientIntensity = _lightingIntensityMultiplier.Evaluate(_time);
        RenderSettings.reflectionIntensity = _reflectionIntensityMultiplier.Evaluate(_time);
    }

    private void FirefliesToggle(bool shouldShow)
    {
        _habitat.ToggleFireflies(shouldShow);
    }

    private void TimeEvaluate()
    {
        _time += _timeRate * _speed * Time.deltaTime;

        // Update time of day
        if (_time >= 1.0f)
        {
            _time = 0.0f;
        }

        // Update the light intensity
        _dayLight.intensity = _dayLightIntensity.Evaluate(_time);
        _nightLight.intensity = _nightLightIntensity.Evaluate(_time);

        // Update the light color
        _dayLight.color = _dayLightColor.Evaluate(_time);
        _nightLight.color = _nightLightColor.Evaluate(_time);

        // Check for DayType change
        switch (_dayType)
        {
            case DayType.DayTime:
                if(_nightLight.intensity > 0f)
                {
                    DayTypeChanged.Invoke(DayType.NightTime);
                }
                break;
            case DayType.NightTime:
                if(_nightLight.intensity <= 0)
                {
                    DayTypeChanged.Invoke(DayType.DayTime);
                }
                break;
            default:
                Debug.LogWarning($"DayType is not valid [{_dayType}] please change!");
                break;
        }
    }

    // Lights are opposite directions.
    private void LightRotation()
    {
        _dayRotation.x = (_time - 0.25f) * (360f);
        _nightRotation.x = (_time - 0.75f) * (360f);

        _dayLight.transform.eulerAngles = _dayRotation;
        _nightLight.transform.eulerAngles = _nightRotation;
        if (!_nightLight.gameObject.activeInHierarchy)
        {
            _dayType = DayType.DayTime;
        }
    }

    private void OnDayTypeChanged(DayType newType)
    {
        switch (newType)
        {
            case DayType.NightTime:
                FirefliesToggle(true);
                _nightLight.gameObject.SetActive(true);
                _dayType = DayType.NightTime;
                _speed = 2.0f;
                break;
            case DayType.DayTime:
                FirefliesToggle(false);
                _nightLight.gameObject.SetActive(false);
                _dayType = DayType.DayTime;
                _speed = 1.0f;
                break;
            default:
                break;
        }
    }
}