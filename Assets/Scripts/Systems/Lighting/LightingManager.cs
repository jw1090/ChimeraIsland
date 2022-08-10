using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [Header("Light Attributes")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _time;
    [SerializeField] private float _fullDayLength;
    [SerializeField] private float _startTime;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _noon;
    [SerializeField] private DayType _dayType = DayType.None;
    [SerializeField] private AnimationCurve _lightingIntensityMultiplier = null;
    //[SerializeField] private AnimationCurve _reflectionIntensityMultiplier = null;

    [Header("Day Light")]
    [SerializeField] private Light _dayLight = null;
    [SerializeField] private Gradient _dayLightColor = null;
    [SerializeField] private AnimationCurve _dayLightIntensity = null;

    [Header("Night Light")]
    [SerializeField] private Light _nightLight = null;
    [SerializeField] private Gradient _nightLightColor = null;
    [SerializeField] private AnimationCurve _nightLightIntensity = null;

    public DayType DayType { get => _dayType; }
    private float _timeRate;

    private void Start()
    {
        _timeRate = 1.0f / _fullDayLength;
        _time = _startTime;
    }

    private void Update()
    {
        _time += _timeRate * _speed * Time.deltaTime;

        if(_time >= 1.0f)
        {
            _time = 0.0f;
        }

        //Light Rotation -> Lights are opposite directions
        _dayLight.transform.eulerAngles = (_time - 0.25f) * _noon * 4.0f;
        _nightLight.transform.eulerAngles = (_time - 0.75f) * _noon * 4.0f;

        //Light Intensity
        _dayLight.intensity = _dayLightIntensity.Evaluate(_time);
        _nightLight.intensity = _nightLightIntensity.Evaluate(_time);

        //Change the colors
        _dayLight.color = _dayLightColor.Evaluate(_time);
        _nightLight.color = _nightLightColor.Evaluate(_time);

        //Toggle the Day Light
        if(_dayLight.intensity == 0 && _dayLight.gameObject.activeInHierarchy)
        {
            _dayLight.gameObject.SetActive(false);
        }
        else if(_dayLight.intensity > 0 && !_dayLight.gameObject.activeInHierarchy)
        {
            _dayLight.gameObject.SetActive(true);
            _dayType = DayType.DayTime;
        }

        //Toggle the Nigth Light
        if (_nightLight.intensity == 0 && _nightLight.gameObject.activeInHierarchy)
        {
            _nightLight.gameObject.SetActive(false);
        }
        else if (_nightLight.intensity > 0 && !_nightLight.gameObject.activeInHierarchy)
        {
            _nightLight.gameObject.SetActive(true);
            _dayType = DayType.NightTime;
        }

        //Sped the time if night
        if(_dayType == DayType.NightTime)
        {
            _speed = 4;
        }
        else
        {
            _speed = 1;
        }

        //Lighing and reflection intensity -> Test "Realistic Night"
        RenderSettings.ambientIntensity = _lightingIntensityMultiplier.Evaluate(_time);
        //RenderSettings.reflectionIntensity = _reflectionIntensityMultiplier.Evaluate(_time);
    }

}
