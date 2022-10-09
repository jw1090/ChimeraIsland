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
    [SerializeField] private int _screenEdgeSize = 50;

    [Header("Collision")]
    [SerializeField] private float _sphereRadius = 2.0f;
    [SerializeField] private float _offset = 1.5f;

    private Camera _camera = null;
    private Coroutine _transitionCoroutine = null;
    private HabitatManager _habitatManager = null;
    private InputManager _inputManager = null;
    private Rect _upRect = new Rect();
    private Rect _downRect = new Rect();
    private Rect _rightRect = new Rect();
    private Rect _leftRect = new Rect();
    private bool _initialized = false;
    private bool _canMoveForward = true;
    private bool _canMoveBackward = true;
    private bool _canMoveLeft = true;
    private bool _canMoveRight = true;
    private bool _canMoveUp = true;
    private bool _canMoveDown = true;
    private float _zoom = 90.0f;
    private float _pitchSpeed = 100.0f;
    private float _yawSpeed = 100.0f;

    public bool IsHolding { get; set; }
    public Camera Camera { get => _camera; }

    public DebugCameraUtil Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _inputManager = ServiceLocator.Get<InputManager>();
        _camera = transform.gameObject.GetComponent<Camera>();

        _upRect = new Rect(1f, Screen.height - _screenEdgeSize, Screen.width, _screenEdgeSize);
        _downRect = new Rect(1f, 1f, Screen.width, _screenEdgeSize);
        _rightRect = new Rect(1f, 1f, _screenEdgeSize, Screen.height);
        _leftRect = new Rect(Screen.width - _screenEdgeSize, 1f, _screenEdgeSize, Screen.height);

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

        Vector3 direction = Vector3.zero;
        float panSpeed = (Input.GetKey(KeyCode.LeftShift)) ? _sprintMultiplier * _speed : _speed;

        bool moveForward = Input.GetKey(KeyCode.W) && _canMoveForward;
        bool moveBackward = Input.GetKey(KeyCode.S) && _canMoveBackward;
        bool moveLeft = Input.GetKey(KeyCode.A) && _canMoveLeft;
        bool moveRight = Input.GetKey(KeyCode.D) && _canMoveRight;
        bool moveDown = Input.GetKey(KeyCode.Q) && _canMoveDown;
        bool moveUp = Input.GetKey(KeyCode.E) && _canMoveUp;

        direction.z = moveForward ? -1 : moveBackward ? 1 : 0;
        direction.x = moveLeft ? 1 : moveRight ? -1 : 0;
        direction.y = moveUp ? -1 : moveDown ? 1 : 0;
        transform.position = transform.position + direction * panSpeed * Time.deltaTime;

        float pitch = Input.GetAxis("Mouse Y") * _pitchSpeed * Time.deltaTime;
        float yaw = Input.GetAxis("Mouse X") * _yawSpeed * Time.deltaTime;

        transform.Rotate(pitch, 0, 0, Space.Self);
        transform.Rotate(0, yaw, 0, Space.World);
    }
}