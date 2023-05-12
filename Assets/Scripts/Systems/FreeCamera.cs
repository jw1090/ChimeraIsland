using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    [SerializeField] private float _moveSensitivityChange = 0.5f;
    [SerializeField] private float _turnSensitivityChange = 1.5f;
    [SerializeField] private Camera _cameraCO = null;

    private bool _initialized = false;
    private float _movementSpeed = 0.0f;
    private float _mouseRotationSpeed = 100.0f;

    public Camera CameraCO { get => _cameraCO; }

    public FreeCamera Initialize(float movementSpeed)
    {
        _cameraCO.enabled = false;

        _movementSpeed = movementSpeed;

        _initialized = true;

        return this;
    }

    public void CameraUpdate()
    {
        if (_initialized == false)
        {
            return;
        }

        SensitivityChange();
        CameraMovement();
        MouseMovement();
    }

    private void SensitivityChange()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _movementSpeed += _moveSensitivityChange;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && _movementSpeed > _moveSensitivityChange)
        {
            _movementSpeed -= _moveSensitivityChange;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _mouseRotationSpeed += _turnSensitivityChange;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && _mouseRotationSpeed > _turnSensitivityChange)
        {
            _mouseRotationSpeed -= _turnSensitivityChange;
        }
    }

    private void CameraMovement()
    {
        // Z Movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += (transform.forward * _movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= (transform.forward * _movementSpeed * Time.deltaTime);
        }

        // X Movement
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (transform.right * _movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= (transform.right * _movementSpeed * Time.deltaTime);
        }

        // Y Movement
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += (transform.up * _movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position -= (transform.up * _movementSpeed * Time.deltaTime);
        }
    }

    private void MouseMovement()
    {
        float pitch = Input.GetAxis("Mouse Y") * -_mouseRotationSpeed * Time.deltaTime;
        float yaw = Input.GetAxis("Mouse X") * _mouseRotationSpeed * Time.deltaTime;

        transform.Rotate(pitch, 0, 0, Space.Self);
        transform.Rotate(0, yaw, 0, Space.World);
    }
}