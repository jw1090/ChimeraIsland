using System.Collections;
using UnityEngine;

public class CameraUtil : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 20.0f;

    [Header("Zoom")]
    [SerializeField] private float _zoomAmount = 20.0f;
    [SerializeField] private float _minZoom = 40.0f;
    [SerializeField] private float _maxZoom = 90.0f;

    [Header("Edge Follow")]
    [SerializeField] private float _moveSpeed = 12.0f;
    [SerializeField] private int _screenEdgeSize = 50;

    [Header("Collision")]
    [SerializeField] private float _sphereRadius = 1.5f;
    [SerializeField] private float _offset = 5.0f;

    private Camera _cameraCO = null;
    private HabitatManager _habitatManager = null;
    private Rect _upRect = new Rect();
    private Rect _downRect = new Rect();
    private Rect _rightRect = new Rect();
    private Rect _leftRect = new Rect();
    private Vector3 _direction = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;
    private bool _initialized = false;
    private bool _canMoveUp = true;
    private bool _canMoveDown = true;
    private bool _canMoveLeft = true;
    private bool _canMoveRight = true;
    private bool _moveUp = false;
    private bool _moveDown = false;
    private bool _moveRight = false;
    private bool _moveLeft = false;
    private bool _moveSlow = false;
    private float _zoom = 90.0f;

    public bool IsHolding { get; set; }
    public Camera CameraCO { get => _cameraCO; }

    public CameraUtil Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _cameraCO = GetComponent<Camera>();

        _upRect = new Rect(1f, Screen.height - _screenEdgeSize, Screen.width, _screenEdgeSize);
        _downRect = new Rect(1f, 1f, Screen.width, _screenEdgeSize);
        _rightRect = new Rect(1f, 1f, _screenEdgeSize, Screen.height);
        _leftRect = new Rect(Screen.width - _screenEdgeSize, 1f, _screenEdgeSize, Screen.height);

        _initialized = true;

        return this;
    }

    private void Update()
    {
        if(_initialized == false)
        {
            return;
        }

        CameraMovement();
        ScreenMove();

        CameraZoom();

        CameraCollisionCheck();
    }

    private void CameraMovement()
    {
        float panSpeed = (Input.GetKey(KeyCode.LeftShift)) ? 2 * _speed : _speed;
        Vector3 newPos = transform.position;

        if(_moveSlow == true)
        {
            panSpeed *= 0.25f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (_canMoveUp)
            {
                newPos.z -= panSpeed * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (_canMoveDown)
            {
                newPos.z += panSpeed * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (_canMoveLeft)
            {
                newPos.x += panSpeed * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (_canMoveRight)
            {
                newPos.x -= panSpeed * Time.deltaTime;
            }
        }

        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");

        if (_horizontal != 0 || _vertical != 0)
        {
            transform.position = newPos;
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

        _direction.x = _moveLeft ? -1 : _moveRight ? 1 : 0;
        _direction.z = _moveUp ? 1 : _moveDown ? -1 : 0;

        if (_moveSlow == true)
        {
            _direction.x *= 0.25f;
            _direction.z *= 0.25f;
        }

        transform.position = Vector3.Lerp(transform.position, transform.position + _direction * _moveSpeed, Time.deltaTime);
    }

    private void CameraCollisionCheck()
    {
        Vector3 newPosition = transform.localPosition;
        RaycastHit hit;
        float pushback = _offset * 10.0f;

        if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.forward, out hit, _offset))
        {
            if (hit.transform.CompareTag("Bounds"))
            {
                newPosition.z = transform.localPosition.z - pushback;

                _canMoveDown = false;
            }
        }
        else
        {
            _canMoveDown = true;
        }

        if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.back, out hit, _offset))
        {
            if (hit.transform.CompareTag("Bounds"))
            {
                newPosition.z = transform.localPosition.z + pushback;

                _canMoveUp = false;
            }
        }
        else
        {
            _canMoveUp = true;
        }

        if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.right, out hit, _offset))
        {
            if (hit.transform.CompareTag("Bounds"))
            {
                newPosition.x = transform.localPosition.x - pushback;

                _canMoveLeft = false;
            }
        }
        else
        {
            _canMoveLeft = true;
        }

        if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.left, out hit, _offset))
        {
            if (hit.transform.CompareTag("Bounds"))
            {
                newPosition.x = transform.localPosition.x + pushback;

                _canMoveRight = false;
            }
        }
        else
        {
            _canMoveRight = true;
        }

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, 1.2f);

        if(_canMoveDown && _canMoveLeft && _canMoveRight && _canMoveUp)
        {
            _moveSlow = false;
        }
    }

    public void FacilityCameraShift(FacilityType facilityType)
    {
        HabitatType habitatType = _habitatManager.CurrentHabitat.Type;

        switch (habitatType)
        {
            case HabitatType.StonePlains:
                switch (facilityType)
                {
                    case FacilityType.Cave:
                        StartCoroutine(MoveCamera(new Vector3(-18.0f, 20.0f, -7.0f), 0.25f));
                        break;
                    case FacilityType.Waterfall:
                        StartCoroutine(MoveCamera(new Vector3(41.0f, 20.0f, 51.0f), 0.25f));
                        break;
                    case FacilityType.RuneStone:
                        StartCoroutine(MoveCamera(new Vector3(16.0f, 20.0f, 39.0f), 0.25f));
                        break;
                    default:
                        Debug.LogWarning($"Invalid Facility Type [{facilityType}], please change!");
                        break;
                }
                break;
            case HabitatType.TreeOfLife:
                switch (facilityType)
                {
                    case FacilityType.Cave:
                        StartCoroutine(MoveCamera(new Vector3(16.0f, 24.0f, 44.0f), 0.5f));
                        break;
                    case FacilityType.Waterfall:
                        StartCoroutine(MoveCamera(new Vector3(60.0f, 24.0f, 20.0f), 0.5f));
                        break;
                    case FacilityType.RuneStone:
                        StartCoroutine(MoveCamera(new Vector3(-66.0f, 24.0f, 12.0f), 0.5f));
                        break;
                    default:
                        Debug.LogWarning($"Invalid Facility Type [{facilityType}], please change!");
                        break;
                }
                break;
            default:
                Debug.LogWarning($"Invalid Habitat Type [{habitatType}], please change!");
                break;
        }
    }

    public void ChimeraCameraShift()
    {
        HabitatType habitatType = _habitatManager.CurrentHabitat.Type;

        switch (habitatType)
        {
            case HabitatType.StonePlains:
                StartCoroutine(MoveCamera(new Vector3(18.0f, 20.0f, 16.6f), 0.25f));
                break;
            case HabitatType.TreeOfLife:
                StartCoroutine(MoveCamera(new Vector3(-5.6f, 24.0f, 5.5f), 0.5f));
                break;
            default:
                Debug.LogWarning($"Invalid Habitat Type [{habitatType}], please change!");
                break;
        }
    }

    private IEnumerator MoveCamera(Vector3 target, float time)
    {
        while (Vector3.Distance(transform.position, target) > 0.5f)
        {
            yield return new WaitForSeconds(0.01f);

            transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, time);
        }
    }
}