using System.Collections;
using UnityEngine;

public class CameraUtil : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 20.0f;

    [Header("Zoom")]
    [SerializeField] private float _zoom = 80.0f;
    [SerializeField] private float _zoomAmount = 20.0f;
    [SerializeField] private float _minZoom = 40.0f;
    [SerializeField] private float _maxZoom = 90.0f;

    [Header("Edge Follow")]
    [SerializeField] private float _moveSpeed = 12.0f;
    [SerializeField] private int _screenEdgeSize = 50;

    [Header("Collision")]
    [SerializeField] private float _sphereRadius = 1.5f;
    [SerializeField] private float _offset = 5.0f;
    [SerializeField] private float _resolutionTime = 1.5f;

    private Camera _cameraCO = null;
    private HabitatManager _habitatManager = null;
    private Coroutine _cameraDelayRoutine = null;
    private Rect _upRect = new Rect();
    private Rect _downRect = new Rect();
    private Rect _rightRect = new Rect();
    private Rect _leftRect = new Rect();
    private Vector3 _dir = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;
    private bool _moveUp = false;
    private bool _moveDown = false;
    private bool _moveRight = false;
    private bool _moveLeft = false;
    private bool _canMove = false;
    private float _speedModifier = 0.2f;

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

        _canMove = true;

        return this;
    }

    private void Update()
    {
        CameraMovement();
        ScreenMove();

        CameraZoom();

        CameraCollisionCheck();
    }

    private void CameraMovement()
    {
        float panSpeed = (Input.GetKey(KeyCode.LeftShift)) ? 2 * _speed : _speed;
        Vector3 newPos = transform.position;

        if (_canMove == false)
        {
            panSpeed *= _speedModifier;
        }

        if (Input.GetKey(KeyCode.W))
        {
            newPos.z -= panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            newPos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            newPos.x += panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            newPos.x -= panSpeed * Time.deltaTime;
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
        if (_canMove == false)
        {
            return;
        }

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
    }

    private void CameraCollisionCheck()
    {
        Vector3 newPosition = transform.localPosition;
        bool collision = false;

        if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.forward, out RaycastHit hitFront, _offset))
        {
            if (hitFront.transform.CompareTag("Bounds"))
            {
                newPosition.z = transform.localPosition.z - _offset * 5.0f;
            }

            collision = true;
        }
        else if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.back, out RaycastHit hitBack, _offset))
        {
            if (hitBack.transform.CompareTag("Bounds"))
            {
                newPosition.z = transform.localPosition.z + _offset * 5.0f;
            }

            collision = true;
        }

        if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.right, out RaycastHit hitRight, _offset))
        {
            if (hitRight.transform.CompareTag("Bounds"))
            {
                newPosition.x = transform.localPosition.x - _offset * 5.0f;
            }

            collision = true;
        }
        else if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.left, out RaycastHit hitLeft, _offset))
        {
            if (hitLeft.transform.CompareTag("Bounds"))
            {
                newPosition.x = transform.localPosition.x + _offset * 5.0f;
            }

            collision = true;
        }

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, newPosition, ref _velocity, _resolutionTime);

        if (collision == true)
        {
            _canMove = false;

            if(_cameraDelayRoutine == null)
            {
                _cameraDelayRoutine = StartCoroutine(CameraMoveDelay());
            }
        }
    }

    private IEnumerator CameraMoveDelay()
    {
        yield return new WaitForSeconds(0.35f);

        _canMove = true;
        _cameraDelayRoutine = null;
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