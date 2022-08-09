using UnityEngine;

public class LightRotate : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private DayType _dayType = DayType.None;
    [SerializeField] private Quaternion _nightStart;
    [SerializeField] private Quaternion _dayStart;
    [SerializeField] private Vector3 _currentEulerAngle;
    [SerializeField] private Light _light = null;

    public DayType DayType { get => _dayType; }

    private void Update()
    {
        _currentEulerAngle += Time.deltaTime * Vector3.right * _speed;
        //transform.Rotate(Vector3.up, _speed * Time.deltaTime); 

        transform.eulerAngles = _currentEulerAngle;
        if(transform.eulerAngles.x >= _nightStart.eulerAngles.x + 140)
        {
            _dayType = DayType.NightTime;
            if (_dayType == DayType.NightTime)
            {
                _speed = 4;
                _light.gameObject.SetActive(true);
            }
        }
        else
        {
            _dayType = DayType.DayTime;
            if(_dayType == DayType.DayTime)
            {
                _speed = 2;
                _light.gameObject.SetActive(false);
            }
        }
    }
}