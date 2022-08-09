using UnityEngine;

public class LightRotate : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private DayType _dayType = DayType.None;
    [SerializeField] private Quaternion _nightStart;

    private void Update()
    {
        float angle = _speed * Time.deltaTime;
        transform.Rotate(Vector3.up, angle);
        Debug.Log(Quaternion.Angle(gameObject.transform.rotation, _nightStart));
        if(gameObject.transform.rotation == _nightStart)
        {
            
            Debug.Log("crazy");
        }
    }
}