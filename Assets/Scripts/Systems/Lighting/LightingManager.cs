using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightingManager : MonoBehaviour
{
    [Header("Light Attributes")]
    [SerializeField][Range(0.0f, 1.0f)] private float _time = 0.0f;
    [SerializeField] private float _fullDayLength = 0.0f;
    [SerializeField] private float _dayStartTime = 0.15f;
    [SerializeField] private float _nightStartTime = 0.8f;
    private DayType _dayType = DayType.DayTime;
    private float _speed = 1.0f;
    private float _timeRate = 0.0f;
    private bool _initialized = false;

    [Header("Day Light")]
    [SerializeField] private Gradient _dayLightColor = null;
    [SerializeField] private Gradient _nightLightColor = null;
    [SerializeField] private AnimationCurve _lightingIntensity = null;
    private Vector3 _sourceLightRotation = Vector3.zero;

    [Header("Night Fade")]
    [SerializeField] private float _nightFadeDuration = 5.0f;
    [SerializeField] private float _skyNightAlpha = 0.05f;
    [SerializeField] private float _starsNightAlpha = 0.85f;

    [Header("Day Fade")]
    [SerializeField] private float _dayFadeDuration = 15.0f;
    [SerializeField] private float _skyDayAlpha = 0.55f;
    [SerializeField] private float _starsDayAlpha = 0.0f;

    [Header("References")]
    [SerializeField] private Light _sourceLight = null;
    [SerializeField] private Light _ambientColorLight = null;
    [SerializeField] private GameObject _sky = null;
    [SerializeField] private GameObject _stars = null;
    private Habitat _habitat = null;
    private Material _skyMaterial = null;
    private Material _starMaterial = null;

    public event Action<DayType> DayTypeChanged = null;

    public DayType DayType { get => _dayType; }

    public LightingManager Initialize()
    {
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;
        _skyMaterial = _sky.GetComponent<MeshRenderer>().material;
        _starMaterial = _stars.GetComponent<MeshRenderer>().material;

        DayTypeChanged = OnDayTypeChanged;

        _timeRate = 1.0f / _fullDayLength;
        _time = _dayStartTime + 0.1f;

        _initialized = true;

        DayTypeChanged.Invoke(DayType.DayTime);

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

        RenderSettings.ambientIntensity = _lightingIntensity.Evaluate(_time);
        RenderSettings.reflectionIntensity = _lightingIntensity.Evaluate(_time);

        _ambientColorLight.intensity = _lightingIntensity.Evaluate(_time);
        _ambientColorLight.color = ColorEvaluate();

        _sourceLight.color = _ambientColorLight.color;

        _sky.transform.Rotate(Vector3.up * 0.9f * Time.deltaTime);
        _stars.transform.Rotate(Vector3.up * 0.05f * Time.deltaTime);
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

        // Check for DayType change
        switch (_dayType)
        {
            case DayType.DayTime:
                if (_time > _nightStartTime)
                {
                    DayTypeChanged.Invoke(DayType.NightTime);
                }
                break;
            case DayType.NightTime:
                if (_time > _dayStartTime && _time < _nightStartTime)
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

        Color result;
        if (_time < 0.5f)
        {
            result = Color.Lerp(nightColor, dayColor, _time * 2.0f);
        }
        else
        {
            result = Color.Lerp(dayColor, nightColor, (_time - 0.5f) * 2.0f);
        }

        return result;
    }

    // Lights are opposite directions.
    private void LightRotation()
    {
        _sourceLightRotation.x = (_time - 0.25f) * (360f);
        _sourceLight.transform.eulerAngles = _sourceLightRotation;
    }

    private void OnDayTypeChanged(DayType daytype)
    {
        _dayType = daytype;

        if (daytype == DayType.NightTime)
        {
            FirefliesToggle(true);
            _speed = 2.0f;
        }
        else
        {
            FirefliesToggle(false);
            _speed = 1.0f;
        }

        StartCoroutine(StartSkyFade());
    }

    private IEnumerator StartSkyFade()
    {
        float timer = 0.0f;

        float duration;
        if (_dayType == DayType.NightTime)
        {
            duration = _nightFadeDuration;
        }
        else
        {
            duration = _dayFadeDuration;
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float dayFadeProgress = timer / duration;

            EvaluateSkyFade(dayFadeProgress);

            yield return null;
        }
    }

    private void EvaluateSkyFade(float progress)
    {
        Color startSkyColor = Color.white;
        Color startStarColor = Color.white;
        Color endSkyColor = Color.white;
        Color endStarColor = Color.white;

        if (_dayType == DayType.NightTime)
        {
            startSkyColor.a = _skyDayAlpha;
            startStarColor.a = _starsDayAlpha;
            endSkyColor.a = _skyNightAlpha;
            endStarColor.a = _starsNightAlpha;
        }
        else if (_dayType == DayType.DayTime)
        {
            startSkyColor.a = _skyNightAlpha;
            startStarColor.a = _starsNightAlpha;
            endSkyColor.a = _skyDayAlpha;
            endStarColor.a = _starsDayAlpha;
        }
        else
        {
            Debug.LogError($"Error in sky fade evaluation!");
        }

        _skyMaterial.color = Color.Lerp(startSkyColor, endSkyColor, progress);
        _starMaterial.color = Color.Lerp(startStarColor, endStarColor, progress);
    }
}