using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraRoot = null;
    [SerializeField] private Transform _cameraTarget = null;
    [SerializeField] private Transform _cameraTargetPosition = null;

    private Vector3 _deltaMousePosition = Vector3.zero;
    private Vector3 _lastMousePosition = Vector3.zero;
    private float _deltaMouseScroll;

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
            //distance  _cameraTargetPosition.position = _cameraTargetPosition.position + _cameraTargetPosition.forward * _deltaMouseScroll *缩放速度* Time.deltaTime;
            _cameraTargetPosition.position = _cameraTargetPosition.position + _cameraTargetPosition.forward * _deltaMouseScroll * 3000 * Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, _cameraTargetPosition.position, Time.deltaTime);
        transform.LookAt(_cameraTarget, Vector3.up);
    }
}
