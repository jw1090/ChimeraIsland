using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 20.0f;
    [SerializeField] private Vector3 _pos = Vector3.zero;

    [Header("Zoom")]
    [SerializeField] private float _zoom = 80.0f;
    [SerializeField] private float _zoomAmount = 20.0f;
    [SerializeField] private float _minZoom = 40.0f;
    [SerializeField] private float _maxZoom = 90.0f;

    [Header("CameraFollow")]
    [SerializeField] private float _moveSpeed = 12.0f;
    [SerializeField] private int _screenEdgeSize = 50;

    private Camera _cameraCO = null;
    private Rect _upRect = new Rect();
    private Rect _downRect = new Rect();
    private Rect _rightRect= new Rect();
    private Rect _leftRect = new Rect();
    private Vector3 _dir = Vector3.zero;
    private bool _moveUp = false;
    private bool _moveDown = false;
    private bool _moveRight = false;
    private bool _moveLeft = false;

    public bool IsHolding { get; set; }
    public Camera CameraCO { get => _cameraCO; }

    public CameraController Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _cameraCO = GetComponent<Camera>();

        _upRect = new Rect(1f, Screen.height - _screenEdgeSize, Screen.width, _screenEdgeSize);
        _downRect = new Rect(1f, 1f, Screen.width, _screenEdgeSize);
        _rightRect = new Rect(1f, 1f, _screenEdgeSize, Screen.height);
        _leftRect = new Rect(Screen.width - _screenEdgeSize, 1f, _screenEdgeSize, Screen.height);

        _pos = transform.position;

        return this;
    }

    private void Update()
    {
        ScreenMove();
        CameraMovement();
        CameraZoom();
    }

    private void CameraMovement()
    {
        float panSpeed = (Input.GetKey(KeyCode.LeftShift)) ? 2 * _speed : _speed;

        if (Input.GetKey(KeyCode.W))
        {
            _pos.z -= panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _pos.x += panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _pos.x -= panSpeed * Time.deltaTime;
        }

        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");

        if (_horizontal != 0 || _vertical != 0)
        {
            transform.position = _pos;
        }
    }

    private void CameraZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            _zoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomAmount;
            _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
            CameraCO.fieldOfView = _zoom;
        }
    }

    private void ScreenMove()
    {
        if (IsHolding == false)
        {
            return;
        }

        _moveDown = (_upRect.Contains(Input.mousePosition));
        _moveUp = (_downRect.Contains(Input.mousePosition));
        _moveLeft = (_leftRect.Contains(Input.mousePosition));
        _moveRight = (_rightRect.Contains(Input.mousePosition));

        _dir.z = _moveUp ? 1 : _moveDown ? -1 : 0;
        _dir.x = _moveLeft ? -1 : _moveRight ? 1 : 0;

        transform.position = Vector3.Lerp(transform.position, transform.position + _dir * _moveSpeed, Time.deltaTime);
        _pos = transform.position;
    }
}