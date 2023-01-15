using System;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [Header("Light Attributes")]
    [SerializeField] [Range(0.0f, 1.0f)] private float _time = 0.0f;
    [SerializeField] private float _fullDayLength = 0.0f;
    [SerializeField] private float _startTime = 0.0f;

    [Header("Day Light")]
    [SerializeField] private Light _sourceLight = null;
    [SerializeField] private Light _ambientColorLight = null;
    [SerializeField] private Gradient _dayLightColor = null;
    [SerializeField] private Gradient _nightLightColor = null;
    [SerializeField] private AnimationCurve _lightingIntensity = null;

    [Header("Sky")]
    [SerializeField] private GameObject _sky = null;
    [SerializeField] private GameObject _stars = null;
    [SerializeField] private Color _daySky = new Color();
    [SerializeField] private Color _nightSky = new Color();
    [SerializeField] private Color _nightStarsSky = new Color();
    [SerializeField] private float _transitionDuration = 0.5f;
    [SerializeField] private float _transitionDurationDay = 0.5f;

    private Habitat _habitat = null;
    private Material _skyMaterial = null;
    private Material _starMaterial = null;
    private DayType _dayType = DayType.None;
    private Vector3 _mainLightRotation = Vector3.zero;
    private bool _initialized = false;
    private float _timeRate = 0.0f;
    private float _speed = 0.0f;

    public event Action<DayType> DayTypeChanged = null;

    public DayType DayType { get => _dayType; }

    public LightingManager Initialize()
    {
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;
        _skyMaterial = _sky.GetComponent<MeshRenderer>().material;
        _starMaterial = _stars.GetComponent<MeshRenderer>().material;

        DayTypeChanged = OnDayTypeChanged;
        _mainLightRotation = _ambientColorLight.transform.eulerAngles;

        _timeRate = 1.0f / _fullDayLength;
        _time = _startTime;

        _dayType = DayType.DayTime;
        _speed = 1.0f;

        _initialized = true;

        return this;
    }

    private void FixedUpdate()
    {
        if (_initialized == false)
        {
            return;
        }

        TimeEvaluate();
        LightRotation();
        SkyTransition();

        RenderSettings.ambientIntensity = _lightingIntensity.Evaluate(_time);
        RenderSettings.reflectionIntensity = _lightingIntensity.Evaluate(_time);

        _sky.transform.Rotate(Vector3.up * 1.5f * Time.deltaTime);
        _stars.transform.Rotate(Vector3.down * 0.2f * Time.deltaTime);
    }

    private void FirefliesToggle(bool shouldShow)
    {
        _habitat?.ToggleFireflies(shouldShow);
    }

    private void TimeEvaluate()
    {
        _time += _timeRate * _speed * Time.deltaTime;

        // Update time of day
        if (_time >= 1.0f)
        {
            _time = 0.0f;
        }

        _ambientColorLight.intensity = _lightingIntensity.Evaluate(_time);
        _ambientColorLight.color = ColorEvaluate();

        // Check for DayType change
        switch (_dayType)
        {
            case DayType.DayTime:
                if (_ambientColorLight.intensity > 0.0f)
                {
                    DayTypeChanged.Invoke(DayType.NightTime);
                }
                break;
            case DayType.NightTime:
                if (_ambientColorLight.intensity <= 0.0f)
                {
                    DayTypeChanged.Invoke(DayType.DayTime);
                }
                break;
            default:
                Debug.LogWarning($"DayType is not valid [{_dayType}] please change!");
                break;
        }
    }

    private Color ColorEvaluate()
    {
        Color dayColor = _dayLightColor.Evaluate(_time);
        Color nightColor = _nightLightColor.Evaluate(_time);

        Color result = Color.Lerp(dayColor, nightColor, _time);

        return result;
    }

    // Lights are opposite directions.
    private void LightRotation()
    {
        _mainLightRotation.x = (_time - 0.25f) * (360f);
        _sourceLight.transform.eulerAngles = _mainLightRotation;
    }

    private void SkyTransition()
    {
        if (_dayType == DayType.NightTime)
        {
            _skyMaterial.color = Color.Lerp(_skyMaterial.color, _nightSky, _transitionDuration);
            if (_ambientColorLight.intensity > 0.07f)
            {
                _starMaterial.color = Color.Lerp(_starMaterial.color, _daySky, _transitionDuration);
            }
            else
            {
                _starMaterial.color = Color.Lerp(_starMaterial.color, _nightStarsSky, _transitionDurationDay);
            }
        }
        else
        {
            _skyMaterial.color = Color.Lerp(_skyMaterial.color, _daySky, _transitionDuration);
            _starMaterial.color = Color.Lerp(_starMaterial.color, _nightStarsSky, _transitionDuration);
        }
    }

    private void OnDayTypeChanged(DayType newType)
    {
        switch (newType)
        {
            case DayType.NightTime:
                _dayType = DayType.NightTime;

                FirefliesToggle(true);

                _speed = 1.0f;
                break;
            case DayType.DayTime:
                _dayType = DayType.DayTime;

                FirefliesToggle(false);

                _speed = 1.0f;
                break;
            default:
                Debug.LogError($"Day type is not valid: {newType}");
                break;
        }
    }
}