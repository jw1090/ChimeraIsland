using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineCamera : MonoBehaviour
{
    public int speed = 5;
    public Vector3 vect;

    [SerializeField] private Transform _cameraRoot = null;
    [SerializeField] private Transform _cameraTarget = null;
    [SerializeField] private Transform _cameraTargetPosition = null;

    private Vector3 _deltaMousePosition = Vector3.zero;
    private Vector3 _lastMousePosition = Vector3.zero;
    private float _deltaMouseScroll;
    private float xcream;
    private float ycream;

    private void CreamView()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        if(x != 0  || y != 0)
        {
            LimitAngleUandD(60);
            this.transform.Rotate(-y * speed, 0, 0);
            this.transform.Rotate(0, x * speed, 0);
        }
    }
    //Limit camera up and down
    private void LimitAngleUandD(float angle)
    {
        vect = this.transform.eulerAngles;
        ycream = vect.y;
        if(ycream > angle)
        {
            this.transform.rotation = Quaternion.Euler(vect.x, angle, 0);
        }
        else if (ycream < angle)
        {
            this.transform.rotation = Quaternion.Euler(vect.x, -angle, 0);
        }
    }

    private float IsPosNum(float x)
    {
        x -= 180;
        if (x < 0)
            return x + 180;
        else return x - 180;
    }

    private void Update()
    {
        if (_cameraTarget == null) return;

        _deltaMousePosition = Input.mousePosition - _lastMousePosition;
        _lastMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(0) == true)
        {

            //rotate   _cameraRoot.Rotate(0f, _deltaMousePosition.x * Time.deltaTime * rotateNUM, 0f);
            _cameraRoot.Rotate(0f, _deltaMousePosition.x * Time.deltaTime * 10f, 0f);
            _cameraTargetPosition.position = new Vector3(_cameraTargetPosition.position.x, _cameraTargetPosition.position.y - _deltaMousePosition.y * Time.deltaTime, _cameraTargetPosition.position.z);
        }

        _deltaMouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (_deltaMouseScroll != 0)
        {
            //distance  _cameraTargetPosition.position = _cameraTargetPosition.position + _cameraTargetPosition.forward * _deltaMouseScroll *ScrollSpeed* Time.deltaTime;
            _cameraTargetPosition.position = _cameraTargetPosition.position + _cameraTargetPosition.forward * _deltaMouseScroll * 3000 * Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, _cameraTargetPosition.position, Time.deltaTime);
        transform.LookAt(_cameraTarget, Vector3.up);
    }
}
