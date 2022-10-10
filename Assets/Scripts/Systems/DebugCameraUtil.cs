using System.Collections;
using UnityEngine;

public class DebugCameraUtil : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 20.0f;
    [SerializeField] private float _sprintMultiplier = 1.5f;

    [Header("Zoom")]
    [SerializeField] private float _zoomAmount = 20.0f;
    [SerializeField] private float _minZoom = 20.0f;
    [SerializeField] private float _maxZoom = 90.0f;

    [Header("Edge Follow")]
    [SerializeField] private float _moveSpeed = 12.0f;

    private Camera _camera = null;
    private bool _initialized = false;
    private float _zoom = 90.0f;
    private float _pitchSpeed = 100.0f;
    private float _yawSpeed = 100.0f;
    public Camera Camera { get => _camera; }

    public DebugCameraUtil Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _camera = transform.gameObject.GetComponent<Camera>();

        _initialized = true;

        return this;
    }

    public void Update()
    {
        float pitch = Input.GetAxis("Mouse Y") * _pitchSpeed * Time.deltaTime;
        float yaw = Input.GetAxis("Mouse X") * _yawSpeed * Time.deltaTime;

        transform.Rotate(-pitch, 0, 0, Space.Self);
        transform.Rotate(0, yaw, 0, Space.World);
    }

    public void CameraZoom()
    {
        _zoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomAmount;
        _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
        Camera.fieldOfView = _zoom;
    }

    public void CameraMovement()
    {
        if (_initialized == false)
        {
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow)) _moveSpeed += 0.01f;
        if (Input.GetKey(KeyCode.DownArrow) && _moveSpeed > 0.01f) _moveSpeed -= 0.01f;

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += (transform.forward * _moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += (-transform.forward * _moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += (-transform.right * _moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (transform.right * _moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += (transform.up * _moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += (-transform.up * _moveSpeed * Time.deltaTime);
        }
        float pitch = Input.GetAxis("Mouse Y") * _pitchSpeed * Time.deltaTime;
        float yaw = Input.GetAxis("Mouse X") * _yawSpeed * Time.deltaTime;

        transform.Rotate(pitch, 0, 0, Space.Self);
        transform.Rotate(0, yaw, 0, Space.World);
    }
}